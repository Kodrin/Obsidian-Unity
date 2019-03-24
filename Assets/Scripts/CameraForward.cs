using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraForward : MonoBehaviour
{
	[Header("Camera Properties")]
	public float _panningSpeed;
	public Transform _originalPos;
	public Transform _target;
	public float _stopDistanceFromTarget;

    public float _transitionToNextSceneTimer = 0;
    public float _transitionWaitTime = 10.0f;

    [Header("Components")]
    public WorldManager _worldManager;

    // Update is called once per frame
    void Update()
    {

    	//PAN THE CAMERA
    	if(Vector3.Distance(_target.position, transform.position) > _stopDistanceFromTarget){
        	gameObject.transform.Translate(Vector3.forward * _panningSpeed);
    	} else {
            _transitionToNextSceneTimer += Time.deltaTime;
        }

        //TRANSITION TO NEXT SCENE
        if(_transitionToNextSceneTimer > _transitionWaitTime){
            _transitionToNextSceneTimer = 0;
            gameObject.transform.position = _originalPos.position; //reset position

            _worldManager._obituaryIsFinished = true;   //TRANSITION TO INIT PHASE
        }


    }
}
