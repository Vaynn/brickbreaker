using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private PlayerControls playerControls;
    private bool isDragging = false;
    private Camera mainCamera;
    private float startY;

    private void Awake(){
        Debug.Log("Awake");
        playerControls = new PlayerControls();
        mainCamera = Camera.main;
    }

    private void OnEnable(){
        Debug.Log("Enable");
        playerControls.Move.Enable();
        playerControls.Move.TouchPress.performed += StartDrag;
        playerControls.Move.TouchPress.canceled += EndDrag;
        playerControls.Move.TouchPosition.performed += Drag;
        startY = transform.position.y;
    }

    private void OnDisable(){
        Debug.Log("Disable");
        playerControls.Move.TouchPress.performed -= StartDrag;
        playerControls.Move.TouchPress.canceled -= EndDrag;
        playerControls.Move.TouchPosition.performed -= Drag;
        playerControls.Move.Disable();
    }

    private void StartDrag(InputAction.CallbackContext context){
        Debug.Log("StartDrag");
        Vector2 touchPos = playerControls.Move.TouchPosition.ReadValue<Vector2>();
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y));
        worldPos.z = 0;
        Collider2D hit = Physics2D.OverlapPoint(worldPos);
        if(hit != null && hit.gameObject == gameObject)
        {
            isDragging = true;
        }
    }

    private void Drag(InputAction.CallbackContext context){
        Debug.Log("Drag");
        if (!isDragging) return;

        Vector2 touchPos = playerControls.Move.TouchPosition.ReadValue<Vector2>();
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y));

        float screenLeft = mainCamera.ViewportToWorldPoint(Vector3.zero).x;
        float screenRight = mainCamera.ViewportToWorldPoint(Vector3.one).x;
        worldPos.x = Mathf.Clamp(worldPos.x, screenLeft, screenRight);
        worldPos.z = 0;
        worldPos.y = startY;
        transform.position = worldPos;
    }

    private void EndDrag(InputAction.CallbackContext context){
        Debug.Log("EndDrag");
        isDragging = false;
    }
}
