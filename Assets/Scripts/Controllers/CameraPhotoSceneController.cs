using UnityEngine;

public class CameraPhotoSceneController : MonoBehaviour
{
    [SerializeField] private float sensitivity = 3f;
    [SerializeField] private float smoothTime = 0.05f;
    private Vector2 currentMouseDelta;
    public bool allowMouseLook = false;

    void Start()
    {
        allowMouseLook = true; 
        LockCursor(); 
    }

    void Update()
    {
        HandleMouseToggle();
        
        if (allowMouseLook)
        {
            RotateCamera();
        }
    }

    private void HandleMouseToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            allowMouseLook = !allowMouseLook;
            
            if (allowMouseLook)
            {
                LockCursor();
            }
            else
            {
                UnlockCursor();
            }
        }
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity;

        // Smooth movement
        currentMouseDelta.x = Mathf.Lerp(currentMouseDelta.x, mouseX, smoothTime);
        currentMouseDelta.y = Mathf.Lerp(currentMouseDelta.y, mouseY, smoothTime);

        // Apply separate rotation for horizontal and vertical movement
        transform.Rotate(Vector3.up, currentMouseDelta.x, Space.World);
        transform.Rotate(Vector3.right, -currentMouseDelta.y, Space.Self);

        // Lock the Z-axis rotation to prevent camera tilting
        Vector3 eulerAngles = transform.eulerAngles;
        eulerAngles.z = 0f;
        transform.eulerAngles = eulerAngles;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
