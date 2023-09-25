using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour
{
    private GameObject Player;
    private FoodSpawner foodSpawner;
    private CinemachineVirtualCamera vcam;
    private AudioSource popSound;
    public Text player_score;
    private int score;

    [SerializeField] private float pickupRange;
    [SerializeField] private float flightSpeed;

    [SerializeField] private float playerGrowSpeed;
    [SerializeField] private float playerGrowAmount;

    [SerializeField] private float cameraGrowSpeed;
    [SerializeField] private float cameraGrowAmount;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        foodSpawner = FoodSpawner.ins;
        vcam = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
        popSound = gameObject.GetComponent<AudioSource>();
        score = 0;
<<<<<<< Updated upstream
        player_score.text = "Score: " + score;
=======
        player_score.text = "Score: "+score;
>>>>>>> Stashed changes
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        Vector3 playerCollisionPos = new Vector3(
            Player.transform.position.x,
            Player.transform.position.y - (Player.transform.localScale.y / 10),
            Player.transform.position.z
        );

        if (distance < pickupRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerCollisionPos, flightSpeed * Time.deltaTime);
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
                    score += 10;
<<<<<<< Updated upstream
                    player_score.text = "Score: "+ score;
                    print("Score added!");
=======
                    player_score.text = "Score" + score;
                    print("Score Added!");
>>>>>>> Stashed changes
                    print("Player has eaten food!");
                    popSound.Play();
                    Destroy(gameObject);
                    foodSpawner.CreateFood();
                    Vector3 playerCurrentSize = GameObject.Find("Player_Prefab").transform.localScale;
                    Vector3 playerGrowSize = new Vector3(playerCurrentSize.x + playerGrowAmount, playerCurrentSize.y + playerGrowAmount, playerCurrentSize.z + playerGrowAmount);
                    GameObject.Find("Player_Prefab").transform.localScale = Vector3.Lerp(playerCurrentSize, playerGrowSize, Mathf.SmoothStep(0f, 1f, playerGrowSpeed * Time.deltaTime));
                    if (stomach.FoodAmount % 20 == 0) {
                        vcam.m_Lens.OrthographicSize = Mathf.Lerp(
                            vcam.m_Lens.OrthographicSize, 
                            vcam.m_Lens.OrthographicSize + cameraGrowAmount, 
                            cameraGrowSpeed * Time.deltaTime
                        );
                    }
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
