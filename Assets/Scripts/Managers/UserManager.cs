using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class UserManager : MonoBehaviour
{
   public static UserManager Instance {get; private set;}

   public UserData Profile {get; private set;}
   public List<Score> Scores {get; private set;}

   void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
   }

   public void Initialize(UserData profile, List<Score> scores){
    Profile = profile;
    Scores = scores;

   }

   public void AddCoins(int newValue){
     Profile.coins += newValue;
     UserDataService.UpdateCoins(Profile.uid, newValue);
   }

    public void SubCoins(int newValue){
     Profile.coins -= newValue;
     UserDataService.UpdateCoins(Profile.uid, -newValue);
   }
   

   public void UpdateHighestLevel(int level){
        if(level > Profile.highestLevel){
            Profile.highestLevel = level;
            UserDataService.UpdateHighestLevel(Profile.uid, level);
        }
   }

   public async Task UpdateTotalScore(int amount){
    Profile.totalScore += amount;
    UserDataService.UpdateTotalScore(Profile.uid, amount);
    await UserDataService.UpdateLeaderBoardTotalScore(Profile.uid, Profile.username, amount);
   }

   public async Task SubmitBestScore(string levelId, int scoreValue){
        var bestScore = new Score(levelId, scoreValue);
        var existing = Scores.Find(b => b.levelId == levelId);
        if (existing == null){
            Scores.Add(bestScore);
            await UserDataService.UpdateBestScore(Profile.uid, bestScore);
        }
            
        else if (scoreValue > existing.score){
            existing.score = scoreValue;
            await UserDataService.UpdateBestScore(Profile.uid, bestScore);
        }
        
   }

   public int GetScoreForLevel(string levelId){
    var bs = Scores.FirstOrDefault(b => b.levelId == levelId);
    return bs != null ? bs.score : 0;
   }

   public async Task UpdateEquippedBall(string ballId){
    Profile.equippedBall = ballId;
    await UserDataService.UpdateEquippedBall(Profile.uid, ballId);
   }

    public async Task UpdateEquippedPaddle(string paddleId){
    Profile.equippedPaddle = paddleId;
    await UserDataService.UpdateEquippedPaddle(Profile.uid, paddleId);
   }

   public void Reinitialize(){
        Profile = null;
        Scores = null;
        Destroy(gameObject);
   }
}
