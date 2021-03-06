﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushDrag : MonoBehaviour
{
    public Rigidbody rig;
    public LayerMask layerMask;
    public float maxDistance;
    public float grabForce=200f;
    public SpringJoint holding;
    GameObject holdingGameObject;
    //public RigidbodyConstraints rigidbodyConstraints;
    PlayerMovement playerMovement;

    void Start()
    {
        holding=null;
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (!holding && holdingGameObject) {
            ResetHolding();
        }
        if (Input.GetButtonDown("Grab")){
            if (holding && !playerMovement.lookChanging){
                Destroy(holdingGameObject.GetComponent<SpringJoint>());
                ResetHolding();
            }
            else if (!holding){
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, layerMask)){
                    //Debug.DrawRay( transform.position, transform.right, Color.black, 1f);
                    holdingGameObject=hit.collider.gameObject;
                    holding=holdingGameObject.AddComponent<SpringJoint>();
                    holding.spring = 50;
                    holding.damper = 10;
                    //holding.maxDistance = 0.3f;
                    holding.tolerance = 2f;
                    holding.enableCollision = true;
                    holding.breakForce = grabForce;
                    holding.connectedBody = rig;
                    /*if (holdingGameObject.GetComponent<Rigidbody>()){
                        //m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;
                        rigidbodyConstraints = holdingGameObject.GetComponent<Rigidbody>().constraints;
                        //holdingGameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                    }*/

                    if (GetComponent<Jump>()) GetComponent<Jump>().enabled =false;
                }
            }
        }
    }

    public void ResetHolding(){
        //if (holdingGameObject.GetComponent<Rigidbody>()) holdingGameObject.GetComponent<Rigidbody>().constraints = rigidbodyConstraints;
        if (holdingGameObject.transform.position.z!=0) 
            holdingGameObject.transform.position = new Vector3(holdingGameObject.transform.position.x, holdingGameObject.transform.position.y, 0);
        holding=null;
        holdingGameObject=null;
        
        if (GetComponent<Jump>()) GetComponent<Jump>().enabled =true;
    }
}
