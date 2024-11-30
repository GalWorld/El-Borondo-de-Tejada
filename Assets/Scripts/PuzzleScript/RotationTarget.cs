using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTarget : MonoBehaviour
{
    //Is the reference of the LampTarget
    [SerializeField] Transform target;

    void Update()
    {
        transform.LookAt(target);
    }
}
