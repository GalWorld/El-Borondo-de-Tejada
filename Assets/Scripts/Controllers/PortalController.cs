using UnityEngine;

public class PortalController : MonoBehaviour
{
    [Tooltip("The name of the scene to load")]
    [SerializeField] private string nameloadScene;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("I'm into the trigger");
            // Execute Animation

            // Load New Scene
            SceneController.LoadNewScene(nameloadScene);
        }    
    }
}
