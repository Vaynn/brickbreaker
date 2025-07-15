using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;

public class ItemViewModel{
    public ItemData Data {get; set;}
    public bool IsEquipped {get; set;}
}
public class InventoryService : MonoBehaviour
{
    private static FirebaseFirestore db => FirebaseManager.Instance.Store;

   public static async Task<List<ItemViewModel>> GetAvailableShopItems(string userId){
    List<ItemData> allItems = new();
    Dictionary<string, bool> ownedItemIds = new();

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
        bool isEquippe = doc.ContainsField("IsEquipped") ? doc.GetValue<bool>("IsEquipped") : false;
        ownedItemIds[doc.Id] = isEquippe;
    }

    //Return items not owned by the user to display in the shop scene
    List<ItemViewModel> availableItems = allItems
        .Select(item => new ItemViewModel{
            Data = item,
            IsEquipped = ownedItemIds.TryGetValue(item.id, out bool eq) && eq
        })
        .Where(vm => ownedItemIds.ContainsKey(vm.Data.id) && vm.Data.id != "Extra_Life")
        .OrderBy(vm => vm.Data.type)
        .ToList();
    return availableItems;
   }

   public static void SetEquipItem(string itemId, bool isEquipped){
        var inventoryRef = db.Collection("users").Document(UserManager.Instance.Profile.uid).Collection("inventory");
        inventoryRef.Document(itemId).SetAsync(new Dictionary<string, bool>{
            {
                "IsEquipped", isEquipped
            }
        });
   }
}
