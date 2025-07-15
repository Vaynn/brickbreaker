using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    public void resume(){
        Time.timeScale = 1f;
        GameManager.Instance.HidePausePanel();
        SoundManager.PlaySound(SoundType.SELECT);
    }

    public void backToHome(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
        SoundManager.PlaySound(SoundType.BACK);
    }


}
