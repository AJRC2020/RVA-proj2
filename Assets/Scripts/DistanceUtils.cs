

    using UnityEngine;

    public static class DistanceUtils
    {

        public static bool IsClosestToPlayerPlayer1(GameObject target1Obj, GameObject target2Obj)
        {
            // Getting positions and rotations of markers
            var position1 = target1Obj.transform.localPosition;
            var rotation1 = target1Obj.transform.localRotation;
            
            var position2 = target2Obj.transform.localPosition;
            var rotation2 = target2Obj.transform.localRotation;

            // Calculate the vector from the center of one marker to the other
            Vector3 direction = position2 - position1;
            
            Vector3 forward1 = rotation1 * Vector3.forward;
            Vector3 forward2 = rotation2 * Vector3.forward;

            // Calculate the dot product of the forward vectors
            float dotProduct1 =  Vector3.Dot(forward1, direction.normalized);
            float dotProduct2 =  Vector3.Dot(forward2, -direction.normalized);

            // Calculate the angles between the forward vectors
            float angle1 = Mathf.Acos(dotProduct1) * Mathf.Rad2Deg;
            float angle2 = Mathf.Acos(dotProduct2) * Mathf.Rad2Deg;

            return angle1 > angle2;
        }

        // TODO: Not tested...
        public static void MarkerIsOnSide(GameObject smallerMarker, GameObject biggerMarker)
        {
            // Get positions and rotations
            Transform largerMarkerTransform = biggerMarker.transform;
            Transform smallerMarkerTransform = smallerMarker.transform;

            Vector3 largerMarkerPosition = largerMarkerTransform.position;
            Vector3 smallerMarkerPosition = smallerMarkerTransform.position;

            Quaternion largerMarkerRotation = largerMarkerTransform.rotation;
            Quaternion smallerMarkerRotation = smallerMarkerTransform.rotation;

            // Check if the smaller marker is pointing at the bottom of the larger marker
            Vector3 directionToLargerMarker = (largerMarkerPosition - smallerMarkerPosition).normalized;
            float dotProduct = Vector3.Dot(directionToLargerMarker, smallerMarkerTransform.up);

            if (dotProduct < 0)
            {
                // Smaller marker is pointing at the bottom of the larger marker

                // Determine if the smaller marker is on the left or right
                Vector3 crossProduct = Vector3.Cross(smallerMarkerTransform.up, directionToLargerMarker);
    
                if (crossProduct.y > 0)
                {
                    Debug.Log("Smaller marker is on the left and pointing at the bottom of the larger marker!");
                }
                else if (crossProduct.y < 0)
                {
                    Debug.Log("Smaller marker is on the right and pointing at the bottom of the larger marker!");
                }
                else
                {
                    // The markers have the same y-coordinate (could be considered as aligned)
                    Debug.Log("Smaller marker is pointing at the bottom, but its position is aligned with the larger marker.");
                }
            }
            else
            {
                // Smaller marker is not pointing at the bottom of the larger marker
                Debug.Log("Smaller marker is not pointing at the bottom of the larger marker.");
            }

        }
        
        public static bool IsBattlePosition(GameObject target1Obj, GameObject target2Obj)
        {
            // Getting positions and rotations of markers
            var position1 = target1Obj.transform.localPosition;
            var rotation1 = target1Obj.transform.localRotation;
            
            var position2 = target2Obj.transform.localPosition;
            var rotation2 = target2Obj.transform.localRotation;

            // Calculate the vector from the center of one marker to the other
            Vector3 direction = position2 - position1;
            // Check if the markers are close by on the same plane
            float distanceThreshold = 2.0f; // Adjust this threshold as needed
            if (direction.magnitude < distanceThreshold)
            {
                float angleThreshold = 30f; // Adjust this threshold as needed

                // Calculate the forward vectors of the rotations
                Vector3 forward1 = rotation1 * Vector3.forward;
                Vector3 forward2 = rotation2 * Vector3.forward;

                // Calculate the dot product of the forward vectors
                float dotProduct1 =  Vector3.Dot(forward1, direction.normalized);
                float dotProduct2 =  Vector3.Dot(forward2, -direction.normalized);

                // Calculate the angles between the forward vectors
                float angle1 = Mathf.Acos(dotProduct1) * Mathf.Rad2Deg;
                float angle2 = Mathf.Acos(dotProduct2) * Mathf.Rad2Deg;


                // Check if either angle is within the threshold
                if (angle1 < angleThreshold && angle2 < angleThreshold)
                {
                    return true;
                }
            }

            return false;
        }
        

    }
