using UnityEngine;

public class PhotoWall : MonoBehaviour, IInteractable
{
    [SerializeField] private PhotoData photoData;

    public void Interact(Transform interactorTransform)
    {
        // DisplayPhoto();
    }

    public string GetInteractText()
    {
        return "Presiona 'E' para ver las fotos de tu Borondo";
    }

    // private void DisplayPhoto()
    // {
    //     Debug.Log($"Mostrando la foto: {photoData.title}");
    // }
}
