using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitControl : MonoBehaviour
{

	public GameObject _target; //the object its going to orbit
	public float _orbitSpeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //look at the target
        if(_target != null){
        	transform.LookAt(_target.transform); 
        	transform.RotateAround(_target.transform.position, Vector3.up, Time.deltaTime * _orbitSpeed);
        }
    }
}
