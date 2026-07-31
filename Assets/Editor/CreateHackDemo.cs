#if UNITY_EDITOR
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Editor utility: creates node prefabs and a HackLevel asset for a playable demo.
public static class CreateHackDemo
{
    [MenuItem("Hacking/Create Sample HackLevel")]
    public static void CreateSampleLevel()
    {
        string basePath = "Assets/HackingDemo";
        if (!Directory.Exists(basePath))
            Directory.CreateDirectory(basePath);

        // Define nodes to create with type, id and UI positions
        var nodeDefs = new List<(string name, HackNodeType type, int id, Vector2 pos)>
        {
            ("Node_Start", HackNodeType.Start, 0, new Vector2(-300,0)),
            ("Node_Normal", HackNodeType.Normal, 1, new Vector2(-100,60)),
            ("Node_Key", HackNodeType.Key, 2, new Vector2(0,0)),
            ("Node_Firewall", HackNodeType.Firewall, 3, new Vector2(100,60)),
            ("Node_End", HackNodeType.End, 4, new Vector2(300,0)),
        };

        var prefabPaths = new List<string>();
        var hackNodes = new List<HackNode>();

        // Create prefabs
        foreach (var def in nodeDefs)
        {
            var go = new GameObject(def.name);
            var node = go.AddComponent<HackNode>();

            // Set private serialized fields via SerializedObject so they persist on prefab
            var so = new SerializedObject(node);
            so.FindProperty("nodeType").intValue = (int)def.type;
            so.FindProperty("id").intValue = def.id;
            so.FindProperty("uiPosition").vector2Value = def.pos;
            so.ApplyModifiedPropertiesWithoutUndo();

            string prefabPath = Path.Combine(basePath, def.name + ".prefab").Replace("\\", "/");
            var prefab = PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
            prefabPaths.Add(prefabPath);
            hackNodes.Add(prefab.GetComponent<HackNode>());

            Object.DestroyImmediate(go);
        }

        // Assign neighbors on the prefab components (will serialize cross-prefab references)
        // We load prefabs as assets and set their HackNode.neighbors property via SerializedObject.
        // Connections: Start -> Normal; Normal -> Key; Key -> Firewall; Firewall -> End
        var assetNodes = new List<HackNode>();
        foreach (var p in prefabPaths)
        {
            var prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(p);
            assetNodes.Add(prefabAsset.GetComponent<HackNode>());
        }

        // Helper to set neighbors list
        void SetNeighbors(HackNode source, params HackNode[] targets)
        {
            var so = new SerializedObject(source);
            var neighborsProp = so.FindProperty("neighbors");
            neighborsProp.ClearArray();
            for (int i = 0; i < targets.Length; i++)
            {
                neighborsProp.InsertArrayElementAtIndex(i);
                neighborsProp.GetArrayElementAtIndex(i).objectReferenceValue = targets[i];
            }
            so.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(source);
        }

        // Map nodes by name
        var map = new Dictionary<string, HackNode>();
        foreach (var n in assetNodes)
            map[n.name] = n;

        // Set neighbor topology
        // Start -> Normal
        SetNeighbors(map["Node_Start"], map["Node_Normal"]);
        // Normal -> Key
        SetNeighbors(map["Node_Normal"], map["Node_Key"]);
        // Key -> Firewall
        SetNeighbors(map["Node_Key"], map["Node_Firewall"]);
        // Firewall -> End
        SetNeighbors(map["Node_Firewall"], map["Node_End"]);

        // Create HackLevel asset
        var level = ScriptableObject.CreateInstance<HackLevel>();
        var levelPath = Path.Combine(basePath, "SampleHackLevel.asset").Replace("\\", "/");

        // Assign nodes to level via SerializedObject
        var levelSO = new SerializedObject(level);
        levelSO.FindProperty("levelName").stringValue = "SampleLevel";
        levelSO.FindProperty("timeLimit").floatValue = 60f;

        var nodesProp = levelSO.FindProperty("nodes");
        nodesProp.ClearArray();
        int idx = 0;
        foreach (var n in assetNodes)
        {
            nodesProp.InsertArrayElementAtIndex(idx);
            nodesProp.GetArrayElementAtIndex(idx).objectReferenceValue = n;
            idx++;
        }

        // startNode and endNode
        levelSO.FindProperty("startNode").objectReferenceValue = map["Node_Start"];
        levelSO.FindProperty("endNode").objectReferenceValue = map["Node_End"];

        levelSO.ApplyModifiedPropertiesWithoutUndo();

        AssetDatabase.CreateAsset(level, levelPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Sample HackLevel", $"Created sample level and prefabs under {basePath}", "OK");
    }
}
#endif
