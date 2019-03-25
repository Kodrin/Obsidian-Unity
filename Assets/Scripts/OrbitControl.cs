using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitControl : MonoBehaviour
{

    [Header("Camera Properties")]
	public GameObject _target; //the object its going to orbit
	public float _orbitSpeed = 2.0f;
    public float _offset;

    [Header("Components")]
    public PlayerController _playerController;

    // Update is called once per frame
    void Update()
    {
        //remap the hand distance
        float remappedHands= map(_playerController._handDistance, 0.0f, 10.0f, -0.1f,0.1f);

        //constrain the camera

        //look at the target
    	transform.LookAt(_target.transform); 
    	transform.RotateAround(_target.transform.position, Vector3.up, Time.deltaTime * _orbitSpeed);
        transform.Translate(Vector3.forward * remappedHands);
    }

    private float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s-a1)*(b2-b1)/(a2-a1);
    }
}
