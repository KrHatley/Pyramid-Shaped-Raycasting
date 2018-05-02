using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script gets attached to two empty child gameobjects of the Player Character GameObject
/// These transform positions are used to determine the angle at which a RayCast should be preformed
/// </summary>

public class PlayerInteractionChild : MonoBehaviour
{
    
     [Tooltip("Positive or negative float in the X or Y axis less than the value of 1")] 
     [SerializeField] private Vector3 EmptyChildGameObjectPositionModifer;

    void FixedUpdate ()
    {
        this.transform.position = gameObject.transform.parent.position +
            gameObject.transform.parent.forward;
        transform.position += EmptyChildGameObjectPositionModifer;
        // Sets the transform of this attached object to the forward of the Parent object(In this case the Player Object)
        // Then subtracts/adds the editor exposed vector3 called EmptyChildGameObjectPositionModifer 
        // for the second child object it is recommended that the TargetPositionModifer = (-0.05,0,0)
        // for the third child object it is recommended that the TargetPositionModifer = (0,+0.05,0)
    }
}
