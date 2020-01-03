﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Call : MonoBehaviour
{
    public Transform dog;
    bool dogfollow=true;
    NavMeshMovement dogNavMesh;
    NavMeshAgent dogAgent;
    // Start is called before the first frame update
    void Start()
    {
        dogNavMesh = dog.GetComponent<NavMeshMovement>();
        dogAgent = dog.GetComponent<NavMeshAgent>();
        dogfollow=true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Call")){
            dogfollow = !dogfollow;
            dogNavMesh.enabled = dogfollow;
            dogAgent.enabled = dogfollow;
        }
    }
}