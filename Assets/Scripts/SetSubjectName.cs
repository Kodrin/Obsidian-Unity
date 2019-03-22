using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSubjectName : MonoBehaviour
{
	//component 
    [Header("Naming Properties")]
    // public string _subjectName; 
	public TextMesh _txtMesh;

    // Start is called before the first frame update
    void Start()
    {
		//fetch the parent
    	// _txtMesh.text = _subjectName;
    	//debug
    	// Debug.Log(gameObject.transform.parent.name);
    }

    public void SetName(string _subjectName){
       _txtMesh.text = _subjectName; 
    }

}
