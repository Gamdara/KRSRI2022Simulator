
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Kaki : MonoBehaviour
{
    GameObject Coxa ;
    GameObject Fermur ;
    GameObject Tibia ;
    Vector3 target;
    Vector3 target2;
    
    Vector3 current;
    [SerializeField] int posisi;
    [SerializeField] int side;
    [SerializeField] int letak;

    float fermurLength = 10f;
    float tibiaLength = 7f;
    
    float speed = 2f;
    
    IEnumerator InversKinematic(Vector3 v){
        float thetaC = Mathf.Atan2(v.z, v.x) * 180/Mathf.PI ;
        float l = Mathf.Sqrt(v.x * v.x + v.z * v.z);
        float thetaF1 = Mathf.Atan2(v.y, l) ;
        float a = Mathf.Sqrt(l * l + v.y * v.y);
        float thetaF2 = Mathf.Acos((Mathf.Pow(fermurLength, 2) +Mathf.Pow(a, 2) - Mathf.Pow(tibiaLength, 2) )/ (2 * a * fermurLength));
        float thetaF = (thetaF1 + thetaF2)* 180/Mathf.PI;
        float thetaT = (Mathf.Acos((Mathf.Pow(fermurLength, 2) +Mathf.Pow(tibiaLength, 2) - Mathf.Pow(a, 2) )/ (2 * tibiaLength * fermurLength)) * 180/Mathf.PI - 90);
        
        //normalisasi
        thetaC = 90 - thetaC;
        thetaT = thetaT - 90;
        
        Coroutine moveTibia = StartCoroutine(rotateDeg(Tibia, thetaT ));
        Coroutine moveCoxa = StartCoroutine(rotateDeg(Coxa, thetaC));
        Coroutine moveFermur = StartCoroutine(rotateDeg(Fermur, thetaF));
        this.current = v;
        yield return null;
    }

    Vector3 rotateMatrix(Vector3 v, float deg){
        float rad = deg * Mathf.Deg2Rad;
        float x = (Mathf.Cos(rad) * v.x) - (v.z * Mathf.Sin(rad));
        float z = (Mathf.Cos(rad) * v.z) + (v.x * Mathf.Sin(rad));
        v.x = x;
        v.z = z;
        return v;
    }

    public IEnumerator rotation(float deg, int dir){
        Vector3 newT = this.target2;
        Vector3 fw = rotateMatrix(newT, deg * dir);
        Vector3 bw = rotateMatrix(newT, deg *-1 * dir);
        
        yield return new WaitForSeconds(0.5f);
        
        if(this.side == 1){
            Queue<Vector3> steps = trajectory(fw,fw+new Vector3(0,4,newT.z-fw.z),bw + new Vector3(0,4,newT.z-fw.z),bw,0.1f);
            foreach(Vector3 step in steps){
                // step.x *= this.posisi;
                yield return StartCoroutine(InversKinematic(step));
                yield return new WaitForSeconds(0.15f);
            }

            Queue<Vector3> backSteps = trajectory(bw,bw,fw ,fw,0.1f);
            foreach(Vector3 bStep in backSteps){
                yield return StartCoroutine(InversKinematic(bStep));
                yield return new WaitForSeconds(0.15f);
            }
        }
        else{
            Queue<Vector3> backSteps = trajectory(bw,bw,fw ,fw,0.1f);
            foreach(Vector3 bStep in backSteps){
                yield return StartCoroutine(InversKinematic(bStep));
                yield return new WaitForSeconds(0.15f);
            }

            Queue<Vector3> steps = trajectory(fw,fw+new Vector3(0,4,newT.z-fw.z),bw + new Vector3(0,4,newT.z-fw.z),bw,0.1f);
            foreach(Vector3 step in steps){
                yield return StartCoroutine(InversKinematic(step));
                yield return new WaitForSeconds(0.15f);
            }
        }
    }

    static public GameObject getChildGameObject(GameObject source,string withName) {
        //Author: Isaac Dart, June-13.
        Transform[] ts = source.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }

    IEnumerator rotateDeg(GameObject obj, float dest){
        Quaternion target = Quaternion.Euler(0,dest,0);
        while(Mathf.Abs(obj.transform.localRotation.y - target.y) > 0.005){
            obj.transform.localRotation = Quaternion.RotateTowards(obj.transform.localRotation, target, 200 * Time.deltaTime);      
            yield return null;
        }   
    }

    public Queue<Vector3> trajectory(Vector3 P1,Vector3 P2,Vector3 P3,Vector3 P4, float t){
        Queue<Vector3> result = new Queue<Vector3>();
        float mult = t;
        result.Enqueue(P1);
        while(t<1){
            Vector3 Pt = Mathf.Pow(1-t,3) * P1 + 3*t*Mathf.Pow(1-t,2) * P2 + 3*t*t*(1-t) * P3 + Mathf.Pow(t,3) * P4;
            Debug.Log(Pt);
            result.Enqueue(Pt);
            t += mult;
        }
        result.Enqueue(P4);
        return result;
    }

    public IEnumerator step(){
        if(this.side == 1){
            Queue<Vector3> steps = trajectory(new Vector3(-4,0,0),new Vector3(-4,4,0),new Vector3(4,4,0),new Vector3(4,0,0),0.1f);
            foreach(Vector3 step in steps){
                yield return StartCoroutine(InversKinematic(target + Vector3.Scale(step, new Vector3(this.posisi,1,1))));
                yield return new WaitForSeconds(0.15f);
            }
            Queue<Vector3> backSteps = trajectory(new Vector3(4,0,0),new Vector3(4,0,0),new Vector3(-4,0,0),new Vector3(-4,0,0),0.1f);
            foreach(Vector3 bStep in backSteps){
                yield return StartCoroutine(InversKinematic(target + Vector3.Scale(bStep, new Vector3(this.posisi,1,1))));
                yield return new WaitForSeconds(0.15f);
            }
        }
        else{
            Queue<Vector3> backSteps = trajectory(new Vector3(4,0,0),new Vector3(4,0,0),new Vector3(-4,0,0),new Vector3(-4,0,0),0.1f);
            foreach(Vector3 bStep in backSteps){
                yield return StartCoroutine(InversKinematic(target + Vector3.Scale(bStep, new Vector3(this.posisi,1,1))));
                yield return new WaitForSeconds(0.15f);
            }
            Queue<Vector3> steps = trajectory(new Vector3(-4,0,0),new Vector3(-4,4,0),new Vector3(4,4,0),new Vector3(4,0,0),0.1f);
            foreach(Vector3 step in steps){
                yield return StartCoroutine(InversKinematic(target + Vector3.Scale(step, new Vector3(this.posisi,1,1))));
                yield return new WaitForSeconds(0.15f);
            }
        }
    }

    public IEnumerator step2(){
        if(this.side == 1){
            Queue<Vector3> steps = trajectory(new Vector3(-3,0,0),new Vector3(-3,3,0),new Vector3(3,3,0),new Vector3(3,0,0),0.1f);
            foreach(Vector3 step in steps){
                yield return StartCoroutine(InversKinematic(target2 + Vector3.Scale(step, new Vector3(this.posisi,1,1))));
                yield return new WaitForSeconds(0.15f);
            }
            yield return new WaitForSeconds(0.15f);
            Queue<Vector3> backSteps = trajectory(new Vector3(3,0,0),new Vector3(3,0,0),new Vector3(-3,0,0),new Vector3(-3,0,0),0.1f);
            foreach(Vector3 bStep in backSteps){
                yield return StartCoroutine(InversKinematic(target2 + Vector3.Scale(bStep, new Vector3(this.posisi,1,1))));
                yield return new WaitForSeconds(0.15f);
            }
        }
        else{
            Queue<Vector3> backSteps = trajectory(new Vector3(3,0,0),new Vector3(3,0,0),new Vector3(-3,0,0),new Vector3(-3,0,0),0.1f);
            foreach(Vector3 bStep in backSteps){
                yield return StartCoroutine(InversKinematic(target2 + Vector3.Scale(bStep, new Vector3(this.posisi,1,1))));
                yield return new WaitForSeconds(0.15f);
            }
            yield return new WaitForSeconds(0.15f);
            Queue<Vector3> steps = trajectory(new Vector3(-3,0,0),new Vector3(-3,3,0),new Vector3(3,3,0),new Vector3(3,0,0),0.1f);
            foreach(Vector3 step in steps){
                yield return StartCoroutine(InversKinematic(target2 + Vector3.Scale(step, new Vector3(this.posisi,1,1))));
                yield return new WaitForSeconds(0.15f);
            }
        }
    }

    // yield return StartCoroutine(InversKinematic(target + new Vector3(-2* this.posisi,0,0) ));
    public IEnumerator sideStep(){
        if((this.side == 1 && this.posisi == 1) || (this.posisi == 1 && this.side == -1)){
            Queue<Vector3> steps = trajectory(new Vector3(0,0,-2),new Vector3(0,4,-2),new Vector3(0,4,2),new Vector3(0,0,2),0.1f);
            foreach(Vector3 step in steps){
                yield return StartCoroutine(InversKinematic(target + Vector3.Scale(step, new Vector3(this.side * this.posisi,1,1))));
                yield return new WaitForSeconds(0.2f);
            }
            Queue<Vector3> backSteps = trajectory(new Vector3(0,0,2),new Vector3(0,0,2),new Vector3(0,0,-2),new Vector3(0,0,-2),0.1f);
            foreach(Vector3 bStep in backSteps){
                yield return StartCoroutine(InversKinematic(target + Vector3.Scale(bStep, new Vector3(this.side* this.posisi,1,1))));
                yield return new WaitForSeconds(0.2f);
            }
        }
        else{
            Queue<Vector3> backSteps = trajectory(new Vector3(0,0,2),new Vector3(0,0,2),new Vector3(0,0,-2),new Vector3(0,0,-2),0.1f);
            foreach(Vector3 bStep in backSteps){
                yield return StartCoroutine(InversKinematic(target + Vector3.Scale(bStep, new Vector3(this.side* this.posisi,1,1))));
                yield return new WaitForSeconds(0.2f);
            }
            Queue<Vector3> steps = trajectory(new Vector3(0,0,-2),new Vector3(0,4,-2),new Vector3(0,4,2),new Vector3(0,0,2),0.1f);
            foreach(Vector3 step in steps){
                yield return StartCoroutine(InversKinematic(target + Vector3.Scale(step, new Vector3(this.side* this.posisi,1,1))));
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    public IEnumerator stand(){
        yield return StartCoroutine(InversKinematic(target ));
        yield return null;
    }

    public IEnumerator stand2(){
        yield return StartCoroutine(InversKinematic(target2 ));
        yield return null;
    }

    public IEnumerator init(){
        Coroutine moveTibia = StartCoroutine(rotateDeg(Tibia, 0));
        Coroutine moveCoxa = StartCoroutine(rotateDeg(Coxa, 0));
        Coroutine moveFermur = StartCoroutine(rotateDeg(Fermur, 0));
        yield return null;
    }

    void Start()
    {
        Coxa = getChildGameObject(this.gameObject,"Coxa");
        Fermur = getChildGameObject(this.gameObject,"Fermur");
        Tibia = getChildGameObject(this.gameObject,"Tibia");
        target = new Vector3(0, -8, 10);
        
        if(letak == 1)
            target2 = rotateMatrix(this.target, -45 * this.posisi);
        else if(letak == 3)
            target2 = rotateMatrix(this.target, 45 * this.posisi);
        else 
            target2 = target;
    }

    void Update()
    {   
        
    }
}
