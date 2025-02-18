using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PhotoBudgetCanvasController : MonoBehaviour
{
    [Tooltip("Parent object containing all Post-It buttons.")]
    [SerializeField] private Transform postItsParent;

    [Tooltip("Parent object containing all PhotosInformation panels.")]
    [SerializeField] private Transform photosInformationParent;

    [Tooltip("Reference to the locked UI element (should be a single instance).")]
    [SerializeField] private GameObject lockedUI;

    [Tooltip("Main UI container for the budget view.")]
    [SerializeField] private GameObject budgetUI;

    private Dictionary<Button, Transform> buttonToPhotoInfo = new Dictionary<Button, Transform>();

    private void Awake()
    {
        InitializeButtonDictionary();
    }

    void Start()
    {
        // Automatically display the first photo panel on start
        if (photosInformationParent.childCount > 0)
        {
            UpdatePhotoDisplay(photosInformationParent.GetChild(0));
        }
    }

    public void ActivateViewOfBudget()
    {
        budgetUI.SetActive(true);
    }

    public void ExitBudgetPhotos()
    {
        GameController.Instance.SetGameState(GameState.Playing);
        budgetUI.SetActive(false);
    }

    private void InitializeButtonDictionary()
    {
        buttonToPhotoInfo.Clear();

        int buttonCount = postItsParent.childCount;
        int panelCount = photosInformationParent.childCount;

        if (buttonCount != panelCount)
        {
            Debug.LogError($"⚠️ Mismatch: {buttonCount} buttons but {panelCount} panels found!");
            return;
        }

        for (int i = 0; i < buttonCount; i++)
        {
            Button button = postItsParent.GetChild(i).GetComponent<Button>();
            Transform photoInfo = photosInformationParent.GetChild(i);

            if (button != null && photoInfo != null)
            {
                buttonToPhotoInfo[button] = photoInfo;
                button.onClick.AddListener(() => UpdatePhotoDisplay(photoInfo));
            }
        }
    }

    public void UpdatePhotoDisplay(Transform activePhotoInfo)
    {
        if (activePhotoInfo == null)
        {
            Debug.LogWarning("⚠️ No valid PhotosInformation panel found.");
            return;
        }

        // Deactivate all panels before activating the selected one
        foreach (Transform panel in photosInformationParent)
        {
            panel.gameObject.SetActive(false);
        }

        // Activate the selected panel
        activePhotoInfo.gameObject.SetActive(true);

        HashSet<string> collectedPhotoIDs = GameController.Instance.GetCollectedPhotoIDs();
        PhotoUI photoUI = activePhotoInfo.GetComponent<PhotoUI>();

        if (photoUI != null)
        {
            bool isCollected = collectedPhotoIDs.Contains(photoUI.photoData.id);

            if (!isCollected)
            {
                lockedUI.transform.SetParent(activePhotoInfo);
                lockedUI.transform.SetAsLastSibling();
                lockedUI.SetActive(true);
            }
            else
            {
                lockedUI.SetActive(false);
                AssignSceneButton(activePhotoInfo, photoUI.photoData);
            }
        }
    }

    private void AssignSceneButton(Transform activePhotoInfo, PhotoData photoData)
    {
        Button sceneLoadButton = activePhotoInfo.GetChild(0).GetComponentInChildren<Button>(true);

        if (sceneLoadButton != null)
        {
            sceneLoadButton.onClick.RemoveAllListeners();
            sceneLoadButton.onClick.AddListener(() => LoadPhotoScene(photoData));
        }
    }

    private void LoadPhotoScene(PhotoData photoData)
    {
        if (photoData == null)
        {
            Debug.LogError("❌ PhotoData is missing. Cannot load scene.");
            return;
        }

        // Load the scene with the Skybox
        SceneController.LoadPhotoScene("PhotoSkyBox", photoData.skyboxMaterial);
    }
}
