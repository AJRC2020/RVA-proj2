using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MonsterGeneric : MonoBehaviour
{
    public int Health { get; set; }

    public int MaxHealth { get; set; }

    public int Speed { get; set; }

    public int Attack { get; set; }

    public int Defense { get; set; }

    public string Type { get; set; }

    public string Name { get; set; }

    public int EffectTurns { get; set; }

    public string Effect { get; set; }

    public Animator Animator { get; set; }

    public List<MonsterAttack> Attacks { get; set; }

    public void Fight(int index, MonsterGeneric monster)
    {
        if (EffectTurns != 0 && Effect == "Confused") 
        {
            EffectTurns--;
            //Debug.Log(Name + " is Confused");
            if (Random.Range(0, 1) > 0.75f)
            {
                Health -= (int)(Attack / 2.5f);
                if (Health < 0)
                {
                    Health = 0;
                }
                return;
            }
        }

        switch(index)
        {
            case 0:
                Animator.SetTrigger("Attack1");
                break;

            case 1:
                Animator.SetTrigger("Attack2");
                break;
        }

        MonsterAttack attack = Attacks[index];

        float defenseModified = monster.Defense * TypeVariation(attack.Type, monster.Type);

        float damage = CalculateDamage(Attack, attack.Power, defenseModified, Stab(attack.Type));

        //Debug.Log(Name + " used " + attack.Name + "\n" + monster.Name + " took " + (int)damage);

        monster.Health -= (int)damage;
        monster.Animator.SetTrigger("IsHit");

        if (monster.Health < 0)
        {
            monster.Health = 0;
        }

        if (attack.HasSpecialEffect) 
        {
            attack.SpecialEffect?.Invoke(monster);
        }

        if (EffectTurns != 0 && Effect == "Poisoned")
        {
            Health -= MaxHealth / 9;
            if (Health < 0)
            {
                Health = 0;
            }
            EffectTurns--;

            //Debug.Log(Name + " is poisoned");
        }
                
        if (EffectTurns == 0 && (Effect == "Poisoned" || Effect == "Confused"))
        {
            Effect = "None";
        }
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
