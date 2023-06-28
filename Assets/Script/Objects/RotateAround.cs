using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] private float degreesPerSecond = 45f;
    void Update()
    {
        transform.LookAt(target);
        transform.RotateAround(target.position, Vector3.forward, degreesPerSecond * Time.deltaTime);
    }
}
