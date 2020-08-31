using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    public float speed;
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.back, Time.deltaTime * speed);
    }
}
