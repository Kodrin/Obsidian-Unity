using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HashGenerator : MonoBehaviour
{
	//references https://answers.unity.com/questions/800488/random-char-a-to-z.html
	[Header("Hash Parameters")]
	public Text _referenceToSubjectName;
	public string _prefix = "0";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space"))
        	_referenceToSubjectName.text = _referenceToSubjectName.text+ "\n" + GetHash(10);
    }

    //return a string
    public string GetHash(int length){
    	string hash = _prefix + " o_"; //hash that is going to get returned
    	string st = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    	int hashLength = length;

    	//iterate through an array
    	for(int i = 0; i < hashLength; i++){
    		//socket is the final generated letter or number 
    		string socket;
    		//decide whether its a letter or number
    		int binary = Random.Range(0,2);

    		//if its 0 then its a random number
    		if(binary == 0)
    		{
    			int generateNumber = Random.Range(0,9);
    			socket = generateNumber.ToString();
    			hash = hash + socket;
    		}

    		//if its 1 then its a letter
    		if(binary == 1)
    		{
    			char c = st[Random.Range(0,st.Length)];
    			socket = c.ToString();
    			hash = hash + socket; 
    		}
    	}

    	return hash;
    }
}
