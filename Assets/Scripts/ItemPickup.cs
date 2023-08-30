using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private GameObject Player;
    public float pickupRange;
    public float flightSpeed;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);
        
        if (distance < pickupRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, flightSpeed * Time.deltaTime);
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

        Destroy(gameObject);
    }
}
