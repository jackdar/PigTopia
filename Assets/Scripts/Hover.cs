using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    void Update()
    {
        Vector3 p = transform.position;
        p.y = Mathf.Cos(Time.time) / 4;
        transform.position = p;
    }
}
