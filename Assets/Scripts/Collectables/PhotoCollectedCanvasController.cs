using UnityEngine;
using UnityEngine.UI;

public class PhotoCollectedCanvasController : MonoBehaviour
{
    [Header("Canvas Settings")]
    [SerializeField] private Text titleText;
    [SerializeField] private Text descriptionText;

    [Header("Game Object Settings")]
    [Tooltip("Animator that controls the animation of the photo when collected.")]
    [SerializeField] private Animator photoAnimator;

    [Tooltip("UI Canvas that appears when the photo is collected.")]
    [SerializeField] private GameObject photoCanvasCollected;

    public void ShowAnimationAboutPhoto(PhotoData photoData)
    {
        if (photoData == null) return;

        photoCanvasCollected.SetActive(true); 

        titleText.text = photoData.title;
        descriptionText.text = photoData.description;

        if (photoData.animation != null)
        {
            PlayAnimation(photoData.animation);
        }

        Cursor.lockState = CursorLockMode.None;
    }

    private void PlayAnimation(AnimationClip clip)
    {
        if (photoAnimator == null)
        {
            Debug.LogError("⚠️ No Animator assigned in PhotoCollectedCanvasController!");
            return;
        }

        // Create an AnimatorOverrideController to instantiate the AnimationClip
        AnimatorOverrideController overrideController = new AnimatorOverrideController(photoAnimator.runtimeAnimatorController);
        overrideController["DefaultAnimation"] = clip; 
        photoAnimator.runtimeAnimatorController = overrideController;

        photoAnimator.Play(clip.name);
    }

    //This metod is executed when the exit button is pressed
    public void ExitButton()
    {
        photoCanvasCollected.SetActive(false); 
        Cursor.lockState = CursorLockMode.Locked;
        GameController.Instance.SetGameState(GameState.Playing); 
    }
}
