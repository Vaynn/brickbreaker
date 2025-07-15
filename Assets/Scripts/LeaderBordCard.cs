using TMPro;
using UnityEngine;

public class LeaderBordCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rank;
    [SerializeField] private TextMeshProUGUI username;
    [SerializeField] private TextMeshProUGUI score;

    public void Setup(string rank, string username, int score){
        this.rank.text = rank;
        this.username.text = username;
        this.score.text = score.ToString();
    }
   
}
