using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        print("Collision detected!");

        float distance = Vector3.Distance(transform.position, other.transform.position);
        float step = 10.0f * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, other.transform.position, step);
        if (distance < 0.1f)
        {
            // Destroy
        }
        IInventory inventory = other.GetComponent<IInventory>();

        if (inventory != null)
        {
            inventory.ItemAmount = inventory.ItemAmount + 1;
            print("Player inventory has " + inventory.ItemAmount + " items in it!");
        }
    }
}
