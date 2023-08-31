using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    #region instance
    public static FoodSpawner ins;

    private void Awake()
    {
        if (ins == null)
        {
            ins = this;
        }
    }
    #endregion

    public GameObject Food;
    public List<GameObject> Create_Food = new List<GameObject>();
    public int Max_Food = 50;
    public float Time_To_Instantiate = 0.0f;
    public Vector2 pos;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(CreateFood());
    }

    public IEnumerator CreateFood()
    {
        while (true)
        {
            if (Create_Food.Count <= Max_Food)
            {
                Vector2 Position = new Vector2(Random.Range(-pos.x, pos.x), Random.Range(-pos.y, pos.y));
                Position /= 2;
                GameObject m = Instantiate(Food, Position, Quaternion.identity);
                AddFood(m);
            }
            yield return new WaitForSeconds(Time_To_Instantiate);
        }
    }

    public void AddFood(GameObject m)
    {
        if (Create_Food.Contains(m) == false)
        {
            Create_Food.Add(m);
        }
    }

    public void RemoveFood(GameObject m)
    {
        if (Create_Food.Contains(m) == true)
        {
            Create_Food.Remove(m);
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, pos);
    }
}