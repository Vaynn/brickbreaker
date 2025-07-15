using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private GameObject purpleRankPrefab;
    [SerializeField] private GameObject blackRankPrefab;
    [SerializeField] private GameObject redRanckPrefab;
    [SerializeField] private Transform grid;
    [SerializeField] private Button back;
    void Start()
    {
        back.onClick.AddListener(() => {
        SceneManager.LoadScene("Menu");
        SoundManager.PlaySound(SoundType.BACK);
        });
        PopulateGrid();
    }

    void Update()
    {
        
    }

    public async void PopulateGrid(){
        var topList = await UserDataService.GetTopTenRank();
        var myRank = await UserDataService.GetMyRank();
        bool amIInTopTen = false;
        int i = 1;
        Debug.Log(topList.Count);
        GameObject sc;
        foreach (var rank in topList){
            if (rank.username == UserManager.Instance.Profile.username){
                Debug.Log("i am in top ten . " + i);
                sc = Instantiate(redRanckPrefab, grid);
                amIInTopTen = true;
            }    
            else if(i % 2 == 0)
                sc = Instantiate(blackRankPrefab, grid);
            else
                sc = Instantiate(purpleRankPrefab, grid);
            var card = sc.GetComponent<LeaderBordCard>();
            if(card != null)
                card.Setup("#" + i, rank.username, rank.totalScore);
            i++;
        }
        if (!amIInTopTen){
            Debug.Log("i am not in top ten " + myRank);
            sc = Instantiate(redRanckPrefab, grid);
            var card = sc.GetComponent<LeaderBordCard>();
            if(card != null)
                card.Setup("#" + myRank, UserManager.Instance.Profile.username, UserManager.Instance.Profile.totalScore);
        }
    }
}
