using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatBubble : MonoBehaviour
{
    private TextMeshPro nameNPC;
    private TextMeshPro textMessage;
    private Transform mainCameraTransform;

    private void Awake() 
    {
        nameNPC =  transform.Find("NameNPC").GetComponent<TextMeshPro>();
        textMessage =  transform.Find("Text").GetComponent<TextMeshPro>();
    }

    private void OnEnable()
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        if (camera != null)
        {
            mainCameraTransform = camera.transform;
        }
        else
        {
            Debug.LogWarning("Main Camera not found. Make sure the Camera has the 'MainCamera' tag.");
        }
    }

    private void Start() 
    {
        SetUp("Mariposa", "Ve hacia el portal para encontrar una de tus preciadas gatas!");    
    }

    private void FixedUpdate() 
    {
        if (mainCameraTransform != null)
        {
            Vector3 direction = mainCameraTransform.position - transform.position;
            direction.y = 0; 
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void SetUp(string npcName, string message)
    {
        nameNPC.SetText(npcName);
        textMessage.SetText(message);
    }
}
