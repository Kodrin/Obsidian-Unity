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
	public bool _resetPosition = false;


    // Start is called before the first frame update
    void Start()
    {
        //original position is equal to current position
        // _originalPos = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
    	//reset the position to original position
    	if(_resetPosition){
    		_resetPosition = false;
    		gameObject.transform.position = _originalPos.position;
    	}

    	//pan the camera
    	if(Vector3.Distance(_target.position, transform.position) > _stopDistanceFromTarget){
        	gameObject.transform.Translate(Vector3.forward * _panningSpeed);
    	}


    }
}
