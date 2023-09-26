using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStomach : MonoBehaviour, IStomach
{
    public int FoodAmount { get => _foodamount; set => _foodamount = value; }

    private int _foodamount = 0;
}