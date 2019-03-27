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
	public GameObject _obituary;
	public Camera _obituaryCamera;
	
	[Header("Which Sequence are we on?")]
	public bool _isOnInitializing;
	public bool _isOnLiveScanning;
	public bool _isOnObituary;

	[Header("Is the sequence finished? True for yes")]
	public bool _initializationIsFinished = false;
	public bool _liveScanningIsFinished = false;
	public bool _obituaryIsFinished = false;

    [Header("Participant Point Cloud")]
    public Texture2D _participantPointCloud;
    public PointCloudDataManager _pointCloudCapturePrefab;
    public Renderer _obituaryParticipantPointCloud;

    [Header("Data Loader")]
    public LoadData _loadData; 

    // Start is called before the first frame update
    void Start()
    {
        // //check which of the scenes is active at the moment to set booleans
        // CheckActiveScenes();
    }

    // Update is called once per frame
    void Update()
    {
        //check which of the scenes is active at the moment to set booleans
        CheckActiveScenes();

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
    	_isOnObituary = _obituary.activeSelf;
    }

    private void SceneManager(){
    	//if this scene is finished, proceed to the next one
    	if(_initializationIsFinished){
    		_initializationIsFinished = false; //reset the bool TRIGGER IS IN THE VIDEONTERMINAL.CS
            _pointCloudCapturePrefab.SaveParticipant(); //store the participant's point cloud data for later use
            _loadData.ShiftData(_participantPointCloud); //shift the data and assign participant point cloud
            _obituaryParticipantPointCloud.material.SetTexture("_MainTex", _participantPointCloud);//assign that point cloud to the obituary space
    		TransitionToNextPhase(_initialization, _initCamera, _liveScanning, _liveScanningCamera);
    	}

    	//if this scene is finished, proceed to the next one
    	if(_liveScanningIsFinished){
            _liveScanningIsFinished = false;
            TransitionToNextPhase(_liveScanning, _liveScanningCamera, _obituary, _obituaryCamera); // TRIGGER IS IN THE PLAYERCONTROLLER.CS
    	}

    	//if this scene is finished, proceed to the next one
    	if(_obituaryIsFinished){
            _obituaryIsFinished = false;
            TransitionToNextPhase( _obituary, _obituaryCamera,_initialization, _initCamera);
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
