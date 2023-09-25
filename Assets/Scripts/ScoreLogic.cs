using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreLogic : MonoBehaviour
{
    private int score;
    [SerializeField] TMP_Text playerScore;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        playerScore.text = score.ToString();
    }

    public int getScore()
    {
        return score;
    }

    public void increaseScore()
    {
        score++;
    }
}
