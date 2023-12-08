using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGeneric : MonoBehaviour
{
    public int Health { get; set; }

    public int MaxHealth { get; set; }

    public int Speed { get; set; }

    public int Attack { get; set; }

    public int Defense { get; set; }

    public string Type { get; set; }

    public int effectTurns { get; set; }

    public string effect { get; set; }

    public List<MonsterAttack> Attacks { get; set; }

    public void Fight(int index, MonsterGeneric monster)
    {
        if (effectTurns != 0 && effect == "Confused") 
        {
            if (Random.Range(0, 1) > 0.75f)
            {
                Health -= (int)(Attack / 2.5f);
                if (Health < 0)
                {
                    Health = 0;
                }
                return;
            }
            effectTurns--;
        }

        MonsterAttack attack = Attacks[index];

        float defenseModified = monster.Defense * TypeVariation(attack.Type, monster.Type);

        float damage = CalculateDamage(Attack, attack.Power, defenseModified, Stab(attack.Type));

        monster.Health -= (int)damage; 

        if (monster.Health < 0)
        {
            monster.Health = 0;
        }

        if (attack.hasSpecialEffect) 
        {
            attack.specialEffect?.Invoke(monster);
        }

        if (effectTurns != 0 && effect == "Poisoned")
        {
            Health -= MaxHealth / 7;
            if (Health < 0)
            {
                Health = 0;
            }
            effectTurns--;
        }
    }

    public void EnableDefensePosition()
    {
        Defense += 5;
        Attack -= 5;
    }

    public void DisableDefensePosition()
    {
        Defense -= 5;
        Attack += 5;
    }

    float CalculateDamage(int userAttack, int movePower, float enemyDefense, bool hasStab)
    {
        if (hasStab)
        {
            return 1.25f * userAttack * movePower * Random.Range(0.85f, 1.15f) / enemyDefense;
        }
        else
        {
            return userAttack * movePower * Random.Range(0.85f, 1.15f) / enemyDefense;
        }
    }

    bool Stab(string attackType)
    {
        return attackType == Type;
    }

    float TypeVariation(string attackType, string enemyType)
    {
        if (attackType == "Fire" && enemyType == "Water" ||
            attackType == "Water" && enemyType == "Grass" ||
            attackType == "Grass" && enemyType == "Fire")
        {
            return 1.25f;
        }

        if (attackType == "Fire" && enemyType == "Grass" ||
            attackType == "Grass" && enemyType == "Water" ||
            attackType == "Water" && enemyType == "Fire")
        {
            return 0.75f;
        }

        return 1.0f;
    }
}
