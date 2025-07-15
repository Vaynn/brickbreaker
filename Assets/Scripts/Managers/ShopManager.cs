using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform grid;
    [SerializeField] private Button back;
    [SerializeField] private TextMeshProUGUI purse;
    [SerializeField] private GameObject PurchasePanel;
    [SerializeField] private Button BuyPurchasePanelButton;
    [SerializeField] private GameObject Overlay;
    private List<GameObject> itemsObject = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        string uid = UserManager.Instance.Profile.uid;
        List<ItemData> shopItems = await ShopService.GetAvailableShopItems(uid);
        CreateCards(shopItems);
        purse.text = UserManager.Instance.Profile.coins.ToString();
    }

    public void CreateCards(List<ItemData> shopItems){
        foreach (var item in shopItems){
            GameObject sc = Instantiate(itemPrefab, grid);
            var card = sc.GetComponent<ShopCards>();
            if(card != null){
                if(item.price <= UserManager.Instance.Profile.coins){
                    card.SetupActive(item.name, item.price, item.icon);
                    Button but = card.GetComponentInChildren<Button>();
                    if (but != null){
                        but.onClick.AddListener(() => ShowPurchasePanel(item, shopItems));
                    }
                    else
                        Debug.Log("No button");
                }
                else {
                    card.SetupInactive(item.name, item.price, item.icon);
                    Button but = card.GetComponentInChildren<Button>();
                    if (but != null)
                        but.onClick.AddListener(() => ShowPurchasePanel(item, shopItems));
                    else
                        Debug.Log("No button");
                }
                itemsObject.Add(sc);
            }
                
        }
    }
    public void ShowPurchasePanel(ItemData item, List<ItemData> itemDatas){
        PurchasePanel.SetActive(true);
        BuyPurchasePanelButton.onClick.AddListener(() => PurchaseItem(item,itemDatas));
        Overlay.SetActive(true);
        SoundManager.PlaySound(SoundType.SELECT);
    
    }

    public void HidePurchasePanel(){
        PurchasePanel.SetActive(false);
        Overlay.SetActive(false);
        SoundManager.PlaySound(SoundType.BACK);
    }

    public void backToHome(){
        SceneManager.LoadScene("Menu");
        SoundManager.PlaySound(SoundType.BACK);
    }

    public void PurchaseItem(ItemData item, List<ItemData> itemDatas){
        UserManager.Instance.SubCoins(item.price);
        purse.text = UserManager.Instance.Profile.coins.ToString();
        if (item.id != "Extra_Life")
            itemDatas.RemoveAll(it => it.name == item.name);
        foreach(var card in itemsObject){
            Destroy(card);    
        }
        CreateCards(itemDatas);
        InventoryItem invent = new InventoryItem(item.id, false);
        ShopService.AddToInventory(invent);
        SoundManager.PlaySound(SoundType.PURCHASE);
        HidePurchasePanel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
