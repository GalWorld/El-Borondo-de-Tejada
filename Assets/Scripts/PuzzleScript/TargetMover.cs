using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMover : MonoBehaviour
{
    [SerializeField] Transform[] points; // Points Array
    private float moveSpeed = 4f; // Move Speed
    private int currentPointIndex = 0; // Current Point
    private bool isMoving = false; // Moving Boolean

    // Start Movement
    public void MoveToNextPoint()
    {
        if (points.Length == 0 || isMoving) return; // No hacer nada si ya está moviéndose

        // Actualizar el índice al siguiente punto
        currentPointIndex = (currentPointIndex + 1) % points.Length;

        // Iniciar la corrutina de movimiento
        StartCoroutine(MoveToPoint(points[currentPointIndex].position));
    }

    // Movement coroutine
    private IEnumerator MoveToPoint(Vector3 targetPosition)
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f) // Check if we are close enough
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime); // Move towards the target
            yield return null; // Wait until the next frame
            //Debug.Log(points[currentPointIndex]);
        }

        transform.position = targetPosition; // Snap to the final position
        isMoving = false;
    }
}
