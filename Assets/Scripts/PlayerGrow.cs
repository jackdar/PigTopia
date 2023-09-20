using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrow : MonoBehaviour
{
    public void IncreasePlayerSize(float growAmount) {
        this.gameObject.transform.localScale += new Vector3(growAmount, growAmount, growAmount);
        print("Player has grown!");
    }
}
