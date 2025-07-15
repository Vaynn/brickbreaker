using UnityEngine;

public class LifeBar : MonoBehaviour
{
   [SerializeField] RectTransform fillRect;
   public int maxHits = 3;
   private int currentHits;

   void Awake(){
    currentHits = maxHits;
    UpdateBar();
   }

   public void TakeHit(){
    currentHits = Mathf.Max(0, currentHits - 1);
   }

   void UpdateBar(){
    float percent = (float)currentHits/maxHits;
    float fullWidth = fillRect.parent.GetComponent<RectTransform>().rect.width;
    fillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fullWidth * percent);
   }
}
