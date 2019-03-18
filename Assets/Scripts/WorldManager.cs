using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
	/*
	WORLD MANAGER SCRIPT: 
	This script is used to dictate the flow of the experience and narrative
	*/
	[Header("Are you debugging?")]
	public bool _isDebugging = false;

	[Header("List of sequences/cameras")]
	public GameObject _initialization;
	public Camera _initCamera;
	public GameObject _liveScanning;
	public Camera _liveScanningCamera;
	public GameObject _Obituary;
	public Camera _obituaryCamera;
	
	// [Header("Resetting the experience if no one is there")]
	//to be determined
	// public float 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //debug
        if(_isDebugging)
        	Debugging();
    }

    //when you are debugging
    private void Debugging(){

    }

    //Transition to the next phase
    public void TransitionToNextPhase(GameObject currentScene, Camera currentCamera, GameObject nextScene, Camera nextCamera){
    	//disable current scene and camera
    	currentScene.SetActive(false);
    	currentCamera.enabled = false;

    	//enable the next scene and camera
    	nextScene.SetActive(true);
    	nextCamera.enabled = true;
    }

}
