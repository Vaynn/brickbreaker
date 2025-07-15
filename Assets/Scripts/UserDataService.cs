using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Firebase.Firestore;

public class UserDataService
{
    private static FirebaseFirestore db => FirebaseManager.Instance.Store;

    public static void UpdateCoins(string uid, int newValue){
        var userRef = db.Collection("users").Document(uid);
        userRef.UpdateAsync(new Dictionary<String, object>{
            {
                "coins", FieldValue.Increment(newValue)
            }
        });
    }

    public static void UpdateHighestLevel(string uid, int level){
        var userRef = db.Collection("users").Document(uid);
        userRef.UpdateAsync(new Dictionary<string, object> {
            {
                "highestLevel", level
            }
        });
    }

    public static void UpdateTotalScore(string uid, int amount){
        var userRef = db.Collection("users").Document(uid);
        userRef.UpdateAsync(new Dictionary<string, object>{
            {
                "totalScore", FieldValue.Increment(amount)
            }
        });
    }

       public static async Task UpdateLeaderBoardTotalScore(string uid, string username, int amount){
        var userRef = db.Collection("leaderboard").Document(uid);
        try {
            await userRef.SetAsync(new Dictionary<string, object>{
            {
                "username", username
            },
            {
                "totalScore", FieldValue.Increment(amount)
            }
        }, SetOptions.MergeAll
        );
        } catch {
            throw;
        }   
    }

    public static async Task UpdateBestScore(string uid, Score score){
        var scoreRef = db.Collection("users").Document(uid)
            .Collection("scores")
            .Document(score.levelId);
        try {
            await scoreRef.SetAsync(score, SetOptions.MergeAll);
        }catch{
            throw;
        }
        
    }

    public static async Task<List<UserTotalScore>> GetTopTenRank(){
        List<UserTotalScore> top = new List<UserTotalScore>();
        Query topUsers = db.Collection("leaderboard")
                        .OrderByDescending("totalScore")
                        .Limit(10);
        QuerySnapshot snap = await topUsers.GetSnapshotAsync();
        foreach(DocumentSnapshot doc in snap.Documents){
            top.Add(doc.ConvertTo<UserTotalScore>());
        }
        return top;
    }

    public static async Task<int> GetMyRank(){
        int rank = 0;
        var snap = await db.Collection("leaderboard")
        .WhereGreaterThan("totalScore", UserManager.Instance.Profile.totalScore)
        .GetSnapshotAsync();
        rank = snap.Count + 1;
        return rank;
    }

    public static async Task UpdateEquippedBall(string uid, string ballId){
        var userRef = db.Collection("users").Document(uid);
        await userRef.UpdateAsync("equippedBall", ballId);
    }

    public static async Task UpdateEquippedPaddle(string uid, string paddleId){
        var userRef = db.Collection("users").Document(uid);
        await userRef.UpdateAsync("equippedPaddle", paddleId);
    }
}
