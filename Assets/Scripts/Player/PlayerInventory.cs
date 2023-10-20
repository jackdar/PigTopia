using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, IInventory
{
    public int ItemAmount { get => _itemamount; set => _itemamount = value; }

    private int _itemamount = 0;
}
