using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPhotoData", menuName = "Photo Collection/PhotoData")]
public class PhotoData : ScriptableObject
{
    [Tooltip("Unique ID for the photo, used to track collection status.")]
    public string id;

    [Tooltip("Title of the photo displayed in the UI.")]
    public string title;

    [Tooltip("Description of the photo, providing details and context.")]
    public string description;

    [Tooltip("Icon representing the photo in menus and UI.")]
    public Sprite icon;

    [Tooltip("Animation clip that plays when the photo is collected.")]
    public AnimationClip animation;

    [Tooltip("360-degree image associated with the photo.")]
    public Texture2D image360;

    [Tooltip("List of interesting facts related to the photo.")]
    public List<string> funFacts;
}
