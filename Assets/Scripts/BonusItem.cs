using UnityEngine;

public class BonusItem : MonoBehaviour
{
    public BonusType bonusType;
    public float fallSpeed = 2f;

    void Update(){
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Paddle")){
            GameManager.Instance.ApplyBonus(bonusType);
            SoundManager.PlaySound(SoundType.BONUS);
            Destroy(gameObject);
        };
    }
}
