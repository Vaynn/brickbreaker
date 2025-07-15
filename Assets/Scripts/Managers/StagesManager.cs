using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StagesManager : MonoBehaviour
{
    [SerializeField] private GameObject scorePrefab;
    [SerializeField] private Transform grid;
    [SerializeField] private Button back;
    void Start()
    {
        back.onClick.AddListener(() => {
            SceneManager.LoadScene("Menu");
            SoundManager.PlaySound(SoundType.BACK);
            });
        GameContext.Instance.SetMode(GameMode.Retry);
        PopulateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PopulateGrid(){
        var sortedStages = UserManager.Instance.Scores.OrderBy(p => p.levelId).ToList();

        foreach (var data in sortedStages){
            GameObject sc = Instantiate(scorePrefab, grid);
            var card = sc.GetComponent<StagesCards>();
            if(card != null)
                card.Setup(data.levelId, data.score);
        }
    }
}
