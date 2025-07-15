using Firebase.Firestore;

[FirestoreData]
public class Score
{
    [FirestoreDocumentId] public string levelId {get; set;}
    [FirestoreProperty] public int score {get; set;}

    public Score(){}
    public Score(string id, int scoreScore){
        levelId = id;
        score = scoreScore;
    }
}
