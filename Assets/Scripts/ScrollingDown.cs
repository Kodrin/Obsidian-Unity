using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingDown : MonoBehaviour
{
	[Header("Scrolling Properties")]
	public float _panningSpeed;
	public Transform _originalZPosition;

	void Start()
	{
		_originalZPosition = GameObject.FindGameObjectWithTag("OriginalMuralPosition").transform;
	}
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(new Vector3(0,-1,0) * _panningSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x,_originalZPosition.position.y,gameObject.transform.position.z);
    	Debug.Log(other.name);
    }

}
