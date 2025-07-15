using UnityEngine;
using UnityEngine.Rendering;

public class GameContext : MonoBehaviour
{
    public static GameContext Instance {get; private set;}
    public GameMode CurrentMode {get; private set;} = GameMode.Adventure;
    public int LevelToLoad {get; private set;}
    public int LastScoreLevelToLoad {get; private set;}
  

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMode(GameMode mode) => CurrentMode = mode;
    public void SetLevelToLoad(int level) => LevelToLoad = level;

    public void SetLastScoreLevelToLoad(int score) => LastScoreLevelToLoad = score;

    public void Reinitialize(){
        SetMode(GameMode.Adventure);
        SetLevelToLoad(0);
        SetLastScoreLevelToLoad(0);
    }
  

}
