using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private GameObject Player;
    private FoodSpawner foodSpawner;
    [SerializeField] private float grow;
    [SerializeField] private float pickupRange;
    [SerializeField] private float flightSpeed;

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
        print("Collided!");
        if (other.tag.Equals("Player"))
        {
            if (gameObject.tag.Equals("Food"))
            {
                print("Player has eaten food!");
                Destroy(gameObject);

                foodSpawner.CreateFood();

                this.gameObject.transform.localScale += new Vector3(grow, grow, grow);
                print("Player has grown!");

                IStomach stomach = other.GetComponent<IStomach>();
                if (stomach != null)
                {
                    stomach.FoodAmount = stomach.FoodAmount + 1;
                    
                    Destroy(gameObject);
                    foodSpawner.CreateFood();
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
                Destroy(gameObject);
            }
        }
    }
}
