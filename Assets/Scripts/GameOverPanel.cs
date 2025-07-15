using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverPanel : MonoBehaviour
{

    
    public void retry(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.Instance.HideGameOverPanel();
        SoundManager.PlaySound(SoundType.SELECT);
        Time.timeScale = 1f;
    }

    public void backToHome(){
       SceneManager.LoadScene("Menu");
       SoundManager.PlaySound(SoundType.BACK);
       Time.timeScale = 1f;
    }


}
