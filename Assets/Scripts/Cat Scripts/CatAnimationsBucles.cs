using System.Collections;
using UnityEngine;

public class CatAnimationsBucles : MonoBehaviour
{
    //Asign the animator
    [SerializeField] private Animator animator;
    //variable which contains the time between single chance
    [SerializeField] private float animationInterval = 10f; // Tiempo entre cambios de animaciones

    private void Start()
    {
        // Start the coroutine
        StartCoroutine(LoopIdleAnimations());
    }

    private IEnumerator LoopIdleAnimations()
    {
        while (true)
        {

            yield return new WaitForSeconds(animationInterval);

            //Shoot the trigger every *animationInterval* seconds
            animator.SetTrigger("NextAnimation");
        }
    }
}
