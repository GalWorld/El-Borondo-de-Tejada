using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMover : MonoBehaviour
{
    [SerializeField] Transform[] points; // Points Array
    private float moveSpeed = 2f; // Move Speed
    private int currentPointIndex = 0; // Current Point
    private bool isMoving = false; // Moving Boolean

    // Start Movement
    public void MoveToNextPoint()
    {
        if (points.Length == 0) return; // Check if there are points in the array

        // Update new point index (in sequential order)
        currentPointIndex = (currentPointIndex + 1) % points.Length;

        // Start Movement coroutine if not already moving
        if (!isMoving)
        {
            StartCoroutine(MoveToPoint(points[currentPointIndex].position));
        }
    }

    // Movement coroutine
    private IEnumerator MoveToPoint(Vector3 targetPosition)
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f) // Check if we are close enough
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime); // Move towards the target
            yield return null; // Wait until the next frame
        }

        transform.position = targetPosition; // Snap to the final position
        isMoving = false;
    }
}
