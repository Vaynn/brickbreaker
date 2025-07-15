using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private const string PREF_MUSIC = "MusicVolume";
    private const string PREF_SFX = "SFXVolume";
    public Button backButton;

    public Button logoutButton;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backButton.onClick.AddListener(() => {
            SceneManager.LoadScene("Menu");
            SoundManager.PlaySound(SoundType.BACK);
        });
        logoutButton.onClick.AddListener(Logout);
        SoundManager.Instance.gameAudioMixer.GetFloat("BGMVolume", out float currentBGM);
        SoundManager.Instance.gameAudioMixer.GetFloat("SFXVolume", out float currentSFX);

        musicSlider.value = PlayerPrefs.GetFloat(PREF_MUSIC, 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat(PREF_SFX, 1f);

        musicSlider.onValueChanged.AddListener(SoundManager.Instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SoundManager.Instance.SetSFXVolume);
        
    }

    private void Logout(){
        FirebaseAuth.DefaultInstance.SignOut();
        UserManager.Instance.Reinitialize();
        GameContext.Instance.Reinitialize();
        SceneManager.LoadScene("Authentication");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
