using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace Thnguyet.SaveGame
{
    public interface ISaveData
    {
        object GetData();
        void SetData(string data);
        void OnAllDataLoaded();
        void RegisterSaveData();
        bool DataChanged { get; set; }
    }

    public class SaveGameManager : Thnguyet.Singleton<SaveGameManager>
    {
        [System.Serializable]
        private class SaveDataEntry
        {
            public string key;
            public string json;
        }

        [System.Serializable]
        private class SaveDataPayload
        {
            public string version = "1";
            public List<SaveDataEntry> mandatory = new List<SaveDataEntry>();
            public List<SaveDataEntry> optional = new List<SaveDataEntry>();
        }

        const string MANDATORY_SAVE_NAME = "mwovjtpamcjaytifnhyqlbprths";
        const string OPTIONAL_SAVE_NAME = "nalgowuthvnapqyewngoapwvz";

        public delegate object ObjectDataCallback();

        public delegate void StringDataCallback(string data);

        private readonly Dictionary<string, ISaveData> mMandatory = new Dictionary<string, ISaveData>();
        private readonly Dictionary<string, ISaveData> mOptional = new Dictionary<string, ISaveData>();

        public void RegisterMandatoryData(string name, ISaveData data)
        {
            if (string.IsNullOrWhiteSpace(name) || data == null)
                return;

            mMandatory[name] = data;
        }

        public void RegisterOptionalData(string name, ISaveData data)
        {
            if (string.IsNullOrWhiteSpace(name) || data == null)
                return;

            mOptional[name] = data;
        }

        public void Save(bool mandatory = true, bool optional = true, bool hasBackup = true)
        {
            if (mandatory)
            {
                try
                {
                    var payload = new SaveDataPayload();
                    bool hasChanged = false;
                    foreach (var item in mMandatory.Values)
                    {
                        hasChanged |= item.DataChanged;
                    }

                    if (!hasChanged)
                        return;

                    foreach (var kv in mMandatory)
                    {
                        var value = kv.Value;
                        if (value == null)
                            continue;

                        payload.mandatory.Add(new SaveDataEntry
                        {
                            key = kv.Key,
                            json = JsonUtility.ToJson(value.GetData())
                        });
                    }

                    if (payload.mandatory.Count > 0)
                    {
                        SaveToFile(MANDATORY_SAVE_NAME, JsonUtility.ToJson(payload), hasBackup);
                        foreach (var item in mMandatory.Values)
                        {
                            item.DataChanged = false;
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("[SaveGameManager] Luu du lieu bat buoc that bai, se thu lai o lan Save sau: " + e);
                }
            }

            if (optional)
            {
                try
                {
                    var payload = new SaveDataPayload();
                    bool hasChanged = false;
                    foreach (var item in mOptional.Values)
                    {
                        hasChanged |= item.DataChanged;
                    }

                    if (!hasChanged)
                        return;

                    foreach (var kv in mOptional)
                    {
                        var value = kv.Value;
                        if (value == null)
                            continue;

                        payload.optional.Add(new SaveDataEntry
                        {
                            key = kv.Key,
                            json = JsonUtility.ToJson(value.GetData())
                        });
                    }

                    if (payload.optional.Count > 0)
                    {
                        SaveToFile(OPTIONAL_SAVE_NAME, JsonUtility.ToJson(payload), hasBackup);
                        foreach (var item in mOptional.Values)
                        {
                            item.DataChanged = false;
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("[SaveGameManager] Luu du lieu tuy chon that bai, se thu lai o lan Save sau: " + e);
                }
            }
        }

        public void Load(bool mandatory = true, bool optional = true, bool notification = true)
        {
            if (mandatory)
            {
                ApplyLoadedData(MANDATORY_SAVE_NAME, true, mMandatory);
            }

            if (optional)
            {
                ApplyLoadedData(OPTIONAL_SAVE_NAME, false, mOptional);
            }

            if (notification)
            {
                if (mandatory)
                {
                    foreach (var item in mMandatory.Values)
                    {
                        item.OnAllDataLoaded();
                    }
                }

                if (optional)
                {
                    foreach (var item in mOptional.Values)
                    {
                        item.OnAllDataLoaded();
                    }
                }
            }
        }

        private void ApplyLoadedData(string fileName, bool hasBackup, Dictionary<string, ISaveData> target)
        {
            string data = null;
            if (!LoadFromFile(fileName, ref data, hasBackup))
            {
                if (hasBackup)
                {
                    LoadFromFile("_" + fileName, ref data, false);
                }
            }

            var payload = string.IsNullOrEmpty(data)
                ? null
                : JsonUtility.FromJson<SaveDataPayload>(data);

            if (payload == null)
            {
                payload = new SaveDataPayload();
            }

            foreach (var kv in target)
            {
                if (kv.Value == null)
                    continue;

                string json = null;
                var entries = hasBackup && fileName == MANDATORY_SAVE_NAME ? payload.mandatory : payload.optional;
                if (fileName == OPTIONAL_SAVE_NAME)
                {
                    entries = payload.optional;
                }

                if (entries != null)
                {
                    foreach (var entry in entries)
                    {
                        if (entry != null && entry.key == kv.Key)
                        {
                            json = entry.json;
                            break;
                        }
                    }
                }

                kv.Value.SetData(json ?? "");
            }
        }

        private static string GetSaveFolder()
        {
            var folder = Path.Combine(Application.persistentDataPath, "SaveGame");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return folder;
        }

        private static string GetSaveFilePath(string fileName)
        {
            return Path.Combine(GetSaveFolder(), fileName + ".json");
        }

        public bool SaveToFile(string fileName, string data, bool hasBackup = true)
        {
            try
            {
                var savePath = GetSaveFilePath(fileName);
                if (hasBackup && File.Exists(savePath))
                {
                    var backupPath = GetSaveFilePath("_" + fileName);
                    File.Copy(savePath, backupPath, true);
                }

                File.WriteAllText(savePath, data);
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError("[SaveGameManager] SaveToFile failed: " + e);
                return false;
            }
        }

        public bool LoadFromFile(string fileName, ref string data, bool hasBackup = false)
        {
            try
            {
                var savePath = GetSaveFilePath(fileName);
                if (File.Exists(savePath))
                {
                    data = File.ReadAllText(savePath);
                    return true;
                }

                if (hasBackup)
                {
                    var backupPath = GetSaveFilePath("_" + fileName);
                    if (File.Exists(backupPath))
                    {
                        data = File.ReadAllText(backupPath);
                        return true;
                    }
                }

                data = string.Empty;
                return false;
            }
            catch (System.Exception e)
            {
                Debug.LogError("[SaveGameManager] LoadFromFile failed: " + e);
                data = string.Empty;
                return false;
            }
        }

        public void DeleteAll()
        {
            Debug.Log("[SaveGameManager] DELETE ALL SAVE");

            DeleteSave(MANDATORY_SAVE_NAME);
            DeleteSave(OPTIONAL_SAVE_NAME);
            DeleteSave("_" + MANDATORY_SAVE_NAME);
            DeleteSave("_" + OPTIONAL_SAVE_NAME);

            mMandatory.Clear();
            mOptional.Clear();

            Debug.Log("[SaveGameManager] ALL SAVE DELETED");
        }

        public bool DeleteSave(string fileName)
        {
            try
            {
                var savePath = GetSaveFilePath(fileName);
                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                    Debug.Log($"[SaveGameManager] Deleted save: {fileName}");
                    return true;
                }

                Debug.LogWarning($"[SaveGameManager] Save not found: {fileName}");
                return false;
            }
            catch (System.Exception e)
            {
                Debug.LogError("[SaveGameManager] DeleteSave failed: " + e);
                return false;
            }
        }
    }
}