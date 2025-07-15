using Firebase.Firestore;

[FirestoreData]
public class InventoryItem
{
    [FirestoreDocumentId] public string ItemId {get; set;}
    [FirestoreProperty] public bool IsEquipped {get; set;}

    public InventoryItem(){}
    public InventoryItem(string itemId, bool IsEquipped = false){
        this.ItemId = itemId;
        this.IsEquipped = IsEquipped;
    }
}
