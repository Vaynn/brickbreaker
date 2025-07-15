using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI feedback;
    public Button login;
    public Button register;
    private FirebaseAuth auth;
    private FirebaseFirestore store;

    private void Awake(){
        auth = FirebaseManager.Instance.Auth;
        store = FirebaseManager.Instance.Store;
        if(auth.CurrentUser != null){
            SceneManager.LoadScene("Menu");
        }
    }
    void Start(){
        register.onClick.AddListener(GoToRegistration);
        login.onClick.AddListener(OnLogin);  
    }

    void OnLogin(){
        feedback.text = "";
        string email = emailInput.text.Trim();
        string pwd = passwordInput.text;
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pwd)){
            feedback.text = "Email and Password required.";
            return;
        }
        auth.SignInWithEmailAndPasswordAsync(email,pwd).ContinueWithOnMainThread(task => {
            if(task.IsFaulted){
                feedback.text = FirebaseErrorHandler.GetErrorMessage(task.Exception);
            }else{
                SceneManager.LoadScene("Menu");
            }
        });
    }

    void GoToRegistration(){
        SceneManager.LoadScene("Register");
    }
}
