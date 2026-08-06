using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using com.spacepuppy.Collections;

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
        public class SaveGameDictionary : SerializableDictionaryBase<string, ISaveData>
        {
        }

        [System.Serializable]
        public class LoadGameDictionary : SerializableDictionaryBase<string, string>
        {
        }

        const string MANDATORY_SAVE_NAME = "mwovjtpamcjaytifnhyqlbprths";
        const string OPTIONAL_SAVE_NAME = "nalgowuthvnapqyewngoapwvz";

        public delegate object ObjectDataCallback();

        public delegate void StringDataCallback(string data);

        [UnityEngine.SerializeField()] private SaveGameDictionary mMandatory = new SaveGameDictionary();
        [UnityEngine.SerializeField()] private SaveGameDictionary mOptional = new SaveGameDictionary();

        public void RegisterMandatoryData(string name, ISaveData data)
        {
            mMandatory[name] = data;
        }

        public void RegisterOptionalData(string name, ISaveData data)
        {
            mOptional[name] = data;
        }

        public void Save(bool mandatory = true, bool optional = true, bool hasBackup = true)
        {
            if (mandatory)
            {
                try
                {
                    bool hasChanged = false;
                    foreach (string key in mMandatory.Keys)
                    {
                        hasChanged |= mMandatory[key].DataChanged;
                    }

                    if (hasChanged)
                    {
                        LoadGameDictionary temp = new LoadGameDictionary();
                        bool checkValid = false;
                        foreach (string key in mMandatory.Keys)
                        {
                            temp[key] = JsonUtility.ToJson(mMandatory[key].GetData());
                            checkValid = true;
                        }

                        if (checkValid)
                        {
                            // Chỉ hạ cờ SAU khi ghi xong: hạ trước mà ghi hỏng thì lần Save sau thấy
                            // "không có gì đổi" và dữ liệu mất vĩnh viễn.
                            SaveToFile(MANDATORY_SAVE_NAME, JsonUtility.ToJson(temp), hasBackup);
                            foreach (string key in mMandatory.Keys)
                            {
                                mMandatory[key].DataChanged = false;
                            }
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
                    bool hasChanged = false;
                    foreach (string key in mOptional.Keys)
                    {
                        hasChanged |= mOptional[key].DataChanged;
                    }

                    if (hasChanged)
                    {
                        LoadGameDictionary temp = new LoadGameDictionary();
                        bool checkValid = false;
                        foreach (string key in mOptional.Keys)
                        {
                            temp[key] = JsonUtility.ToJson(mOptional[key].GetData());
                            checkValid = true;
                        }

                        if (checkValid)
                        {
                            SaveToFile(OPTIONAL_SAVE_NAME, JsonUtility.ToJson(temp), hasBackup);
                            foreach (string key in mOptional.Keys)
                            {
                                mOptional[key].DataChanged = false;
                            }
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
            LoadGameDictionary loadDictionary = null;
            if (mandatory)
            {
                try
                {
                    string data = null;
                    if (!LoadFromFile(MANDATORY_SAVE_NAME, ref data, true))
                    {
                        LoadFromFile("_" + MANDATORY_SAVE_NAME, ref data, true);
                    }

                    if (string.IsNullOrEmpty(data))
                    {
                        loadDictionary = new LoadGameDictionary();
                    }
                    else
                    {
                        loadDictionary = JsonUtility.FromJson<LoadGameDictionary>(data);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("[SaveGameManager] Doc du lieu bat buoc that bai, dung mac dinh: " + e);
                    loadDictionary = null;
                }

                foreach (string key in mMandatory.Keys)
                {
                    mMandatory[key]
                        .SetData(loadDictionary != null && loadDictionary.ContainsKey(key) &&
                                 loadDictionary[key] != null
                            ? loadDictionary[key]
                            : "");
                }
            }

            if (optional)
            {
                try
                {
                    string data = null;
                    if (!LoadFromFile(OPTIONAL_SAVE_NAME, ref data, false))
                    {
                        LoadFromFile("_" + OPTIONAL_SAVE_NAME, ref data, false);
                    }

                    if (string.IsNullOrEmpty(data))
                    {
                        loadDictionary = new LoadGameDictionary();
                    }
                    else
                    {
                        loadDictionary = JsonUtility.FromJson<LoadGameDictionary>(data);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("[SaveGameManager] Doc du lieu tuy chon that bai, dung mac dinh: " + e);
                    loadDictionary = null;
                }

                foreach (string key in mOptional.Keys)
                {
                    mOptional[key]
                        .SetData(loadDictionary != null && loadDictionary.ContainsKey(key) &&
                                 loadDictionary[key] != null
                            ? loadDictionary[key]
                            : "");
                }
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

        public bool SaveToFile(string fileName, string data, bool hasBackup = true)
        {
            PlayerPrefs.SetString(fileName, data);
            return true;
        }

        public bool LoadFromFile(string fileName, ref string data, bool hasBackup = false)
        {
            data = PlayerPrefs.GetString(fileName);
            return true;
        }

        public void DeleteAll()
        {
            DeleteSave(MANDATORY_SAVE_NAME);
            DeleteSave(OPTIONAL_SAVE_NAME);
            mMandatory.Clear();
            mOptional.Clear();
        }

        public bool DeleteSave(string fileName)
        {
            PlayerPrefs.DeleteKey(fileName);
            return true;
        }
    }
}