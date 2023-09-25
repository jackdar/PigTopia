using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePlayer : MonoBehaviour
{
    public Text player_score;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        player_score.text = "Score: " + score;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        IStomach stomach = other.GetComponent<IStomach>();
        if(other.tag == "Food")
        {
            if(stomach != null)
            {
                score += 10;
                player_score.text = "Score: "+ score;
                print("Score added!");
                Destroy(other.gameObject);
            }
        }
    }
}
