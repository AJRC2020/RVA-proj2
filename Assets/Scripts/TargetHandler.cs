using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;
using UnityEngine.SceneManagement;

public class TargetHandler : MonoBehaviour
{

    public List<Target> targetsOnScreen;

    public HashSet<Target> capturedTargets;

    public GameObject AquarhinoMarker;
    public GameObject PyroscarabMarker;
    public GameObject PricklashMarker;

    public GameObject message;

    public delegate void MultipleTargetsDetected();

    public static event MultipleTargetsDetected onMultipleTargetsDetected;

    public delegate void OneTargetDetected(HashSet<Target> capturedTargets);

    public static event OneTargetDetected onOneTargetDetected;
    
    private void Start()
    {
        targetsOnScreen = new List<Target>();
        capturedTargets = new HashSet<Target>();
        TargetController.onTargetEnable += EnableTarget;
        TargetController.onTargetDisabled += DisableTarget;
        CaptureRhinoController.OnCaptured += OnCaptured;
        CaptureCactusController.OnCaptured += OnCaptured;
        CaptureInsectController.OnCaptured += OnCaptured;
    }
    
        
    private void OnDestroy()
    {
        TargetController.onTargetEnable -= EnableTarget;
        TargetController.onTargetDisabled -= DisableTarget;
        CaptureRhinoController.OnCaptured -= OnCaptured;
        CaptureCactusController.OnCaptured -= OnCaptured;
        CaptureInsectController.OnCaptured -= OnCaptured;

    }

    private void EnableTarget(GameObject monsterUI, GameObject monster, Target target)
    {
        targetsOnScreen.Add(target);
        monster.SetActive(true);

        if (targetsOnScreen.Count == 1)
        {
            message.SetActive(false);

            if (!capturedTargets.Contains(target))
            {
                monsterUI.SetActive(true);
            }
        }

        if (targetsOnScreen.Count == 2)
        {
            onMultipleTargetsDetected();
        }
    }

    private void DisableTarget(GameObject monsterUI, GameObject monster, Target target)
    {
        targetsOnScreen.Remove(target);
        monster.SetActive(false);

        if (monsterUI.activeSelf)
        {
            monsterUI.SetActive(false);
        }

        if (targetsOnScreen.Count == 1)
        {
            onOneTargetDetected(capturedTargets);
        }

        if (targetsOnScreen.Count == 0)
        {
            message.SetActive(true);
        }
        
    }

    private void OnCaptured(GameObject monsterUI,Target target)
    {
        capturedTargets.Add(target);
        monsterUI.SetActive(false);
    }

    private void Update()
    {
        CheckIfBattle();
        UpdateMessage();
    }


    private void UpdateMessage()
    {
        if (targetsOnScreen.Count == 0)
        {
            message.SetActive(true);
        }
        else
        {
            if (message.activeSelf)
            {
                message.SetActive(false);
            }
        }
    }


    private GameObject TargetEnumToTargetObject(Target target)
    {
        if (target == Target.Phyroscarab)
        {
            return PyroscarabMarker;
        }
        if (target == Target.Aquarhin)
        {
            return AquarhinoMarker;
        } 
        if (target == Target.Pricklash)
        {
            return PricklashMarker;
        }

        return null;
    }
    
    private void CheckIfBattle()
    {
        // Check if there are two markers on the screen
        if (targetsOnScreen.Count == 2)
        {
            List<Target> playerCards = new List<Target>();
            Target enemyCard = Target.Aquarhin;
            foreach (var target in targetsOnScreen)
            {
                if (capturedTargets.Contains(target))
                {
                    playerCards.Add(target);
                }
                else
                {
                    enemyCard = target;
                }

            }

            // If there are no player markers then return
            if (playerCards.Count == 0) return;
            
            // Getting positions and rotations of markers
            var target1 = targetsOnScreen[0];
            var target1Obj = TargetEnumToTargetObject(target1);
            var position1 = target1Obj.transform.localPosition;
            var rotation1 = target1Obj.transform.localRotation;
            
            var target2 = targetsOnScreen[1];
            var target2Obj = TargetEnumToTargetObject(target2);
            var position2 = target2Obj.transform.localPosition;
            var rotation2 = target2Obj.transform.localRotation;

            // Calculate the vector from the center of one marker to the other
            UnityEngine.Vector3 direction = position2 - position1;
            // Check if the markers are close by on the same plane
            float distanceThreshold = 2.0f; // Adjust this threshold as needed
            if (direction.magnitude < distanceThreshold)
            {
                float angleThreshold = 30f; // Adjust this threshold as needed

                // Calculate the forward vectors of the rotations
                UnityEngine.Vector3 forward1 = rotation1 * UnityEngine.Vector3.forward;
                UnityEngine.Vector3 forward2 = rotation2 * UnityEngine.Vector3.forward;

                // Calculate the dot product of the forward vectors
                float dotProduct1 =  UnityEngine.Vector3.Dot(forward1, direction.normalized);
                float dotProduct2 =  UnityEngine.Vector3.Dot(forward2, -direction.normalized);

                // Calculate the angles between the forward vectors
                float angle1 = Mathf.Acos(dotProduct1) * Mathf.Rad2Deg;
                float angle2 = Mathf.Acos(dotProduct2) * Mathf.Rad2Deg;


                // Check if either angle is within the threshold
                if (angle1 < angleThreshold && angle2 < angleThreshold)
                {
                    Target playerCard = playerCards[0];
                    if (playerCards.Count == 2)
                    {
                        if (angle1 > angle2)
                        {
                            playerCard = target1;
                            enemyCard = target2;
                            // Card 1 is facing the right way
                        }
                        else
                        {
                            playerCard = target2;
                            enemyCard = target1;
                            // Card 2 is facing the right way
                        }
                    }

                    CaptureInfo.PlayerTarget = playerCard;
                    CaptureInfo.EnemyTarget = enemyCard;
                    SceneManager.LoadScene("FightScene");


                    // TODO: start other scene
                    // The markers are facing each other
                }
            }
            

        } 
    }
}
