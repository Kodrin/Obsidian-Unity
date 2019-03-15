using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSubjectName : MonoBehaviour
{
	//component 
	TextMesh _txtMesh;

    // Start is called before the first frame update
    void Start()
    {
    	//fetch the text mesh
    	_txtMesh = gameObject.GetComponent<TextMesh>();
		//fetch the parent
    	_txtMesh.text = gameObject.transform.parent.name;
    	//debug
    	// Debug.Log(gameObject.transform.parent.name);
    }

}
