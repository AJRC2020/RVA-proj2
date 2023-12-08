using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightRhinoController : MonsterGeneric
{
    // Start is called before the first frame update
    void Start()
    {
        Health = 100;
        MaxHealth = 100;
        Attack = 10;
        Defense = 15;
        Speed = 8;
        Type = "Water";
        
        MonsterAttack attack1 = new MonsterAttack();
        attack1.Type = "Normal";
        attack1.Slots = 10;
        attack1.Power = 10;
        attack1.hasSpecialEffect = false;

        MonsterAttack attack2 = new MonsterAttack();
        attack2.Type = "Water";
        attack2.Slots = 5;
        attack2.Power = 15;
        attack2.hasSpecialEffect = true;
        attack2.specialEffect = (enemy) =>
        {
            enemy.effect = "Confused";
            enemy.effectTurns = 5;
        };

        Attacks = new List<MonsterAttack> { attack1, attack2 };
    }
}
