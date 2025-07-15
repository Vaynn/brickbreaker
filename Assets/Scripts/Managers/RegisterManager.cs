using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RegisterManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField confirmPWDInput;
    public Button registerButton;
    public Button backButton;
    public TMP_Text feedBackText;

    private FirebaseAuth auth;
    private FirebaseFirestore store;

    void Start(){
        auth = FirebaseManager.Instance.Auth;
        store = FirebaseManager.Instance.Store;
        registerButton.onClick.AddListener(RegisterUser);
        backButton.onClick.AddListener(() => SceneManager.LoadScene("Authentication"));
    }

    void RegisterUser(){
        string username = usernameInput.text.Trim();
        string email = emailInput.text.Trim();
        string password = passwordInput.text;
        string confirm = confirmPWDInput.text;
        if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)){
            feedBackText.text = "All fields must be completed!";
            return;
        }
        if(password != confirm){
            feedBackText.text = "Passwords do not match!";
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if(task.IsFaulted || task.IsCanceled){
                feedBackText.text = FirebaseErrorHandler.GetErrorMessage(task.Exception);
            } else {
                SaveUserProfile(task.Result.User, username);
                SceneManager.LoadScene("Menu");
            }
        });
    }
    private void SaveUserProfile(FirebaseUser user, string username){

        string uid = user.UserId;

        UserProfile profile = new UserProfile { DisplayName = username };
        user.UpdateUserProfileAsync(profile);

        UserData userData = new UserData(uid, username);
        var docRef = store.Collection("users").Document(uid);
        docRef.SetAsync(userData)
              .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError($"Firestore write failed: {task.Exception}");
                feedBackText.text = "Registration succeeded but profile save failed.";
            }
            else
            {
                CreateUserInventory(user);
            }
        });
    }

    private void CreateUserInventory(FirebaseUser user){
        string uid = user.UserId;
        List<InventoryItem> itemList = new List<InventoryItem>
        {
            new InventoryItem("Base_Ball", true),
            new InventoryItem("Base_Paddle", true)
        };
        foreach(InventoryItem item in itemList){
            var docRef = store.Collection("users").Document(uid).Collection("inventory").Document(item.ItemId);
            docRef.SetAsync(item)
                .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError($"Firestore write failed: {task.Exception}");
                feedBackText.text = "Inventory Creation Failed";
            }
            else
            {
                SceneManager.LoadScene("Menu");
            }
        });
        }
    }
}
