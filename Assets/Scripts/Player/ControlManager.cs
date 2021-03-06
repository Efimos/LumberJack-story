﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public Transform human;
    public Transform dog;
    public string[] HumanActive;
    public string[] DogActive;
    public string[] HumanInactive;
    public string[] DogInactive;
    public List<Behaviour> HumanBehavioursActive;
    public List<Behaviour> DogBehavioursActive;
    public List<Behaviour> HumanBehavioursInactive;
    public List<Behaviour> DogBehavioursInactive;
    public Transform PlayerPivot;
    public static bool onHuman;
    public static bool onDog;
    bool[] HumanBehavioursActiveWasActive;
    bool[] DogBehavioursActiveWasActive;
    public bool acceptInputs;
    void Start()
    {
        SetupPlayers();
        acceptInputs = true;
    }

    public void SetupPlayers(){
        onHuman = true;
        onDog = false;
        SetPlayers();
        SetBehavioursActive(0);
        for (int i = 0; i < DogBehavioursActiveWasActive.Length; i++)
        {
            DogBehavioursActiveWasActive[i] = true;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("SwitchCharacter") && acceptInputs){
            if (onHuman){
                onHuman = false;
                onDog = true;
                SetBehavioursActive(0);
            }
            else {
                onHuman = true;
                onDog = false;
                SetBehavioursActive(1);
            }
            
            for (int i = 0; i < HumanBehavioursActive.Count; i++)
            {
                HumanBehavioursActive[i].enabled = onHuman && HumanBehavioursActiveWasActive[i];
            }
            for (int i = 0; i < DogBehavioursActive.Count; i++)
            {
                DogBehavioursActive[i].enabled = onDog && DogBehavioursActiveWasActive[i];
            }
            /*
            foreach (Behaviour b in HumanBehavioursActive)
            {
                b.enabled = onHuman;
            }
            foreach (Behaviour b in DogBehavioursActive)
            {
                b.enabled = onDog;
            }
            */
            foreach (Behaviour b in HumanBehavioursInactive)
            {
                b.enabled = !onHuman;
            }
            foreach (Behaviour b in DogBehavioursInactive)
            {
                b.enabled = !onDog;
            }
            SetPlayerCharOnStandby(human, !onHuman);
            SetPlayerCharOnStandby(dog, !onDog);

            if (PlayerPivot){
                if (PlayerPivot.GetComponent<CameraFollowPivot>()){
                    Transform t = null;
                    if (onHuman) t = human;
                    else if (onDog) t = dog;
                    PlayerPivot.GetComponent<CameraFollowPivot>().ChangeTarget(t);
                }
            }
        }
    }

    void SetBehavioursActive(int index){
        if (index==0){
            for (int i = 0; i < HumanBehavioursActiveWasActive.Length; i++)
            {
                HumanBehavioursActiveWasActive[i] = HumanBehavioursActive[i].enabled;
            }
        }
        else if (index==1){
            for (int i = 0; i < DogBehavioursActiveWasActive.Length; i++)
            {
                DogBehavioursActiveWasActive[i] = DogBehavioursActive[i].enabled;
            }
        }
    }

    void SetPlayerCharOnStandby(Transform character, bool onStandby=true){
        character.position = new Vector3(character.position.x, character.position.y, (onStandby? LevelSettings.secondZLine : LevelSettings.mainZLine));
    }

    public void SetPlayers(){
        human = GameObject.FindWithTag("PlayerHuman").transform;
        dog = GameObject.FindWithTag("PlayerDog").transform;

        HumanBehavioursActive = SetupBehaviours(human, HumanActive);
        HumanBehavioursInactive = SetupBehaviours(human, HumanInactive);
        DogBehavioursActive = SetupBehaviours(dog, DogActive);
        DogBehavioursInactive = SetupBehaviours(dog, DogInactive);

        HumanBehavioursActiveWasActive = new bool[HumanBehavioursActive.Count];
        DogBehavioursActiveWasActive = new bool[DogBehavioursActive.Count];
    }

    public void PlayersActive(bool active){
        if (!active){
            if (onHuman) SetBehavioursActive(0);
            if (onDog) SetBehavioursActive(1);
        }
        for (int i = 0; i < HumanBehavioursActive.Count; i++)
        {
            HumanBehavioursActive[i].enabled = active && onHuman && HumanBehavioursActiveWasActive[i];
        }
        for (int i = 0; i < DogBehavioursActive.Count; i++)
        {
            DogBehavioursActive[i].enabled = active && onDog && DogBehavioursActiveWasActive[i];
        }
        foreach (Behaviour b in HumanBehavioursInactive)
        {
            b.enabled = active && !onHuman;
        }
        foreach (Behaviour b in DogBehavioursInactive)
        {
            b.enabled = active && !onDog;
        }
        
        if (PlayerPivot){
            if (PlayerPivot.GetComponent<CameraFollowPivot>()){
                Transform t = null;
                if (onHuman) t = human;
                else if (onDog) t = dog;
                PlayerPivot.GetComponent<CameraFollowPivot>().ChangeTarget(t);
            }
        }
    }

    List<Behaviour> SetupBehaviours(Transform obj, string[] s){
        List<Behaviour> l = new List<Behaviour>{};
        //Debug.Log("-----"+s.ToString()+"-----");
        foreach (Component c in obj.GetComponentsInChildren<Component>())
        {
            //Debug.Log(c.GetType());
            if (StringMatch(c.GetType().ToString(), s)){
                //Debug.Log("Found " + c.GetType());
                Behaviour be = (Behaviour)c;
                //Debug.Log("Behaviour_"+be.GetType());
                l.Add(be);
            }
        }
        return l;
    }

    bool StringMatch (string s, string[] list){
        foreach (string l in list)
        {
            if (s==l) return true;
        }
        return false;
    }
}
