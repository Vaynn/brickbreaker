using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaFitter : MonoBehaviour
{
    void Awake()
    {
        ApplySafeArea();
    }

    #if UNITY_EDITOR
    void Update()
    {
        // Permet de voir le résultat en temps réel dans l'Editor si on redimensionne la GameView
        ApplySafeArea();
    }
    #endif

    void ApplySafeArea()
    {
        RectTransform rt = GetComponent<RectTransform>();
        Rect safe = Screen.safeArea;

        // Calcul des ancres en pourcentage
        Vector2 anchorMin = safe.position;
        Vector2 anchorMax = safe.position + safe.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
    }
}
