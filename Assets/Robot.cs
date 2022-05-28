using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    GameObject RF;
    GameObject RB;
    GameObject RM;
    GameObject LF;
    GameObject LB;
    GameObject LM;
    bool isWalking = false;

    static public GameObject getChildGameObject(GameObject source,string withName) {
        Transform[] ts = source.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }
    
    IEnumerator stand(){
        StartCoroutine(RB.GetComponent<Kaki>().stand());
        StartCoroutine(RF.GetComponent<Kaki>().stand());
        StartCoroutine(RM.GetComponent<Kaki>().stand());
        StartCoroutine(LB.GetComponent<Kaki>().stand());
        StartCoroutine(LF.GetComponent<Kaki>().stand());
        StartCoroutine(LM.GetComponent<Kaki>().stand());
        yield return null;
    }

    IEnumerator standSpread(){
        StartCoroutine(RB.GetComponent<Kaki>().stand2());
        StartCoroutine(RF.GetComponent<Kaki>().stand2());
        StartCoroutine(RM.GetComponent<Kaki>().stand2());
        StartCoroutine(LB.GetComponent<Kaki>().stand2());
        StartCoroutine(LF.GetComponent<Kaki>().stand2());
        StartCoroutine(LM.GetComponent<Kaki>().stand2());
        yield return null;
    }

    IEnumerator init(){
        StartCoroutine(RB.GetComponent<Kaki>().init());
        StartCoroutine(RF.GetComponent<Kaki>().init());
        StartCoroutine(RM.GetComponent<Kaki>().init());
        StartCoroutine(LB.GetComponent<Kaki>().init());
        StartCoroutine(LF.GetComponent<Kaki>().init());
        StartCoroutine(LM.GetComponent<Kaki>().init());
        yield return null;
    }

    IEnumerator step(){
        StartCoroutine(RB.GetComponent<Kaki>().step());
        StartCoroutine(LM.GetComponent<Kaki>().step());
        StartCoroutine(RF.GetComponent<Kaki>().step());
        StartCoroutine(LB.GetComponent<Kaki>().step());
        StartCoroutine(RM.GetComponent<Kaki>().step());
        StartCoroutine(LF.GetComponent<Kaki>().step());
        yield return null;
    }

    IEnumerator step2(){
        StartCoroutine(RB.GetComponent<Kaki>().step2());
        StartCoroutine(LM.GetComponent<Kaki>().step2());
        StartCoroutine(RF.GetComponent<Kaki>().step2());
        StartCoroutine(LB.GetComponent<Kaki>().step2());
        StartCoroutine(RM.GetComponent<Kaki>().step2());
        StartCoroutine(LF.GetComponent<Kaki>().step2());
        yield return null;
    }

    IEnumerator rotation(){
        StartCoroutine(RB.GetComponent<Kaki>().rotation(30f, -1));
        StartCoroutine(LB.GetComponent<Kaki>().rotation(30f, -1));
        StartCoroutine(RF.GetComponent<Kaki>().rotation(30f, -1));
        StartCoroutine(LF.GetComponent<Kaki>().rotation(30f, -1));
        StartCoroutine(RM.GetComponent<Kaki>().rotation(30f, -1));
        StartCoroutine(LM.GetComponent<Kaki>().rotation(30f, -1));
        yield return null;
    }

    IEnumerator sideStep(){
        StartCoroutine(RB.GetComponent<Kaki>().sideStep());
        StartCoroutine(LM.GetComponent<Kaki>().sideStep());
        StartCoroutine(RF.GetComponent<Kaki>().sideStep());
        // yield return new WaitForSeconds(1.5f);
        StartCoroutine(LB.GetComponent<Kaki>().sideStep());
        StartCoroutine(RM.GetComponent<Kaki>().sideStep());
        StartCoroutine(LF.GetComponent<Kaki>().sideStep());
        yield return null;
    }

    void walk(){
        StartCoroutine(step());
    }

    // Start is called before the first frame update
    void Start()
    {
        RF = getChildGameObject(this.gameObject, "RF");
        RB = getChildGameObject(this.gameObject, "RB");
        RM = getChildGameObject(this.gameObject, "RM");
        LF = getChildGameObject(this.gameObject, "LF");
        LB = getChildGameObject(this.gameObject, "LB");
        LM = getChildGameObject(this.gameObject, "LM");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            StartCoroutine(stand());
        if (Input.GetKeyDown(KeyCode.Z))
            StartCoroutine(standSpread());
        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(init());
        if (Input.GetKeyDown(KeyCode.C))
            StartCoroutine(step());
        if (Input.GetKeyDown(KeyCode.V))
            StartCoroutine(step2());
        if (Input.GetKeyDown(KeyCode.B))
            StartCoroutine(rotation());
        
    }
}
