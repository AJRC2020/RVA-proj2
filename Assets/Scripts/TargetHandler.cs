using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class TargetHandler : MonoBehaviour
{

    public List<Target> targetsOnScreen;

    public HashSet<Target> capturedTargets;

    public GameObject AquarhinoMarker;
    public GameObject PyroscarabMarker;
    public GameObject PricklashMarker;

    
    private void Start()
    {
        targetsOnScreen = new List<Target>();
        capturedTargets = new HashSet<Target>();
        CaptureCactusController.OnPricklashCaptured += CapturePricklash;
        CaptureInsectController.OnPyroscarabCaptured += CapturePyroscarab;
        CaptureRhinoController.OnAquarhinoCaptured += CaptureAquarhin;
    }

    private void OnDisable()
    {
        CaptureCactusController.OnPricklashCaptured -= CapturePricklash;
        CaptureInsectController.OnPyroscarabCaptured -= CapturePyroscarab;
        CaptureRhinoController.OnAquarhinoCaptured -= CaptureAquarhin;
    }

    private void OnEnable()
    {
        CaptureCactusController.OnPricklashCaptured += CapturePricklash;
        CaptureInsectController.OnPyroscarabCaptured += CapturePyroscarab;
        CaptureRhinoController.OnAquarhinoCaptured += CaptureAquarhin;
    }

    private void OnDestroy()
    {
        CaptureCactusController.OnPricklashCaptured -= CapturePricklash;
        CaptureInsectController.OnPyroscarabCaptured -= CapturePyroscarab;
        CaptureRhinoController.OnAquarhinoCaptured -= CaptureAquarhin;
    }

    private void Update()
    {
        CheckIfBattle();
    }


    private GameObject TargetEnumToTargetObject(Target target)
    {
        if (target == Target.Phyroscarab)
        {
            return PyroscarabMarker;
        }else if (target == Target.Aquarhin)
        {
            return AquarhinoMarker;

        } else if (target == Target.Pricklash)
        {
            return PyroscarabMarker;
        }

        return null;
    }
    
    private void CheckIfBattle()
    {
        // Check if there are two markers on the screen
        if (targetsOnScreen.Count == 2)
        {
            List<Target> playerCards = new List<Target>();
            Target enemyCard;
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
                    // TODO: start other scene
                    // The markers are facing each other
                }
            }
            

        } 
    }

    public int GetNumberOfTargetsOnScreen()
    {
        return targetsOnScreen.Count;
    }

    private void CaptureAquarhin()
    {
        capturedTargets.Add(Target.Aquarhin);
    }

    public void AddAquarhin()
    {
        targetsOnScreen.Add(Target.Aquarhin);
    }
    
    public void RemoveAquarhin()
    {
        targetsOnScreen.Remove(Target.Aquarhin);
    }
    
    private void CapturePyroscarab()
    {
        capturedTargets.Add(Target.Phyroscarab);
    }
    
    public void AddPyroscarab()
    {
        targetsOnScreen.Add(Target.Phyroscarab);
    }
    
    public void RemovePyroscarab()
    {
        targetsOnScreen.Remove(Target.Phyroscarab);
    }
    
    private void CapturePricklash()
    {
        capturedTargets.Add(Target.Pricklash);
    }
    
    public void AddPricklash()
    {
        targetsOnScreen.Add(Target.Pricklash);
    }
    
    public void RemovePricklash()
    {
        targetsOnScreen.Remove(Target.Pricklash);
    }
    
}
