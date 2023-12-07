using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour
{
    private MonsterGeneric monster1;
    private MonsterGeneric monster2;
    private int turn = 0;
    private int winner;
    private bool isOver = false;
    private bool midTurn = false;
    private bool firstMoved = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOver)
        {
            CheckHealth();
            FightTurn();
        }
    }

    private void FightTurn()
    {
        if (monster1.Speed > monster2.Speed)
        {
            if (midTurn)
            {
                monster2.Fight(RandomAttack(), monster1);
                midTurn = false;
                turn++;
            }
            else
            {
                monster1.Fight(RandomAttack(), monster2);
                midTurn = true;
            }
        }
        else if (monster1.Speed == monster2.Speed)
        {
            if (midTurn && firstMoved)
            {
                monster2.Fight(RandomAttack(), monster1);
                midTurn = false;
                turn++;
            }
            else if (midTurn)
            {
                monster1.Fight(RandomAttack(), monster2);
                midTurn = false;
                turn++;
            }
            else
            {
                if (RNG(0.5f))
                {
                    monster1.Fight(RandomAttack(), monster2);
                    firstMoved = true;
                    midTurn = true;
                }
                else
                {
                    monster2.Fight(RandomAttack(), monster1);
                    firstMoved = false;
                    midTurn = true;
                }
            }
        }
        else
        {
            if (midTurn)
            {
                monster1.Fight(RandomAttack(), monster2);
                midTurn = false;
                turn++;
            }
            else
            {
                monster2.Fight(RandomAttack(), monster1);
                midTurn = true;
            }
        }
    }

    private bool RNG(float threshold)
    {
        return Random.Range(0, 1) > threshold;
    }

    private int RandomAttack()
    {
        if (RNG(0.7f))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private void CheckHealth()
    {
        if (monster1.Health == 0)
        {
            isOver = true;
            winner = 1;
            return;
        }
        if (monster2.Health == 0)
        {
            isOver = true;
            winner = 2;
            return;
        }
    }
}
