using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    [SerializeField] private float hoverSpeed;
    [SerializeField] private float hoverRange;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }
    
    void Update()
    {
        Vector3 p = transform.position;
        p.y = (Mathf.Cos((Time.time / 2) * hoverSpeed) / 20) * hoverRange;
        transform.position = p;
    }
}
