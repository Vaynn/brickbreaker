using Firebase;
using UnityEngine;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;

public class FirebaseManager : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Authentication";

    public static FirebaseManager Instance { get; private set; }
    public FirebaseAuth Auth { get; private set; }
    public FirebaseFirestore Store { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitFirebase();
    }

    void InitFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == DependencyStatus.Available)
            {
                var app = FirebaseApp.DefaultInstance;
                
                Auth   = FirebaseAuth.DefaultInstance;
                Store = FirebaseFirestore.DefaultInstance;
                Debug.Log("Firebase initialized");
                UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogError($"Firebase init error: {task.Result}");
            }
        });
    }
}


