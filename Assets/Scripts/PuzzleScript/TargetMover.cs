using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMover : MonoBehaviour
{
    public Transform[] points; // Points Array
    public float moveSpeed = 2f; // Move Speed
    private int currentPointIndex = 0; // Current Point
    private bool isMoving = false; // Moving Boolean

    // Start Movement
    public void MoveToNextPoint()
    {
        if (points.Length == 0) return;

        // Update new points
        currentPointIndex = (currentPointIndex + 1) % points.Length;

        // Start Movement coroutine
        if (!isMoving)
        {
            StartCoroutine(MoveToPoint(points[currentPointIndex].position));
        }
    }

    // Movement coroutine
    private IEnumerator MoveToPoint(Vector3 targetPosition)
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition; 
        isMoving = false;
    }
}
