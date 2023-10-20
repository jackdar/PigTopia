using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSorter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            transform.parent.GetComponent<SpriteRenderer>().sortingOrder = collision.GetComponent<SpriteRenderer>().sortingOrder - 1;

        }
    }
}
