using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoOnTerminal : MonoBehaviour {
    
    //skeleton reference
    public PlayerController _skeleton;
    public WorldManager _worldManager;
    public float _initThreshold = 2.0f;
    public float _handTimer = 0;
    private bool _hasChangedVideo = false;
    private bool _isReadyToInit = false;

    //list of video clips
    public VideoClip _idle;
    public VideoClip _tracking;
    public VideoClip _initializing;

    //Video variable references
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;

    // Use this for initialization
    void Start () {
        StartCoroutine(PlayVideo());

    }

    void Update () {
        InitializeObsidian();
    } 
    
    IEnumerator PlayVideo()
    {
        videoPlayer.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared)
        {
            yield return waitForSeconds;
            break;
        }
        // rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
        audioSource.Play();
     }

    //timer that waits for the video to finish 
    IEnumerator ResetChangeVideo(float lengthOfVideo)
    {
        yield return new WaitForSeconds(lengthOfVideo);
        _hasChangedVideo = false;

        //if the experience is ready to initialize, init it!
        if(_isReadyToInit){
            _isReadyToInit = false; //reset bool
            _worldManager._initializationIsFinished = true; //INITIALIZE OBSIDIAN
        }
     }

    //init obsidian animation transition
    public void ChangeVideo(VideoClip nextVideo)
    {
        videoPlayer.clip = nextVideo;
        videoPlayer.Play();
        audioSource.Play();

        //has changed video
        _hasChangedVideo = true;
        
        //Wait for the video to finish before starting the next
        StartCoroutine(ResetChangeVideo((float)nextVideo.length));

     }

    //Handles the idle/tracking/initiate of the experience
    public void InitializeObsidian(){
        //if your body is not tracked, then play the idle anima
        if(!BodySourceView.bodyTracked && !_hasChangedVideo)
            ChangeVideo(_idle);

        //plays the initializes anim after the partipant holds his hands together for 5 seconds
        if(_skeleton._handDistance < _initThreshold && BodySourceView.bodyTracked == true){
            //increase the timer
            _handTimer += Time.deltaTime;

            if(_handTimer > 5.0f && !_hasChangedVideo){
                ChangeVideo(_initializing);
                _handTimer = 0; //reset timer
                _isReadyToInit = true; //THE EXPERIENCE IS READY TO INITIALIZE
            }

        } else if(BodySourceView.bodyTracked && !_hasChangedVideo){
            ChangeVideo(_tracking);
        }
    }
}