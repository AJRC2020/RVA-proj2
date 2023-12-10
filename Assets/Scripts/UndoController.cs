using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UndoController : MonoBehaviour
{
    public MonsterGeneric attacker;
    public MonsterGeneric defender;
    public int attack;
    public int damage;
    public bool effected;
    public string effect;
    public bool firstEffectedTurn;
    public int effectedDamage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Undo()
    {
        attacker.Attacks[attack].Slots++;
        defender.Health += damage;

        if (firstEffectedTurn)
        {
            if (defender.Effect == "Burned")
            {
                defender.Defense += 3;
            }
            defender.Effect = "None";
            defender.EffectTurns = 0;
        }
        else if (effected)
        {
            attacker.Health += effectedDamage;
            attacker.EffectTurns++;
            attacker.Effect = effect;
        }
    }
}