using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StagesCards : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private Button button;
    private int level;

    public void Setup(string title, int value){
        this.title.text = title;
        score.text = value.ToString();
        level = int.Parse(title.Replace("level ", ""));
        button.onClick.AddListener(OnClick);
    }

    void OnClick(){
        GameContext.Instance.SetLevelToLoad(level);
        GameContext.Instance.SetMode(GameMode.Retry);
        GameContext.Instance.SetLastScoreLevelToLoad(int.Parse(score.text));
        Debug.Log(int.Parse(score.text));
        SceneManager.LoadScene("BrickBreaker");
    }
}
