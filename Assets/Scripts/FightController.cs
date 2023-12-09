using System;
using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FightController : MonoBehaviour
{
    public Slider playerHealth;
    public Image playerHealthFill;

    public Slider enemyHealth;
    public Image enemyHealthFill;

    public TextMeshProUGUI turnText;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI enemyName;

    public Image playerBurned;
    public Image playerPoisoned;
    public Image playerConfused;
    public Image enemyBurned;
    public Image enemyPoisoned;
    public Image enemyConfused;
    public GameObject runAwayScreen;
    public TextMeshProUGUI messageObj;
    public TextMeshProUGUI time;


    private MonsterGeneric monster1;
    private MonsterGeneric monster2;
    private int turn = 1;
    private int winner;
    private bool isOver = false;
    private bool midTurn = false;
    private bool firstMoved = false;
    private float timeout = 1.5f;

    private readonly List<Target> _targetsOnScreen = new List<Target>();
    private Target _player;
    private Target _enemy;


    public void OnFindTarget(String target)
    {
        Debug.Log(target);
        Enum.TryParse(target, out Target targetEnum);
        _targetsOnScreen.Add(targetEnum);
    }

    public void OnLoseTarget(String target)
    {
        Debug.Log(target);

        Enum.TryParse(target, out Target targetEnum);
        _targetsOnScreen.Remove(targetEnum);
    }

    private void CheckForMarkers()
    {
        if (_targetsOnScreen.Count != 2)
        {
            if (!_targetsOnScreen.Contains(_player))
            {
                StartCoroutine(RunAwayTimer("Cannot find the player card (" + _player + "). Add it back to the field."));
            }else if (!_targetsOnScreen.Contains(_enemy))
            {
                StartCoroutine(RunAwayTimer("Cannot find the enemy card (" + _enemy + "). Add it back to the field."));
            }
            
            var differentTarget = _targetsOnScreen.FirstOrDefault((target => (target != _player && target != _enemy)));

            if (!differentTarget.Equals(default(Target)))
            {
                StartCoroutine(RunAwayTimer("Only the monsters battling should be on the field. Remove the " + differentTarget + " card."));
            }
            else
            {
                StartCoroutine(RunAwayTimer("Only the monsters battling should be on the field. Remove the extras"));

            }
        }
        else
        {
            if (_targetsOnScreen.Contains(_player))
            {
                if (_targetsOnScreen.Contains(_enemy))
                {
                    var differentTarget = _targetsOnScreen.FirstOrDefault((target => (target != _player && target != _enemy)));
                    if (!differentTarget.Equals(default(Target)))
                    {
                        StartCoroutine(RunAwayTimer("Only the monsters battling should be on the field. Remove the " + differentTarget + " card."));
                        return;
                    }
                    
                    //Markers are correct -> Check Distance and position

                    DontRunAway();
                }
                else
                {
                    StartCoroutine(RunAwayTimer("Cannot find the enemy card (" + _enemy + "). Add it back to the field."));
                }
            }
            else{
                StartCoroutine(RunAwayTimer("Cannot find the player card (" + _player + "). Add it back to the field."));
            }
            
        }
        
    }
    
    IEnumerator RunAwayTimer(String message)
    {
        runAwayScreen.SetActive(true);
        messageObj.text = message;
        time.text = "5s";
        yield return new WaitForSeconds(1);
        time.text = "4s";

        yield return new WaitForSeconds(1);
        time.text = "3s";

        yield return new WaitForSeconds(1);
        time.text = "2s";

        yield return new WaitForSeconds(1);
        time.text = "1s";

        yield return new WaitForSeconds(1);
        time.text = "0s";

        RunAway();
    }

    public void RunAway()
    {
        SceneManager.LoadScene("SampleScene");

    }

    private void DontRunAway()
    {
        StopAllCoroutines();
        runAwayScreen.SetActive(false);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _player = CaptureInfo.PlayerTarget;
        _enemy = CaptureInfo.EnemyTarget;
        _targetsOnScreen.Add(_player);
        _targetsOnScreen.Add(_enemy);
        
        switch (CaptureInfo.PlayerTarget)
        {
            case Target.Aquarhin:
                GameObject.Find("PlayerCactusTarget").SetActive(false);
                GameObject.Find("PlayerInsectTarget").SetActive(false);
                monster1 = GameObject.Find("PlayerRhinoTarget").GetComponentInChildren<FightRhinoController>();
                playerName.text = "Aquarhin";
                break;

            case Target.Pricklash:
                GameObject.Find("PlayerInsectTarget").SetActive(false);
                GameObject.Find("PlayerRhinoTarget").SetActive(false);
                monster1 = GameObject.Find("PlayerCactusTarget").GetComponentInChildren<FightCactusController>();
                playerName.text = "Pricklash";
                break;

            case Target.Pyroscarab:
                GameObject.Find("PlayerRhinoTarget").SetActive(false);
                GameObject.Find("PlayerCactusTarget").SetActive(false);
                monster1 = GameObject.Find("PlayerInsectTarget").GetComponentInChildren<FightInsectController>();
                playerName.text = "Pyroscarab";
                break;
        }

        switch (CaptureInfo.EnemyTarget)
        {
            case Target.Aquarhin:
                GameObject.Find("EnemyCactusTarget").SetActive(false);
                GameObject.Find("EnemyInsectTarget").SetActive(false);
                monster2 = GameObject.Find("EnemyRhinoTarget").GetComponentInChildren<FightRhinoController>();
                enemyName.text = "Aquarhin";
                break;

            case Target.Pricklash:
                GameObject.Find("EnemyInsectTarget").SetActive(false);
                GameObject.Find("EnemyRhinoTarget").SetActive(false);
                monster2 = GameObject.Find("EnemyCactusTarget").GetComponentInChildren<FightCactusController>();
                enemyName.text = "Pricklash";
                break;

            case Target.Pyroscarab:
                GameObject.Find("EnemyRhinoTarget").SetActive(false);
                GameObject.Find("EnemyCactusTarget").SetActive(false);
                monster2 = GameObject.Find("EnemyInsectTarget").GetComponentInChildren<FightInsectController>();
                enemyName.text = "Pyroscarab";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckForMarkers();
        if (timeout < 0)
        {
            if (!isOver)
            {
                CheckHealth();
                FightTurn();
                UpdateUI();
            }

            timeout = 1.5f;
        }
        else
        {
            timeout -= Time.deltaTime;
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

        Debug.Log("Player Health = " + monster1.Health + " Enemy Health = " + monster2.Health);
    }

    private bool RNG(float threshold)
    {
        return Random.Range(0f, 1f) > threshold;
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

    private void UpdateUI()
    {
        playerHealth.value = (float)monster1.Health / monster1.MaxHealth;
        enemyHealth.value = (float)monster2.Health / monster2.MaxHealth;

        if (playerHealth.value < 0.25f)
        {
            playerHealthFill.color = Color.red;
        }
        if (enemyHealth.value < 0.25f)
        {
            enemyHealthFill.color = Color.red;
        }

        turnText.text = "Turn " + turn;

        switch(monster1.Effect)
        {
            case null:
            case "None":
                playerBurned.enabled = false;
                playerPoisoned.enabled = false;
                playerConfused.enabled = false;
                break;

            case "Burned":
                playerBurned.enabled = true;
                playerPoisoned.enabled = false;
                playerConfused.enabled = false;
                break;

            case "Poisoned":
                playerBurned.enabled = false;
                playerPoisoned.enabled = true;
                playerConfused.enabled = false;
                break;

            case "Confused":
                playerBurned.enabled = false;
                playerPoisoned.enabled = false;
                playerConfused.enabled = true;
                break;
        }

        switch (monster2.Effect)
        {
            case null:
            case "None":
                enemyBurned.enabled = false;
                enemyPoisoned.enabled = false;
                enemyConfused.enabled = false;
                break;

            case "Burned":
                enemyBurned.enabled = true;
                enemyPoisoned.enabled = false;
                enemyConfused.enabled = false;
                break;

            case "Poisoned":
                enemyBurned.enabled = false;
                enemyPoisoned.enabled = true;
                enemyConfused.enabled = false;
                break;

            case "Confused":
                enemyBurned.enabled = false;
                enemyPoisoned.enabled = false;
                enemyConfused.enabled = true;
                break;
        }
    }
}
