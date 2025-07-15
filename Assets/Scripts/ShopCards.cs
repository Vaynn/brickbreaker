using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class ShopCards : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private Image image;
    [SerializeField] private Image coins;
    [SerializeField] private Button button;

    public void SetupActive(string description, int price, string icon){
        this.description.text =  description;
        this.price.text = price.ToString();
        Sprite sprite = Resources.Load<Sprite>("Sprites/DarkTheme/Items/" + icon);
        image.sprite = sprite;
    }
    public void SetupInactive(string description, int price, string icon){
        this.description.text =  description;
        this.price.text = price.ToString();
        Sprite sprite = Resources.Load<Sprite>("Sprites/DarkTheme/Items/" + icon);
        image.sprite = sprite;
        TransparancizeText(this.description);
        TransparancizeText(this.price);
        this.price.color = Color.red;
        TransparancizeImage(image);
        TransparancizeImage(coins);
        button.interactable = false;
    }

    public void TransparancizeText(TextMeshProUGUI text){
        Color currentColor = text.color;
        text.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0.5f);
    }

    public void TransparancizeImage(Image image){
        Color currentColor = image.color;
        image.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0.5f);
    }


}
