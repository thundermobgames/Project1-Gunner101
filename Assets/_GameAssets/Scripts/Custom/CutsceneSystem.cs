using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CutsceneSystem : MonoBehaviour
{

    [Serializable]
    public class Cutscene
    {
        public string sceneName; 
        public Transform camRig;
        public Camera camera;
        public Transform startPoint;
        public Transform endPoint;
        public Transform lookAtTarget;
        public float moveSpeed;
        [HideInInspector] public int sceneStatus;
        [HideInInspector] public float moveRatio;
        [HideInInspector] public int index;

        public void initCutscene()
        {
            camRig.gameObject.SetActive(false);
            startPoint.GetComponent<Renderer>().enabled = false;
            endPoint.GetComponent<Renderer>().enabled = false;
        }
    }

    [SerializeField] List<Cutscene> cutscenes;
    bool enableOutro;

    public delegate void CutsceneSystemAction(Cutscene cutscene);
    public static event CutsceneSystemAction OnCutsceneStarted;
    public static event CutsceneSystemAction OnCutsceneRunning;
    public static event CutsceneSystemAction OnCutsceneStopped;

    private void Awake() 
    {
        foreach (Cutscene scene in cutscenes) { scene.initCutscene(); }        
    }

    private void OnEnable() {
        
    }

    private void OnDisable() {
        
    }

    private void Start() {
        
    }

    private void Update() 
    {
        if (cutscenes[0].sceneStatus != 2) { playCutscene(0);}
        else if (cutscenes[1].sceneStatus != 2) { playCutscene(1);}
        if (enableOutro) { 
            if (cutscenes[2].sceneStatus != 2) { playCutscene(2);} 
            else if (cutscenes[3].sceneStatus != 2) { playCutscene(3);} 
        }
    }

    void playCutscene(int index)
    {
        Cutscene currCutscene = cutscenes[index];
        if(currCutscene.sceneStatus==0)
        {
            currCutscene.sceneStatus = 1;
            currCutscene.camRig.gameObject.SetActive(true);
            currCutscene.index = index;
            if (OnCutsceneStarted != null) { OnCutsceneStarted.Invoke(currCutscene); }
        }
        else if (currCutscene.sceneStatus == 1)
        {
            if(currCutscene.moveRatio<0.99f)
            {
                currCutscene.moveRatio += currCutscene.moveSpeed * Time.deltaTime;
                currCutscene.camRig.position = Vector3.Lerp(currCutscene.startPoint.position, 
                                currCutscene.endPoint.position, currCutscene.moveRatio);
                currCutscene.camera.transform.LookAt(currCutscene.lookAtTarget);                
                if (OnCutsceneRunning != null) { OnCutsceneRunning.Invoke(currCutscene); }
                if(currCutscene.moveRatio>=0.99f)
                {
                    currCutscene.sceneStatus = 2;
                    currCutscene.camRig.gameObject.SetActive(false);
                    if (OnCutsceneStopped != null) { OnCutsceneStopped.Invoke(currCutscene); }
                }
            }            
        }
    }

    void CurseEye_OnCurseEyeDestroyed()
    {
        enableOutro = true;
    }

}
