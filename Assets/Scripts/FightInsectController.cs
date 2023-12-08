using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightInsectController : MonsterGeneric
{
    // Start is called before the first frame update
    void Start()
    {
        Health = 80;
        MaxHealth = 80;
        Attack = 12;
        Defense = 12;
        Speed = 10;
        Type = "Fire";

        MonsterAttack attack1 = new MonsterAttack();
        attack1.Type = "Normal";
        attack1.Slots = 10;
        attack1.Power = 12;
        attack1.hasSpecialEffect = false;

        MonsterAttack attack2 = new MonsterAttack();
        attack2.Type = "Fire";
        attack2.Slots = 5;
        attack2.Power = 15;
        attack2.hasSpecialEffect = true;
        attack2.specialEffect = (enemy) =>
        {
            if (enemy.Type != "Burned" && Random.Range(0, 1) > 0.5f)
            {
                enemy.effect = "Burned";
                enemy.Defense -= 3;
            }
        };

        Attacks = new List<MonsterAttack> { attack1, attack2 };
    }
}
