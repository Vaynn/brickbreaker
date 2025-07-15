using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InventoryCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Image equippedImage;
    [SerializeField] private Image image;
    [SerializeField] private Button button;

    public void Setup(string description, bool isEquipped, string icon){
        this.description.text =  description;
        if(isEquipped){
            button.interactable = false;
            equippedImage.enabled = isEquipped;
        } else {
            equippedImage.enabled = isEquipped;
        }
        Sprite sprite = Resources.Load<Sprite>("Sprites/DarkTheme/Items/" + icon);
        image.sprite = sprite;
    }
}
