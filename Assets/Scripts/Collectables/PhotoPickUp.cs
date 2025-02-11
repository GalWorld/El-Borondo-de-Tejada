using UnityEngine;

public class PhotoPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private PhotoData photoData;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 50f;

    [Header("Bobbling Settings")]
    [SerializeField] private float bobbingAmplitude = 0.5f;
    [SerializeField] private float bobbingFrequency = 1f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }
    void FixedUpdate()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        float newY = startPosition.y + Mathf.Sin(Time.time * bobbingFrequency) * bobbingAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    public void Interact(Transform interactorTransform)
    {
        CollectPhoto();
    }

    public string GetInteractText()
    {
        return "Presiona 'E' para recoger la foto";
    }

    private void CollectPhoto()
    {
        Debug.Log($"Foto recogida: {photoData.title}");
    }
}
