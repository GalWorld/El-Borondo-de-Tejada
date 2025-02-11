using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPhotoData", menuName = "Photo Collection/PhotoData")]
public class PhotoData : ScriptableObject
{
    public string id;
    public string title;
    public string description;
    public Sprite illustration;
    public List<Sprite> animationFrames;
    public Texture2D image360;
    public List<string> funFacts;
}
