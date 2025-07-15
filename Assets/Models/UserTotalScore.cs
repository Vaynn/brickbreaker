using Firebase.Firestore;

[FirestoreData]
public class UserTotalScore
{
    [FirestoreDocumentId] public string uid {get; set;}
    [FirestoreProperty]
    public string username {get; set;}
    [FirestoreProperty]
    public int totalScore{ get; set;}

    public UserTotalScore(){}
    public UserTotalScore(string uid, string username, int totalScore){
        this.uid = uid;
        this.username = username;
        this.totalScore = totalScore;
    }
}
