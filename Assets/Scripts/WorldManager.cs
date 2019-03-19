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

	[Header("Is the sequence finished? True for yes")]
	public bool _initializationIsFinished = false;
	public bool _liveScanningIsFinished = false;
	public bool _obituaryIsFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        //check which of the scenes is active at the moment to set booleans
        CheckActiveScenes();
    }

    // Update is called once per frame
    void Update()
    {
        //handle the scene transitions
        SceneManager();

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

    private void SceneManager(){
    	//if this scene is finished, proceed to the next one
    	if(_initializationIsFinished){
    		_initializationIsFinished = false; //reset the bool TRIGGER IS IN THE VIDEONTERMINAL.CS
    		TransitionToNextPhase(_initialization, _initCamera, _liveScanning, _liveScanningCamera);
    	}

    	//if this scene is finished, proceed to the next one
    	if(_liveScanningIsFinished){

    	}

    	//if this scene is finished, proceed to the next one
    	if(_obituaryIsFinished){

    	}

    }

    //Transition to the next phase
    public void TransitionToNextPhase(GameObject currentScene, Camera currentCamera, GameObject nextScene, Camera nextCamera){

    	//enable the next scene and camera
    	nextScene.SetActive(true);
    	nextCamera.enabled = true;

    	//disable current scene and camera
    	currentScene.SetActive(false);
    	currentCamera.enabled = false;

    }

}
