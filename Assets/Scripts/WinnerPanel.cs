using System.Threading.Tasks;
using UnityEngine;


public class WinnerPanel : MonoBehaviour
{
     public void GoToNextLevel(){
        GameManager.Instance.LoadNextLevel(true);
        GameManager.Instance.HideWinnerPanel();
        Time.timeScale = 1f;
        SoundManager.PlaySound(SoundType.SELECT);
    }

    public void backToHome(){
        GameManager.Instance.LoadNextLevel(false);
        Time.timeScale = 1f;
        SoundManager.PlaySound(SoundType.BACK);
    }
}
