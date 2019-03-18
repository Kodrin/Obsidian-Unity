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
	
	[Header("Which Sequence are we on?")]
	public bool _isOnInitializing;
	public bool _isOnLiveScanning;
	public bool _isOnObituary;

    // Start is called before the first frame update
    void Start()
    {
        //check which of the scenes is active at the moment to set booleans
        CheckActiveScenes();
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

    private void CheckActiveScenes(){
    	//initialize the booleans
    	_isOnInitializing = _initialization.activeSelf;
    	_isOnLiveScanning = _liveScanning.activeSelf;
    	_isOnObituary = _Obituary.activeSelf;
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
