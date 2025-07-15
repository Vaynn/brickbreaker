using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public string dirName = "LevelsJson";
   public string fileName = "level1.json";
   public Transform parentContainer;

   [System.Serializable]
   public class BrickData{
        public Vector2 position;
        public string prefabName;
    }

    [System.Serializable]
    public class LevelData{
        public List<BrickData> bricks;
    }

    void Start(){
        if (GameContext.Instance.CurrentMode != GameMode.Generator)
            fileName = "level" + GameContext.Instance.LevelToLoad + ".json";
        else
        {
            string userDir = Path.Combine(Application.streamingAssetsPath, "Generator", UserManager.Instance.Profile.uid);
            string fileN = $"level{GameContext.Instance.LevelToLoad}.json";
            fileName = Path.Combine(userDir, fileN);

            
        }
        StartCoroutine(LoadLevelCoroutine(fileName));
    }

    public IEnumerator LoadLevelCoroutine(string fileName){
        string path = Path.Combine(Application.streamingAssetsPath, dirName, fileName);
        if(!File.Exists(path)){
            Debug.LogError("Fichier de niveau non trouvé: " + path);
            yield break;
        }
        string json = File.ReadAllText(path);
        LevelData level = JsonUtility.FromJson<LevelData>(json);

        foreach (BrickData data in level.bricks)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Bricks/" + data.prefabName);
            if (prefab != null){
                GameObject brick = Instantiate(prefab, data.position, Quaternion.identity, parentContainer);
                GameManager.Instance.RegisterBricks();
            } else {
                Debug.LogWarning("Prefab introuvable : " + data.prefabName);
            }
        }
    }
}
