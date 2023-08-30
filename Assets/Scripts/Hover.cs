using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    public float hoverSpeed;
    public float hoverRange;

    void Update()
    {
        Vector3 p = transform.position;
        p.y = (Mathf.Cos((Time.time / 2) * hoverSpeed) / 20) * hoverRange;
        transform.position = p;
    }
}
