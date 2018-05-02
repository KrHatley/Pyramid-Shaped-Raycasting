using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///     This Script is to be used with/attached to the Player Character Game Object
///     Specificly, this script will be used for activating the Iinteractable interface
///     Using a child object and the quaterion angle, multiple raycasts allow for a wider area of interaction
/// </summary>

public class PlayerInteraction : MonoBehaviour
{

    private Rigidbody Rbody;
    [Tooltip("recommended value of 3.0f,Changes the distance of Player RayCasts")]
    [SerializeField] private float RaycastMaxDistance;
    [Tooltip("Empty GameObject which is made a child of the player, add component 'PlayerInteractionChild'")]
    [SerializeField] private GameObject Child;// Empty GameObject, should be child of the Player Object
    [Tooltip("Empty GameObject which is made a child of the player, add component 'PlayerInteractionChild'")]
    [SerializeField] private GameObject Child2;// Empty GameObject, should be child of the Player Object
    private Transform ChildTransform;// Transform of Child obejct
    private Transform Child2Transform;// Transform of Child obejct

    private GameObject[] Hits;// an array of the above GameObject information returned through raycasts
    private Quaternion RayRotate;
    private RaycastHit hit;// object information of the GameObject that the RayCast connects with
    private RaycastHit hit2;
    private RaycastHit hit3;
    private RaycastHit hit4;
    private RaycastHit hit5;
    
    private void Awake()
    {
        if (Child==null)
        {
            Child = GameObject.FindGameObjectWithTag("PlayerEmptyChild1");
            // finds object( Which should be an Empty Gameobject and Child of the player)
        }
        if (Child2==null)
        {
            Child2 = GameObject.FindGameObjectWithTag("PlayerEmptyChild2");
            // finds object( Which should be an Empty Gameobject and Child of the player)
        }

        Hits = new GameObject[5];
        Rbody = GetComponent<Rigidbody>();
        ChildTransform = Child.transform;
        // Grabs the  Child Gameobject and assigns it's transform to TargetLoc
        Child2Transform = Child2.transform; 
        // Grabs the Child2 Gameobejct and assigns it's transform to the TargetLoc2
    }
	
	// Update is called once per frame
	void Update ()
    {
        PyramidRayCastingForInteractables();

        if (Hits.Length > 0)// array size larger than zero
         {
           Array.Clear(Hits, 0, Hits.Length);// empties the array
         }
        // These next five lines are strictly for being able to see the rays inside the editor scene
        Debug.DrawRay(this.transform.position,  GetHorizontalVectorDirection(), Color.cyan);
        Debug.DrawRay(this.transform.position, GetNegativeHorizontalVectorDirection(), Color.cyan);
        Debug.DrawRay(this.transform.position, GetVerticalVectorDirection(), Color.magenta);
        Debug.DrawRay(this.transform.position, GetNegativeVerticalVectorDirection(), Color.magenta);
        Debug.DrawRay(this.transform.position, this.transform.forward, Color.yellow);
       
    }

    /// <summary> PyramidRayCastingForInteractables():
    /// called in FixedUpdate()
    /// calls Five RayCasts and Assigns the hit objects to the array 
    /// </summary>
    private void PyramidRayCastingForInteractables()
    {
        if (Physics.Raycast(this.transform.position, transform.forward, out hit, RaycastMaxDistance))
        {
            Hits[0] = hit.rigidbody.gameObject;
        }
        if (Physics.Raycast(this.transform.position, GetHorizontalVectorDirection(), out hit2, RaycastMaxDistance))
        {
            Hits[1] = hit2.rigidbody.gameObject;
        }
        if (Physics.Raycast(this.transform.position, GetNegativeHorizontalVectorDirection(), out hit3, RaycastMaxDistance))
        {
            Hits[2] = hit3.rigidbody.gameObject;
        }
        if (Physics.Raycast(this.transform.position, GetVerticalVectorDirection(), out hit4, RaycastMaxDistance))
        {
            Hits[3] = hit4.rigidbody.gameObject;
        }
        if (Physics.Raycast(this.transform.position, GetNegativeVerticalVectorDirection(), out hit5, RaycastMaxDistance))
        {
            Hits[4] = hit5.rigidbody.gameObject;
        }

        DisplayClosestObjectOptions();

    }
    /// <summary> DisplayClosestObjectOptions():
    /// Called in PyramidRayCastingForInteractables()
    /// Uses the first object in the array to determine the object closest to the player
    /// </summary>
    private void DisplayClosestObjectOptions()
    {
        GameObject closerToPlayer = Hits[0];
        if (Hits[0]!=null)
        {
            for (int i = 0; i < Hits.Length; i++)
            {
                if (Hits[i] == null)
                {
                    Hits[i] = Hits[0];
                    // failsafe to avoid null object reference that won't interfer with the below tests
                }
            }

            for (int i = 1; i < Hits.Length; i++)
            {
                if (Hits[0] != Hits[i])
                {
                    // Condition : array position i is not the same game object as array position 0(first position)
                    Vector3 Test = Hits[0].transform.position - transform.position;
                    Vector3 target = Hits[i].transform.position - transform.position;
                    if (target.magnitude < Test.magnitude)
                    {
                        // the size of target is less than the size of test
                        Vector3 Test2 = closerToPlayer.transform.position - transform.position;
                        if (target.magnitude < Test2.magnitude)
                        {
                            closerToPlayer = Hits[i];
                        }

                    }

                }
            }
            closerToPlayer.GetComponent<IinteractableOptions>().DisplayInteractOptions();
            // calls the interact method implemented by the Iinteractable interface attached to the closest objec to the player
            
        }
        else
        {
            Debug.Log("Nothing Detected!");
        }
        
    }

    /// <summary> GetHorizontalAngleFromEmptyChildPosition():
    /// Calculates the angle between the child object and GameObject's currecnt position
    /// determines axis of rotation(around the upward Y-Axis)
    /// this method is used to calculate the angle of a raycast based on ChildTransform postion
    /// </summary>
    Quaternion GetHorizontalAngleFromEmptyChildPosition()
    {
        Vector3 targetDir = ChildTransform.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);
        RayRotate = Quaternion.AngleAxis(angle, transform.up);
        return RayRotate;
    }

    /// <summary> GetVerticalAngleFromEmptyChildPosition():
    /// Calculates the angle between the child object and GameObject's currecnt position
    /// determines axis of rotation(around the forward X-Axis)
    /// this method is used to calculate the angle of a raycast based on Child2Transform postion
    /// </summary>
    Quaternion GetVerticalAngleFromEmptyChildPosition()
    {
        Vector3 targetDir = Child2Transform.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);
        RayRotate = Quaternion.AngleAxis(angle, transform.forward);//rotates about the Gameobject forward or X-axis
        return RayRotate;
    }

    /// <summary> GetHorizontalVectorDirection():
    /// returns the angle as a vector 3
    /// this method is used to modify the angle returned by the called method
    /// </summary>
    Vector3 GetHorizontalVectorDirection()
    {
        GetHorizontalAngleFromEmptyChildPosition();
        Vector3 modifiedAngle = RayRotate * transform.forward;
        // Debug.Log("Horizontal Angle: "+modifiedAngle);
        return modifiedAngle;
    }

    /// <summary> GetNegativeHorizontalVectorDirection():
    ///  multiplies the angle by negative 1 in the y-axis
    ///  returns the angle as a vector 3
    /// </summary>
    Vector3 GetNegativeHorizontalVectorDirection()
    {
         GetHorizontalAngleFromEmptyChildPosition(); 
         RayRotate.y = -RayRotate.y;
         Vector3 NegmodifiedAngle = RayRotate * transform.forward;
        // Debug.Log("Negative Horizontal Angle: " + NegmodifiedAngle);
         return NegmodifiedAngle;
        
    }

    /// <summary> GetVerticalVectorDirection():
    /// returns the angle as a vector 3
    /// this method is used to modify the angle returned by the called method
    /// </summary>
    Vector3 GetVerticalVectorDirection()
    {
        GetVerticalAngleFromEmptyChildPosition();
        RayRotate.x *= 2;
        Vector3 modifiedAngle = RayRotate * transform.forward;
        // Debug.Log("Vertical Angle: " + modifiedAngle);
        return modifiedAngle;
    }

    /// <summary> GetVerticalNegativeAngle():
    /// multiplies the angle by negative 1 in the x-axis
    /// returns the angle as a vector 3
    /// this method is used to modify the angle returned by the called method
    /// </summary>
    Vector3 GetNegativeVerticalVectorDirection()
    {
        GetVerticalAngleFromEmptyChildPosition();
        RayRotate.x = -RayRotate.x;
        Vector3 NegmodifiedAngle = RayRotate * transform.forward;
       // Debug.Log("Negative Vertical Angle: " + NegmodifiedAngle);
        return NegmodifiedAngle;
        
    }
}
