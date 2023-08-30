using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eat : MonoBehaviour
{

    public GameObject[] Food;
    public Vector2 pos;

    // Update is called once per frame
    public void Update()
    {
        if (GameObject.FindGameObjectWithTag("Food") != null)
        {
            Food = GameObject.FindGameObjectsWithTag("Food");
        }
    }

    public void RemoveFood(GameObject FoodObject)
    {
        List<GameObject> FoodList = new List<GameObject>();
        for (int i = 0; i < Food.Length; i++)
        {
            FoodList.Add(Food[i]);
        }
        FoodList.Remove(FoodObject);
        Food = FoodList.ToArray();
        FoodSpawner.ins.RemoveFood(FoodObject);
    }

    public void AddFood(GameObject FoodObject)
    {
        List<GameObject> FoodList = new List<GameObject>();
        for (int i = 0; i < Food.Length; i++)
        {
            FoodList.Add(Food[i]);
        }
        FoodList.Add(FoodObject);
        Food = FoodList.ToArray();
        FoodSpawner.ins.AddFood(FoodObject);
    }

    public void Check()
    {
        for (int i = 0; i < Food.Length; i++)
        {
            Transform m = Food[i].transform;

            // Adjusted distance calculation using pos values
            if (Vector2.Distance(transform.position, m.position) <= transform.localScale.x / 2 + pos.x / 2)
            {
                RemoveFood(m.gameObject);
                // eat 
                PlayerEat();

                // destroy
                fs.RemoveFood(m.gameObject);
                Destroy(m.gameObject);
            }
        }
    }

    FoodSpawner fs;
    // Start is called before the first frame update
    void Start()
    {
        FoodSpawner.ins.Player.Add(gameObject);
        Update();
        InvokeRepeating("Check", 0, 0.1f);
    }
    
    void PlayerEat()
    {
        transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
    } 
}