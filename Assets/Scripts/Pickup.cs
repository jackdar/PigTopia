using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private GameObject Player;
    private FoodSpawner foodSpawner;
    public float grow = 0.05f;
    public float pickupRange;
    public float flightSpeed;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        foodSpawner = FoodSpawner.ins;
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
        if (other.tag.Equals("Player"))
        {
            if (gameObject.tag.Equals("Food"))
            {
                IStomach stomach = other.GetComponent<IStomach>();
                if (stomach != null)
                {
                    stomach.FoodAmount = stomach.FoodAmount + 1;
                    print("Player has eaten food!");
                    Destroy(gameObject);
                    foodSpawner.CreateFood();
                    Vector3 newScale = Player.transform.localScale + new Vector3(grow, grow, grow);
                    Player.transform.localScale = newScale;
                }
            }

            if (gameObject.tag.Equals("Item"))
            {
                IInventory inventory = other.GetComponent<IInventory>();
                if (inventory != null)
                {
                    inventory.ItemAmount = inventory.ItemAmount + 1;
                    print("Player inventory has " + inventory.ItemAmount + " items in it!");
                }
            }

            Destroy(gameObject);
        }
    }
}
