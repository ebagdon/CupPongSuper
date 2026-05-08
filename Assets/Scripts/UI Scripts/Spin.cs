using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    // our zRotation and the rotate speed
    private float zRotation;
    [SerializeField] private float zRotateSpeed = -7f;

    private void Update()
    {
        // rotate
        Rotate();
    }

    void Rotate()
    {
        // add to the rotation and set the rotation
        zRotation += zRotateSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, zRotation);
    }
}