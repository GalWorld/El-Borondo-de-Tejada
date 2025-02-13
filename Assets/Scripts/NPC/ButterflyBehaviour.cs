using UnityEngine;

public class ButterflyBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject preButterfly;
    [SerializeField] private GameObject postButterfly;

    private bool hasSwitched = false; 

    private void FixedUpdate()
    {
        if (!hasSwitched) 
        {
            GameObject catObject = GameObject.FindGameObjectWithTag("Cat");

            if (catObject != null && catObject.activeSelf)
            {
                if (preButterfly != null)
                    preButterfly.SetActive(false);

                if (postButterfly != null)
                    postButterfly.SetActive(true);

                hasSwitched = true; 
            }
        }
    }
}
