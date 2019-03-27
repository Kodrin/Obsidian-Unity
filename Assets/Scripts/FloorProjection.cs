using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class FloorProjection : MonoBehaviour
{
	[Header("References")]
	public GameObject _wallProjectionRef; // reference to the first projection

    [Header("Clips")]
    //list of video clips
    public VideoClip _idle;
    public VideoClip _initializing;
    public VideoClip _putYourHandsUp;
    public VideoClip _otherScenes;

    [Header("Video References")]
    //Video variable references
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;

    public bool _hasChangedVideo = false;
    public bool _animReset;

    // Update is called once per frame
    void Update()
    {
    	//if the wall projection is not active play the loop
    	if(_wallProjectionRef.activeSelf == false){
	    	//switch the video
			videoPlayer.clip = _otherScenes;
			videoPlayer.isLooping = true;
    	} else {
            InitializeObsidian();
        }
        
    }

   //Handles the idle/tracking/initiate of the experience
    public void InitializeObsidian(){

        //if your body is not tracked, then play the idle anima
        if(!BodySourceView.bodyTracked && !_hasChangedVideo){
            MimicVideo(_idle, true);
            _animReset = true;
        }

        if(BodySourceView.bodyTracked && !_hasChangedVideo){

            // BOOL TO PLAY VIDEO PLAYER ONE SHOT
            if(_animReset){
                _animReset = false;
                MimicVideo(_initializing, false);
            }

            // PUT YOUR HANDS UP ANIM
            if(!_hasChangedVideo)
                MimicVideo(_putYourHandsUp, true);
        }
    }

	public void MimicVideo(VideoClip nextVideo, bool Looping)
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

	//timer that waits for the video to finish 
    IEnumerator ResetChangeVideo(float lengthOfVideo)
    {
        yield return new WaitForSeconds(lengthOfVideo);
        _hasChangedVideo = false;
     }
}
