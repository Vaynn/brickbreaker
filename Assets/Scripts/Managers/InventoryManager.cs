using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
   [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform grid;
    [SerializeField] private Button back;
    [SerializeField] private GameObject EquippedPanel;
    [SerializeField] private Button ValidEquippedPanelButton;
    [SerializeField] private GameObject Overlay;
    private List<GameObject> itemsObject = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        string uid = UserManager.Instance.Profile.uid;
        List<ItemViewModel> shopItems = await InventoryService.GetAvailableShopItems(uid);
        CreateCards(shopItems);
    }

    public void CreateCards(List<ItemViewModel> shopItems){
        foreach (var item in shopItems){
            GameObject sc = Instantiate(itemPrefab, grid);
            var card = sc.GetComponent<InventoryCard>();
            if(card != null){
                card.Setup(item.Data.name, item.IsEquipped, item.Data.icon);
                Button but = card.GetComponentInChildren<Button>();
                if (but != null && !item.IsEquipped){
                    but.onClick.AddListener(() => ShowPurchasePanel(item, shopItems));
                }
                else
                    Debug.Log("No button");
            itemsObject.Add(sc);
            }
                
        }
    }
    public void ShowPurchasePanel(ItemViewModel item, List<ItemViewModel> itemDatas){
        EquippedPanel.SetActive(true);
        ValidEquippedPanelButton.onClick.AddListener(async () => await EquipItem(item, itemDatas));
        Overlay.SetActive(true);
        SoundManager.PlaySound(SoundType.SELECT);
    
    }

    public void HidePurchasePanel(){
        EquippedPanel.SetActive(false);
        Overlay.SetActive(false);
        SoundManager.PlaySound(SoundType.BACK);
    }

    public void backToHome(){
        SceneManager.LoadScene("Menu");
        SoundManager.PlaySound(SoundType.BACK);
    }

    public async Task EquipItem(ItemViewModel item, List<ItemViewModel> itemList){
        string unequipped;
       if(item.Data.type == "ball"){
        unequipped = UserManager.Instance.Profile.equippedBall;
        try{
            await UserManager.Instance.UpdateEquippedBall(item.Data.id);
        } catch{
            Debug.LogError("Error Occured with update of equipped ball.");
        }
       } else {
        unequipped = UserManager.Instance.Profile.equippedPaddle;
        await UserManager.Instance.UpdateEquippedPaddle(item.Data.id);
       }
        InventoryService.SetEquipItem(unequipped, false);
        InventoryService.SetEquipItem(item.Data.id,true);
        foreach(var vm in itemList){
            if (vm.Data.id == item.Data.id)
                vm.IsEquipped = true;
            else if (item.Data.type == "ball" && vm.Data.id == UserManager.Instance.Profile.equippedPaddle)
                vm.IsEquipped = true;
            else if (item.Data.type == "paddle" && vm.Data.id == UserManager.Instance.Profile.equippedBall)
                vm.IsEquipped = true;
            else
                vm.IsEquipped = false;       
        }
        SoundManager.PlaySound(SoundType.PURCHASE);
        foreach(var card in itemsObject){
            Destroy(card);    
        }
        CreateCards(itemList);
        HidePurchasePanel();
    }
}
