using TMPro;
using UnityEngine;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using System.Collections.Generic;



public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI currentStage;
    public TextMeshProUGUI score;
    public TextMeshProUGUI coins;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadPlayerProfile();
        
    }

    private void LoadPlayerProfile(){
        Debug.Log("MenuManager");
        string uid = FirebaseManager.Instance.Auth.CurrentUser.UserId;
        var userDoc = FirebaseManager.Instance.Store.Collection("users").Document(uid);
        userDoc.GetSnapshotAsync().ContinueWithOnMainThread(task => {
            if(task.IsCompleted && task.Result.Exists){
                UserData profile = task.Result.ConvertTo<UserData>();
                userDoc.Collection("scores").GetSnapshotAsync().ContinueWithOnMainThread(scoreTask => {
                    if(scoreTask.IsFaulted){
                        FirebaseErrorHandler.GetErrorMessage(scoreTask.Exception);
                        return;
                    }
                    var bestScores = new List<Score>();
                    foreach (var doc in scoreTask.Result.Documents){
                        var bs = doc.ConvertTo<Score>();
                        bs.levelId = doc.Id;
                        bestScores.Add(bs);
                    }
                    UserManager.Instance.Initialize(profile, bestScores);
                    playerName.text = UserManager.Instance.Profile.username;
                    currentStage.text = UserManager.Instance.Profile.highestLevel.ToString();
                    score.text = UserManager.Instance.Profile.totalScore.ToString();
                    coins.text = UserManager.Instance.Profile.coins.ToString();
                }
                );
                
            } else {
                Debug.Log(FirebaseErrorHandler.GetErrorMessage(task.Exception));
                FirebaseManager.Instance.Auth.SignOut();
                SceneManager.LoadScene("Authentication");
                return;
            }
         });
    }

    public void Play(){
        GameContext.Instance.SetMode(GameMode.Adventure);
        GameContext.Instance.SetLevelToLoad(UserManager.Instance.Profile.highestLevel);
        SoundManager.PlaySound(SoundType.SELECT);
        SceneManager.LoadScene("BrickBreaker");
    }

    public void Shop(){
        SceneManager.LoadScene("Shop");
        SoundManager.PlaySound(SoundType.SELECT);
    }

    public void Inventory(){
        SceneManager.LoadScene("Inventory");
        SoundManager.PlaySound(SoundType.SELECT);
    }

    public void StagesList(){
        SceneManager.LoadScene("Stages");
        SoundManager.PlaySound(SoundType.SELECT);
        //todo;
    }

    public void Ranking(){
        SceneManager.LoadScene("LeaderBoard");
        SoundManager.PlaySound(SoundType.SELECT);
;        //todo
    }

    public void Params(){
        SceneManager.LoadScene("Options");
        SoundManager.PlaySound(SoundType.SELECT);
    }

    public void IAGenerator(){
        GameContext.Instance.SetMode(GameMode.Generator);
        SceneManager.LoadScene("IAStagesGenerator");
        SoundManager.PlaySound(SoundType.SELECT);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
