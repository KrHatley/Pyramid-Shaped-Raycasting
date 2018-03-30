using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour,IinteractableOptions {

    /// <summary>
    /// Test class, attached to basic cube with a rigidbody component.
    /// Uses the IInteractable interface to be activated,
    /// this is a generic idea of how gameobject would have their functions called
    /// </summary>
    private bool activate;
    public void DisplayInteractOptions()//Interface implementation
    {
        activate = !activate;//Local variable to activate any state changes in the gameObject this is attached to
        /// This is where you could possibly call unique class functions 
    }

    // Use this for initialization
    void Start () {
        activate = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (activate)
        {
            Debug.Log("Successful Display of interaction Options");
            activate = !activate;
        }
	}
}
