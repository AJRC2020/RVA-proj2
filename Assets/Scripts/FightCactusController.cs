using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCactusController : MonsterGeneric
{
    // Start is called before the first frame update
    void Start()
    {
        Health = 130;
        MaxHealth = 130;
        Attack = 17;
        Defense = 8;
        Speed = 6;
        Type = "Grass";

        MonsterAttack attack1 = new MonsterAttack();
        attack1.Type = "Normal";
        attack1.Slots = 10;
        attack1.Power = 15;
        attack1.hasSpecialEffect = false;

        MonsterAttack attack2 = new MonsterAttack();
        attack2.Type = "Grass";
        attack2.Slots = 4;
        attack2.Power = 8;
        attack2.hasSpecialEffect = true;
        attack2.specialEffect = (enemy) =>
        {
            enemy.effect = "Poisoned";
            enemy.effectTurns = 5;
        };

        Attacks = new List<MonsterAttack> { attack1, attack2 };
    }
}
