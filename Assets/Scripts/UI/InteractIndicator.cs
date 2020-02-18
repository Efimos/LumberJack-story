﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractIndicator : MonoBehaviour
{
    public bool active = true;
    public bool showHint = true;
    public GameObject hint;
    public float timeBeforeHint = 5f;
    Vector3 pos;
    public string keyboardString = "E";
    public string gamepadString = "button2";

    void OnEnable()
    {
        hint.SetActive(false);
        pos = transform.position;
        if (IsActive()){
            StartCoroutine(HintDisplay());
        }
    }

    void OnDisable(){
        StopCoroutine(HintDisplay());
    }

    void Update(){
        if (transform.position != pos){
            StopCoroutine(HintDisplay());
            StartCoroutine(HintDisplay());
            pos = transform.position;
        }
    }

    IEnumerator HintDisplay(){
        if (showHint){
            //timer = 0;
            hint.SetActive(false);
        
            yield return new WaitForSeconds(timeBeforeHint);

            DisplayHint();
        }
    }

    public void DisplayHint(bool b = true){
        hint.SetActive(b);
        if (b){
            string[] temp = Input.GetJoystickNames();
            if(temp.Length > 0){
                for(int i =0; i < temp.Length; ++i){
                    if(!string.IsNullOrEmpty(temp[i])){
                        hint.GetComponentInChildren<TMPro.TextMeshPro>().text = gamepadString;
                    }
                    else {
                        hint.GetComponentInChildren<TMPro.TextMeshPro>().text = keyboardString;
                    }
                }
            }
        }
    }

    public bool IsActive(){
        if (!active) {
            gameObject.SetActive(false);
            return false;
        }
        return true;
    }
}
