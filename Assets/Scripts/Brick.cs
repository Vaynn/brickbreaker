
using UnityEngine;

public class Brick : MonoBehaviour
{
    public BrickData data;
    private int currentHits = 0;
    [SerializeField] private GameObject bonusPrefab;
    [SerializeField] private GameObject crack;
    [SerializeField] string cracksResourcePath = "Sprites/DarkTheme/Bricks/Cracks";
    Sprite[] crackSlices;
    SpriteRenderer overlay;
    [SerializeField] private BonusType bonusType;

    private void Awake()
    {
        overlay  = transform.Find("Crack")!.GetComponent<SpriteRenderer>();
        crackSlices = Resources.LoadAll<Sprite>(cracksResourcePath);
        if(crackSlices == null || crackSlices.Length == 0)
            Debug.LogError("No image found for cracks");
    }

    public void onHit(){
        GameManager.Instance.AddPoints(data.points);
        GameManager.Instance.AddCoins(data.coins);
        currentHits++;
        if (data.hitMax > currentHits){
            overlay.sprite = crackSlices[data.hitMax - currentHits];
        }
        if(currentHits >= data.hitMax){
            if (bonusPrefab != null){
                if(GameManager.Instance.CanSpawnBonus(bonusType)){
                    GameObject bonus = Instantiate(bonusPrefab, transform.position, Quaternion.identity);
                    BonusItem bonusItem = bonus.GetComponent<BonusItem>();
                    if(bonusItem != null){
                        bonusItem.bonusType = bonusType;
                    }
                }
                
            }
            GameManager.Instance.BrickDestroyed();
            SoundManager.PlaySound(SoundType.BRICK_DESTROY);
            Destroy(gameObject);
        }else{
            SoundManager.PlaySound(SoundType.BRICK_HIT);
        }
    }
}
