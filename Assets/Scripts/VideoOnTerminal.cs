using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoOnTerminal : MonoBehaviour {
    [Header("Am I debugging?")]
    public bool _isDebugging = false;

    [Header("References")]
    //player controller reference/world manager
    public FloorProjection _floorProjection;
    public PlayerController _playerController;
    public WorldManager _worldManager;

    [Header("Pose Param/transition to LS")]
    public float _initThreshold = 2.0f;
    public float _poseHoldThreshold = 2.5f;
    private float _poseHoldTimer = 0;

    public bool _hasChangedVideo = false;
    public bool _animReset;

    [Header("Main Audio Loop")]
    public GameObject _audioLoop;

    [Header("Clips")]
    //list of video clips
    public VideoClip _idle;
    public VideoClip _initializing;
    public VideoClip _putYourHandsUp;

    [Header("Video References")]
    //Video variable references
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;


    void Update () {
        InitializeObsidian();

        if(_isDebugging)
            Debugging();
    } 
    
    //timer that waits for the video to finish 
    IEnumerator ResetChangeVideo(float lengthOfVideo)
    {
        yield return new WaitForSeconds(lengthOfVideo);
        _hasChangedVideo = false;
     }

    //init obsidian animation transition
    private void ChangeVideo(VideoClip nextVideo, bool Looping)
    {
        //switch the video
        videoPlayer.clip = nextVideo;

        //do you want it to loop
        if(Looping){
            videoPlayer.isLooping = true;
        } else {
            videoPlayer.isLooping = false;
            videoPlayer.Play();
        }

        //has changed video
        _hasChangedVideo = true;
        
        //Wait for the video to finish before starting the next
        StartCoroutine(ResetChangeVideo((float)nextVideo.length));

     }

    //Handles the idle/tracking/initiate of the experience
    public void InitializeObsidian(){

        //if your body is not tracked, then play the idle anima
        if(!BodySourceView.bodyTracked && !_hasChangedVideo){
            ChangeVideo(_idle, true);
            // _floorProjection.MimicVideo(_floorProjection._idle, true);
            _animReset = true;
        }

        // TRANSITION TO LIVE SCANNING IF YOU PUT YOUR HANDS UP IN THE AIR
        // if(_playerController._handsAreUpInTheAir && BodySourceView.bodyTracked == true){
            
        //     //TIMER FOR HOW LONG IN THE AIR UR HANDS SHOULD BE
        //     _poseHoldTimer += Time.deltaTime;

        //     if(_poseHoldTimer > _poseHoldThreshold && !_hasChangedVideo){
        //         _poseHoldTimer = 0; //reset timer
        //         _worldManager._initializationIsFinished = true; //THE EXPERIENCE IS READY TO INITIALIZE
        //     }

        // }  

        if(BodySourceView.bodyTracked && !_hasChangedVideo){

            // BOOL TO PLAY VIDEO PLAYER ONE SHOT
            if(_animReset){
                _animReset = false;
                ChangeVideo(_initializing, false);
                _audioLoop.PlayDelayed(5); //start the audio loop
                // _floorProjection.MimicVideo(_floorProjection._initializing, false);
            }

            // PUT YOUR HANDS UP ANIM
            if(!_hasChangedVideo)
                ChangeVideo(_putYourHandsUp, true);
                // _floorProjection.MimicVideo(_floorProjection._putYourHandsUp, true);
        }
    }

    private void Debugging(){
        
    }
}