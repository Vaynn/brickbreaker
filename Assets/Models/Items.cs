using Firebase.Firestore;
using System.Collections.Generic;

[System.Serializable]
public class RawItem
{
    public string id;
    public string name;
    public string type;
    public int price;
    public string icon;
}

[FirestoreData]
public class ItemData
{
    [FirestoreDocumentId] public string id {get; set;}
    [FirestoreProperty] public string name {get; set;}
    [FirestoreProperty] public string type {get; set;}
    [FirestoreProperty] public int price {get; set;}
    [FirestoreProperty] public string icon {get; set;}
   

    public ItemData(){}
    public ItemData(RawItem raw){
        this.id = raw.id;
        this.name = raw.name;
        this.type = raw.type;
        this.price = raw.price;
        icon = raw.icon;
    }
    
}

[System.Serializable]
public class RawItemListWrapper
{
    public List<RawItem> items;
}
