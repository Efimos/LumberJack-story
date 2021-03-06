﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public float spinForce=1f;
    public float maxAngVel=1f;
    Rigidbody rig;
    public bool flyingWithSpin;
    public float rayDistance=3f;
    public float slerpMult=100f;
    float angleV;
    public int bladeTouchingCount;
    Jump jump;

    void Start()
    {
        flyingWithSpin=false;
        rig = GetComponent<Rigidbody>();
        bladeTouchingCount = 0;
    }

    void FixedUpdate(){
        if (flyingWithSpin) {
            rig.AddTorque(spinForce * transform.right);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, rig.velocity, out hit, rayDistance)){
                //Debug.DrawRay(transform.position, rig.velocity, Color.red);
                angleV = Mathf.Atan2(rig.velocity.y, rig.velocity.x) * Mathf.Rad2Deg;
                //Debug.Log(angleV);
                Transform t = hit.transform;
                if ( ! (angleV > -135 && angleV < -45)){
                    //Debug.Log("ANGLE CORRECT");
                    if (!t.GetComponent<InteractControl>()){
                        Quaternion rot = Quaternion.Euler(new Vector3(transform.rotation.x, 90 * Mathf.Sign(rig.velocity.x), transform.rotation.z));
                        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * spinForce * slerpMult);
                    }
                }
            }
        }
    }

    void OnThrow(MessageArgs msg){
        msg.received =true;
        InteractControl i = msg.sender.GetComponent<InteractControl>();
        if (i){
            if (i.IsHuman){
                i.ReleaseHand(true, false, true);
                rig.AddForce(i.throwingAngle * i.throwPower * i.throwForce, ForceMode.Impulse);
                rig.maxAngularVelocity = maxAngVel;
                flyingWithSpin=true;
            }
        }
        //rig.AddTorque(spinForce * transform.right, ForceMode.Acceleration);
    }

    void OnGrab(MessageArgs msg){
        msg.received = true;
        Transform t = msg.sender;
        if (t.GetComponent<InteractControl>()){
            jump = t.GetComponent<Jump>();
            if (jump){
                if (jump.isActiveAndEnabled){
                    //Debug.Log("has jump");
                    if (rig.isKinematic && !jump.IsGrounded() && Input.GetButton("Jump")){
                        AxeJump();    
                    }
                }
            }
            if (rig.isKinematic) rig.isKinematic = false;
        }
    }

    
    void OnReleaseInventory(MessageArgs msg){
        Debug.Log("sended axe");
        Inventory i = msg.sender.GetComponent<Inventory>();
        if (i) {
            if (i.isActiveAndEnabled){
                i.SaveToInventory(transform, -1, true);
                msg.received = true;
            }
        }
    }
    

    void OnCollisionEnter(Collision collision)
    {
        if (flyingWithSpin && !collision.gameObject.GetComponent<InteractControl>()){
            flyingWithSpin = false;
            if ( ! (angleV > -135 && angleV < -45) && bladeTouchingCount>0){
                rig.isKinematic = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        bladeTouchingCount++;
    }
    void OnTriggerExit(Collider other)
    {
        bladeTouchingCount--;
    }

    void AxeJump(){
        //Debug.Log("axe jump");
        Vector3 v = jump.gameObject.GetComponent<Rigidbody>().velocity;
        v = new Vector3(v.x, 0, v.z);
        jump.gameObject.GetComponent<Rigidbody>().velocity = v;
        jump.JumpOnDemand = true;
        jump.onJumpHold = true;
        StartCoroutine(SetJumpPressedForSeconds(1));
    } 

    IEnumerator SetJumpPressedForSeconds(float seconds){
        yield return new WaitForSeconds(seconds);
        if (jump) jump.onJumpHold = false;
    }
}
