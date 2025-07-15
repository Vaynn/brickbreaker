using Firebase.Firestore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
public class ShopService
{
   private static FirebaseFirestore db => FirebaseManager.Instance.Store;

   public static async Task<List<ItemData>> GetAvailableShopItems(string userId){
    List<ItemData> allItems = new();
    List<string> ownedItemIds = new();

    //Load all shop's items
    QuerySnapshot itemSnapShot = await db.Collection("items").GetSnapshotAsync();
    foreach (DocumentSnapshot doc in itemSnapShot.Documents){
        allItems.Add(doc.ConvertTo<ItemData>());
    }

    //Load inventory items, owned by user
    QuerySnapshot inventorySnapshot = await db
        .Collection("users")
        .Document(userId)
        .Collection("inventory")
        .GetSnapshotAsync();
    foreach (DocumentSnapshot doc in inventorySnapshot.Documents){
        ownedItemIds.Add(doc.Id);
    }

    //Return items not owned by the user to display in the shop scene
    List<ItemData> availableItems = allItems
        .Where(item => !ownedItemIds.Contains(item.id) || item.id == "Extra_Life")
        .OrderBy(item => item.price)
        .ToList();
    return availableItems;
   }

   public static void AddToInventory(InventoryItem item){
        var inventoryRef = db.Collection("users").Document(UserManager.Instance.Profile.uid).Collection("inventory");
        inventoryRef.Document(item.ItemId).SetAsync(new Dictionary<string, bool>{
            {
                "IsEquipped", false
            }
        });
   }
}
