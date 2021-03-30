﻿using UnityEngine;

public class WheelController : MonoBehaviour
{
    public Vector3 rotateDirection;
    public float rotateSpeed;
    public bool isRotating;

    private void Update()
    {
        if (isRotating)
        {
            //Rotate the selected wheel in the direction
            //chosen at rotateSpeed Speed
            //if isRotating is checked in the inspector.
            transform.Rotate(rotateDirection * rotateSpeed * Time.deltaTime);
        }
    }
}