using Firebase.Firestore;


[FirestoreData]
public class UserData
{
    [FirestoreDocumentId] public string uid {get; set;}
    [FirestoreProperty] public string username {get; set;}
    [FirestoreProperty] public int coins {get; set;}
    [FirestoreProperty] public int maxLife {get; set;}

    [FirestoreProperty] public int highestLevel {get; set;}
    [FirestoreProperty] public int totalScore {get; set;}
    [FirestoreProperty] public string equippedBall {get; set;}
    [FirestoreProperty] public string equippedPaddle {get; set;}
    public UserData(){}
    public UserData(string id, string name){
        uid = id;
        username = name;
        coins = 0;
        maxLife = 3;
        highestLevel = 1;
        totalScore = 0;
        equippedBall = "Base_Ball";
        equippedPaddle = "Base_Paddle";
    }
    
    
}
