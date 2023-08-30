using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private GameObject Player;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);
        
        if (distance < 10.0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, 1.0f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IInventory inventory = other.GetComponent<IInventory>();

        if (inventory != null)
        {
            inventory.ItemAmount = inventory.ItemAmount + 1;
            print("Player inventory has " + inventory.ItemAmount + " items in it!");
        }
    }
}
