using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Are you debugging?")]
    //are we debugging?
    public bool _isDebugging = false;

    [Header("Reference to WorldManager")]
    public WorldManager _worldManager;

    [Header("Controls")]
    public float _handDistance = 15.0f; //distance between the 2 hands
    public float _elbowLToHead; //distance fron left elbow to head 
    public float _elbowRToHead;
    public float _handsAreUpInTheAirThreshold = 3.0f;
    public bool _handsAreUpInTheAir = false;
    private float _inputHorizontal;

    [Header("Reactive Shader")]
    public Renderer _liveScanRend;
    public Material _liveScanMat;

    [Header("Timers")]
    private float _participantHasExitedTimer = 0;
    public float _participantHasExitedThreshold = 5.0f;
    private float _handsInTheAirTimer = 0;
    public float _handsInTheAirHold = 1.5f;

    private bool _handsWereRaised = false;

    void FixedUpdate()
    {
        // inputs for the initialization phase
        if(_worldManager._isOnInitializing || _isDebugging)
            InitializationControls();

        //inputs for the live point cloud phase
        if(_worldManager._isOnLiveScanning)
            LivePointCloudControls();

        //inputs for the obituary phase
        if(_worldManager._isOnObituary)
            ObituaryControls();
    }

    //controls for the pre-rendered init animations (to initiate the experience)
    private void InitializationControls()
    {
        if (BodySourceView.bodyTracked)
        {   

            // fetch hand positions
            Vector3 handLeft = BodySourceView.jointObjs[7].position;
            Vector3 handRight = BodySourceView.jointObjs[11].position;

            //fetch head position
            Vector3 head = BodySourceView.jointObjs[3].position;

            //fetch elbow positions
            Vector3 elbowLeft = BodySourceView.jointObjs[5].position;
            Vector3 elbowRight = BodySourceView.jointObjs[9].position;

            //calculate hand distance
            _handDistance = Vector3.Distance(handLeft,handRight);

            //calculate the elbow distance to the head
            _elbowLToHead = Vector3.Distance(elbowLeft,head);
            _elbowRToHead = Vector3.Distance(elbowRight,head);

            //average the 2 distances
            float averagedDistance = (_elbowLToHead + _elbowRToHead)/2;

            //if the average is above the threhold, then the hands are up in the air
            // if(averagedDistance < _handsAreUpInTheAirThreshold){
            //     _handsAreUpInTheAir = true;
            // } else {
            //     _handsAreUpInTheAir = false;
            // }

            if(averagedDistance < _handsAreUpInTheAirThreshold && !_handsWereRaised){
                _handsAreUpInTheAir = true;
                _handsInTheAirTimer += Time.deltaTime;

                if(_handsInTheAirTimer > _handsInTheAirHold){
                    StartCoroutine(ResetHandsUp());
                    InitIsFinished();
                }
            } 

            // calc angle of hands
            float angle = Mathf.Atan2(handRight.y - handLeft.y, handRight.x - handLeft.x) * Mathf.Rad2Deg;

            // convert angle rotation to movement values
            _inputHorizontal = Mathf.Lerp(1.0f, -1.0f, Mathf.InverseLerp(-45.0f, 45.0f, angle));

            //debug
            if(_isDebugging){

                Debug.Log("hand distance" + _handDistance);
                Debug.Log("elbow L distance" + _elbowLToHead);
                Debug.Log("elbow R distance" + _elbowRToHead);
                Debug.Log("averaged distance" + averagedDistance);

                Debug.Log("Angle" + angle);
                Debug.Log("Input Horizontal" + _inputHorizontal);
            }
        }
    }

    //controls to navigate the point cloud
    private void LivePointCloudControls(){

        if(BodySourceView.bodyTracked){
             // fetch hand positions
            Vector3 handLeft = BodySourceView.jointObjs[7].position;
            Vector3 handRight = BodySourceView.jointObjs[11].position;

            //calculate hand distance
            _handDistance = Vector3.Distance(handLeft,handRight);

            // calc angle of hands
            float angle = Mathf.Atan2(handRight.y - handLeft.y, handRight.x - handLeft.x) * Mathf.Rad2Deg;

            // convert angle rotation to movement values
            _inputHorizontal = Mathf.Lerp(1.0f, -1.0f, Mathf.InverseLerp(-45.0f, 45.0f, angle));   

            //remap the input
            float reMappedHorizontal = ReMap(_inputHorizontal,1.0f,-1.0f,32.0f,4.0f);

            //assign it to shader        
            _liveScanMat.SetFloat("_Entropy", reMappedHorizontal);
        }

        //if the participant is not detected anymore, proceed to the obituary
        if (!BodySourceView.bodyTracked){
            //timer 
            _participantHasExitedTimer += Time.deltaTime;
            ResetBoneValues();
            //if the timer exceed the threshold, then the participant has left the scene and we can initiate the obituary
            if(_participantHasExitedTimer > _participantHasExitedThreshold){
                _participantHasExitedTimer = 0; 
                _worldManager._liveScanningIsFinished = true;   //TRIGGER THE OBITUARY
            }
        }
    }

    //controls to navigate the obituary
    private void ObituaryControls(){

    }

    private static float ReMap(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s-a1)*(b2-b1)/(a2-a1);
    }

    IEnumerator ResetHandsUp()
    {
        yield return new WaitForSeconds(5);
        _handsWereRaised = false;
        ResetBoneValues();
    }

    private void ResetBoneValues(){
        _handDistance = 7.0f; //distance between the 2 hands
        _elbowLToHead = 100; //distance fron left elbow to head 
        _elbowRToHead = 100;       
    }

    private void InitIsFinished(){
        _handsInTheAirTimer = 0;
        _worldManager._isOnInitializing = false;
        _worldManager._initializationIsFinished = true;

    }

    private void GetListOfBones(){
                //get list of bones
        for(int i = 0; i < BodySourceView.jointObjs.Length; i++){
            Debug.Log(BodySourceView.jointObjs[i].name);
        }
    }

}