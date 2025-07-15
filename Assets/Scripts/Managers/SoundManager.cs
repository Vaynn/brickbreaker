using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public enum SoundType{
    BONUS,
    BRICK_DESTROY,
    BRICK_HIT,
    MENU_MUSIC,
    SELECT,
    BACK,
    PURCHASE,
    CANCEL,
    LOOSE_LIFE,
    WIN,
    PADDLE,
    WALL
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [Header("SFX Clips")]
    [SerializeField] private AudioClip[] soundList;

    [Header("BGM Clips")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip stageMusic;
    [SerializeField] private AudioClip shopMusic;
    [SerializeField] private AudioClip inventoryMusic;
    [SerializeField] private AudioClip stageMenuMusic;

    public static SoundManager Instance {get; private set;}
    private AudioSource sfxSource;
    private AudioSource musicSource;

    public AudioMixer gameAudioMixer;

    private const string PREF_MUSIC = "MusicVolume";
    private const string PREF_SFX = "SFXVolume";
    private void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        var sources = GetComponents<AudioSource>();
        if(sources.Length < 2){
            sfxSource = sources[0];
            musicSource = gameObject.AddComponent<AudioSource>();
        } else {
            sfxSource = sources[0];
            musicSource = sources[1];
        }
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.spatialBlend = 0f;

        musicSource.outputAudioMixerGroup = gameAudioMixer.FindMatchingGroups("BGM")[0];
        sfxSource.outputAudioMixerGroup = gameAudioMixer.FindMatchingGroups("SFX")[0];

        float m = PlayerPrefs.GetFloat(PREF_MUSIC, 0.5f);
        float s = PlayerPrefs.GetFloat(PREF_SFX, 1f);
        ApplyVolume("BGMVolume", m);
        ApplyVolume("SFXVolume", s);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        switch(scene.name){
            case "Menu":
                PlayMusic(inventoryMusic, 0.5f);
                break;
            case "Authentication":
                PlayMusic(inventoryMusic, 0.5f);
                break;
            case "Register":
                PlayMusic(inventoryMusic, 0.5f);
                break;
            case "BrickBreaker":
                PlayMusic(stageMusic, 0.4f);
                break;
            case "Shop":
                PlayMusic(shopMusic, 0.5f);
                break;
            case "Stages":
                PlayMusic(stageMenuMusic, 0.5f);
                break;
            case "LeaderBoard":
                PlayMusic(stageMenuMusic, 0.5f);
                break;
            case "Options":
                PlayMusic(stageMenuMusic, 0.5f);
                break;
            default:
                musicSource.Stop();
                musicSource.clip = null;
                break;
        }
    }

    /// <summary>Joue un SFX</summary>
    public static void PlaySound(SoundType sound, float volume = 1){
        if(Instance == null || Instance.sfxSource == null) return;
        Instance.sfxSource.PlayOneShot(Instance.soundList[(int)sound], volume);
    }

    /// <summary>Change de musique</summary>
    public void PlayMusic(AudioClip clip, float volume = 0.5f){
        if (musicSource.clip == clip) return; // déjà en cours
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.Play();
    }

    /// <summary>
    /// Appelé par un UI Slider [0→1], converti en dB pour la BGM
    /// </summary>
    public void SetMusicVolume(float sliderValue){
        float v = Mathf.Clamp(sliderValue, 0.0001f, 1f);
        float dB = 20f * Mathf.Log10(v);
        gameAudioMixer.SetFloat("BGMVolume", dB);
        PlayerPrefs.SetFloat(PREF_MUSIC, v);
    }

      /// <summary>
    /// Appelé par un UI Slider [0→1], converti en dB pour les SFX
    /// </summary>
    public void SetSFXVolume(float sliderValue)
    {
        float v = Mathf.Clamp(sliderValue, 0.0001f, 1f);
        float dB = 20f * Mathf.Log10(v);
        gameAudioMixer.SetFloat("SFXVolume", dB);
        PlayerPrefs.SetFloat(PREF_SFX, v);
    }

    private void ApplyVolume(string exposedParam, float sliderValue){
        float v = Mathf.Clamp(sliderValue, 0.001f, 1f);
        float dB = 20f * Mathf.Log10(v);
        gameAudioMixer.SetFloat(exposedParam, dB);
    }
     
}
