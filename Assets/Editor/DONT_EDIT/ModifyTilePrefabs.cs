using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

using System.Text;

public class ModifyTilePrefabs : EditorWindow
{

    TextAsset tilePrefabsMap;

    Dictionary<string, GameObject> prefabDict;
    HashSet<string> prefabTypesNeeded;
    Dictionary<string, List<GameObject>> changesToBeMade;

    [MenuItem("494/2) Modify Tile Prefabs", false, 2)]
    public static void Generate()
    {
        EditorWindow.GetWindowWithRect(typeof(ModifyTilePrefabs), new Rect(0, 0, 700, 400));
    }

    void OnGUI()
    {
        // Check for errors
        GameObject rootGO = GameObject.Find("Level");
        if (rootGO == null)
        {
            GUILayout.Label("There is no Level GameObject in this scene. The script can not continue.", EditorStyles.boldLabel);
        }
        else
        {
            GUILayout.Label("Change tile prefabs in current scene", EditorStyles.boldLabel);
            tilePrefabsMap = (TextAsset)EditorGUILayout.ObjectField("Tile Prefabs Map:", tilePrefabsMap, typeof(TextAsset), false);
            if (tilePrefabsMap == null)
            {
                GUILayout.Label("Tile Prefab data not provided!\nAdd Tile prefab file to continue..", EditorStyles.boldLabel);
            }
            else if (GUILayout.Button("Check Updates."))
            {
                Check(rootGO.transform);
                //this.Close();
            }
            if (prefabTypesNeeded != null)
            {
                GUILayout.Label("", EditorStyles.boldLabel);

                string toBeGeneratedMessage = "";
                bool willCreateNewPrefabs = (prefabTypesNeeded.Count != 0);
                if (willCreateNewPrefabs)
                {
                    toBeGeneratedMessage = "The following new prefabs will be generated: ";
                    foreach (string prefabType in prefabTypesNeeded)
                    {
                        toBeGeneratedMessage += "\n - " + EditorUtilityFunctions.tilePrefix + prefabType;
                    }
                }
                else
                {
                    toBeGeneratedMessage = "No new tile prefabs will be generated";
                }
                GUILayout.Label(toBeGeneratedMessage, EditorStyles.label);

                string toBeChangedMessage = "";
                bool willMakeChangesToTiles = (changesToBeMade.Count != 0);
                if (willMakeChangesToTiles)
                {
                    toBeChangedMessage = "The following changes will be made to the tiles marked in red in the scene view:";
                    foreach (KeyValuePair<string, List<GameObject>> entry in changesToBeMade)
                    {
                        toBeChangedMessage += "\n - " + entry.Value.Count + " tiles will be changed to be of prefab " + EditorUtilityFunctions.tilePrefix + entry.Key;
                    }
                }
                else
                {
                    toBeChangedMessage = "No changes to tiles in the scene will be made";
                }
                GUILayout.Label(toBeChangedMessage, EditorStyles.label);

                GUILayout.Label("", EditorStyles.boldLabel);

                if (willCreateNewPrefabs || willMakeChangesToTiles)
                {
                    GUILayout.Label("This script works by deleting these tiles and remaking them with another prefab.", EditorStyles.label);
                    GUILayout.Label("This script has the potential to DELETE objects in your scene.", EditorStyles.label);
                    GUILayout.Label("Ensure you have committed recently!", EditorStyles.boldLabel);
                    GUILayout.Label("By pressing \"I Accept\" you are acknowledging you understand these potential risks.", EditorStyles.label);
                    if (GUILayout.Button("I Accept."))
                    {
                        Execute();
                        this.Close();
                    }
                }
                else
                {
                    GUILayout.Label("No changes will be made, so the script will not run.", EditorStyles.label);
                    GUILayout.Label("Have you updated your TilePrefabMap file?", EditorStyles.label);
                }
            }
        }
    }

    void OnEnable()
    {
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
    }

    void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }
    void OnSceneGUI(SceneView sceneView)
    {
        if (changesToBeMade == null)
            return;
        foreach (KeyValuePair<string, List<GameObject>> entry in changesToBeMade)
        {
            foreach (GameObject g in entry.Value)
            {
                Handles.DrawSolidRectangleWithOutline(new Rect(g.transform.position.x - .5f, g.transform.position.y - .5f, 1, 1), Color.red * .5f, Color.red);
            }
        }
    }

    void Check(Transform root)
    {
        // Load all tile prefab assets into a dict
        prefabDict = new Dictionary<string, GameObject>();
        List<GameObject> prefabs = EditorUtilityFunctions.GetTilePrefabs();
        foreach (GameObject prefab in prefabs)
        {
            string prefabKey = prefab.name.Substring(EditorUtilityFunctions.tilePrefix.Length); // Get the part after Tile_, like WALL
            prefabDict.Add(prefabKey, prefab);
        }

        // Parse input txt file
        string[] prefabByIndexArray = tilePrefabsMap.text.Split(new char[] { ' ', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        // Remove inconsistent line endings.
        for (int i = 0; i < prefabByIndexArray.Length; i++)
        {
            prefabByIndexArray[i] = prefabByIndexArray[i].TrimEnd('\n', '\r');
        }

        // Check to see if any new prefabs are needed
        prefabTypesNeeded = new HashSet<string>();
        foreach (string prefabType in prefabByIndexArray)
        {
            if (prefabType == "NULL" || prefabDict.ContainsKey(prefabType) || prefabTypesNeeded.Contains(prefabType))
                continue;

            
            prefabTypesNeeded.Add(prefabType);
        }

        changesToBeMade = new Dictionary<string, List<GameObject>>();
        foreach (Transform room in root)
        {
            foreach (Transform child in room)
            {
                if (!child.name.StartsWith(EditorUtilityFunctions.tilePrefix)) // Only worrying about tiles
                    continue;
                SpriteRenderer childSpriteRenderer = child.GetComponent<SpriteRenderer>();
                if (childSpriteRenderer == null)
                    continue;
				if (childSpriteRenderer.sprite == null)
					continue;
                string idAsString = childSpriteRenderer.sprite.name.Substring(EditorUtilityFunctions.spriteSheetIDPrefix.Length);
                int id = int.Parse(idAsString);

                GameObject currentPrefab = (GameObject)PrefabUtility.GetCorrespondingObjectFromSource(child.gameObject);
                string currentType = currentPrefab.name.Substring(EditorUtilityFunctions.tilePrefix.Length);
                string targetType = prefabByIndexArray[id];

                if (currentType == targetType)
                    continue;

                if (!changesToBeMade.ContainsKey(targetType))
                {
                    changesToBeMade.Add(targetType, new List<GameObject>());
                }
                changesToBeMade[targetType].Add(child.gameObject);
            }
        }
    }

    /* Necessary Due to some bizarre comparison issues with strings in .net */
    bool ListContainsItem(List<string> l, string s)
    {
        for (int i = 0; i < l.Count; i++)
        {
            if (l[i].CompareTo(s) == 0)
                return true;
        }

        return false;
    }

    bool DictKeysContainsKey(List<string> keys, string s)
    {
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i].CompareTo(s) == 0)
                return true;
        }

        return false;
    }

    void Execute()
    {
        // Generate new prefabs
        foreach (string type in prefabTypesNeeded)
        {
            GameObject newPrefab = EditorUtilityFunctions.GenerateNewTilePrefab(type);
            prefabDict.Add(type, newPrefab);
        }

        // Update tiles
        foreach (KeyValuePair<string, List<GameObject>> entry in changesToBeMade)
        {
            if (entry.Key == "NULL")
                continue;

            GameObject prefab = prefabDict[entry.Key];
            foreach (GameObject oldTile in entry.Value)
            {
                if (oldTile == null)
                    continue;

                GameObject newTile = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                newTile.transform.position = oldTile.transform.position;
                newTile.GetComponent<SpriteRenderer>().sprite = oldTile.GetComponent<SpriteRenderer>().sprite;
                newTile.transform.parent = oldTile.transform.parent;
                DestroyImmediate(oldTile);
            }
        }
    }
}
