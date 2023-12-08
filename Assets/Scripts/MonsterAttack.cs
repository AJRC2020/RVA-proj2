using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    public int Power { get; set; }

    public int Slots { get; set; }

    public string Type { get; set; }

    public bool hasSpecialEffect { get; set; }

    public Action<MonsterGeneric> specialEffect { get; set; }
}
