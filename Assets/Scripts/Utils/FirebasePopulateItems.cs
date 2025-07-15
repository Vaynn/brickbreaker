using System.Collections.Generic;
using System.IO;
using Firebase.Extensions;
using UnityEngine;

public class FirebasePopulateItems
{
    public static void PopulateItemsFromJson(){
        string path = Path.Combine(Application.streamingAssetsPath, "items.json");
        string json = File.ReadAllText(path);
        RawItemListWrapper wrapper = JsonUtility.FromJson<RawItemListWrapper>(json);
        List<RawItem> raws = wrapper.items;

        foreach (var raw in raws)
        {
            Debug.Log(raw.id);
            ItemData item = new ItemData(raw);
            Debug.Log(item.id);
            FirebaseManager.Instance.Store.Collection("items").Document(item.id).SetAsync(item)
                .ContinueWithOnMainThread(task =>
    {
        if (task.IsCompletedSuccessfully)
        {
            Debug.Log($"Item '{item.id}' envoyé dans Firestore.");
        }
        else
        {
            Debug.LogError($"Erreur d'envoi Firestore pour '{item.id}' : {task.Exception}");
        }
    });

        }
    }
}
