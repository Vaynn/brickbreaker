using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;


public class LevelEditorSaver : MonoBehaviour
{
    public string dirName = "StreamingAssets/LevelsJson";
    public string fileName = "level1.json";

    [System.Serializable]
    public class BrickData{
        public Vector2 position;
        public string prefabName;
    }

    [System.Serializable]
    public class LevelData{
        public List<BrickData> bricks = new List<BrickData>();
    }

    public void SaveLevel(){
        LevelData level = new LevelData();

        foreach (Transform child in transform)
        {
            BrickData brick = new BrickData();
            brick.position = child.position;
            brick.prefabName = child.name.Replace("(Clone)", "").Trim().Split(' ')[0];
            level.bricks.Add(brick);
        }
        string json = JsonUtility.ToJson(level, true);
        string fullPath = Path.Combine(Application.dataPath, dirName);
        Directory.CreateDirectory(fullPath); // Crée le dossier s’il n'existe pas
        File.WriteAllText(Path.Combine(fullPath, fileName), json);
        Debug.Log("Niveau sauvegardé avec " + level.bricks.Count + " briques !");
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(LevelEditorSaver))]
public class LevelEditorSaverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelEditorSaver saver = (LevelEditorSaver)target;
        if (GUILayout.Button("Sauvegarder le niveau"))
        {
            saver.SaveLevel();
        }
    }
}
#endif

