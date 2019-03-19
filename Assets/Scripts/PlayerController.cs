using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Are you debugging?")]
    //are we debugging?
    public bool _isDebugging = false;

    [Header("Reference to WorldManager")]
    public WorldManager _worldManager;

    [Header("Initialization Controls")]
    public float _handDistance = 15.0f;
    private float _inputHorizontal;

    void Start()
    {

    }

    void Update()
    {
        // inputs for the initialization phase
        if(_worldManager._isOnInitializing)
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

            //calculate hand distance
            _handDistance = Vector3.Distance(handLeft,handRight);

            // calc angle of hands
            float angle = Mathf.Atan2(handRight.y - handLeft.y, handRight.x - handLeft.x) * Mathf.Rad2Deg;

            // convert angle rotation to movement values
            _inputHorizontal = Mathf.Lerp(1.0f, -1.0f, Mathf.InverseLerp(-45.0f, 45.0f, angle));

            //debug
            if(_isDebugging){
                Debug.Log("hand distance" + _handDistance);
                Debug.Log("Angle" + angle);
                Debug.Log("Input Horizontal" + _inputHorizontal);
            }
        }
    }

    //controls to navigate the point cloud
    private void LivePointCloudControls(){

    }

    //controls to navigate the obituary
    private void ObituaryControls(){

    }

}