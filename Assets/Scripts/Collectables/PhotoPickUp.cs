using UnityEngine;

public class PhotoPickup : MonoBehaviour, IInteractable
{
    [Tooltip("Reference to the PhotoData ScriptableObject containing this photo's information.")]
    [SerializeField] private PhotoData photoData;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 50f;

    [Header("Bobbling Settings")]
    [SerializeField] private float bobbingAmplitude = 0.5f;
    [SerializeField] private float bobbingFrequency = 1f;

    private Vector3 startPosition;
    private PhotoCollectedCanvasController PhotoCollectedCanvasController;

    void Start()
    {
        startPosition = transform.position;
        PhotoCollectedCanvasController = FindAnyObjectByType<PhotoCollectedCanvasController>();

        // Check if the photo has already been collected
        if (GameController.Instance.HasCollectedPhoto(photoData.id))
        {
            gameObject.SetActive(false);
        }
    }
    
    void FixedUpdate()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        float newY = startPosition.y + Mathf.Sin(Time.time * bobbingFrequency) * bobbingAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    public void Interact(Transform interactorTransform)
    {
        if (PhotoCollectedCanvasController != null)
        {
            GameController.Instance.SetGameState(GameState.Interacting);
            PhotoCollectedCanvasController.ShowAnimationAboutPhoto(photoData);
            GameController.Instance.CollectPhoto(photoData);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("PhotoCollectedCanvasController is missing!");
        }
    }

    public string GetInteractText()
    {
        return "Presiona 'E' para recoger la foto";
    }
}
