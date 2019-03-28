using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class PointCloudDataManager : MonoBehaviour
{
	[Header("Am I debugging?")]
	public bool _isDebugging = false;
	public Texture2D _debugText;
	public GameObject captureObject;

	[Header("Capture Point Cloud")]
	public GameObject livepointCloudFeed;
	public string lastPngSaved;
	public bool doNotSetTexture = false;

	//components
	[Header("Components")]
	public WorldManager worldMan;
	DepthSourceManagerMy depthManager;

	[Header("Hash Parameters")]
	public string _prefix = "0";


    // Start is called before the first frame update
    void Start()
    {
    	//fetch components
    	depthManager = livepointCloudFeed.GetComponent<DepthSourceManagerMy>(); //referece to the depth manager for the texture cpature
        
    }

    // Update is called once per frame
    void Update()
    {

        //DEBUG
        if(_isDebugging){ 
	        //create directory 
	        if(Input.GetKeyDown("d"))
	        	Directory.CreateDirectory(Application.dataPath);
			//save texture
			if(Input.GetKeyDown(KeyCode.S) && doNotSetTexture != true)
				SaveTextureAsPNG(depthManager._Texture, GetHash(10));
			//capture
			if(Input.GetKeyDown("space") && doNotSetTexture != true)
				captureData(LoadPNG(Application.dataPath + "/" + lastPngSaved + ".png"));
		}
    }


	//-----------------------------------
	//PROJECTING DATA ONTO ANOTHER OBJECT
	private void captureData(Texture2D loadedTexture){
		//get components 
		Renderer captMat = captureObject.GetComponent<Renderer>();
		// captMat.material.SetTexture("_MainTex", _Texture);
		captMat.material.SetTexture("_MainTex", loadedTexture);
	}

	//public function to save the participant's data
	public void SaveParticipant(){
		//save the texture as a png
		SaveTextureAsPNG(depthManager._Texture, GetHash(10));

		//load the png
		worldMan._participantPointCloud = LoadPNG(Application.dataPath + "/" + lastPngSaved + ".png");

		//store it temporarily in a public variable 
		// worldMan._participantPointCloud = depthManager._Texture;
	}

	//SAVE DEPTH AS PNG
	public static void SaveTextureAsPNG(Texture2D _texture, string _fileName)
	 {
	 	//encoding
	     byte[] _bytes = _texture.EncodeToPNG();
	     System.IO.File.WriteAllBytes(Application.dataPath + "/" + _fileName + ".png", _bytes);

	     //DEBUG
	     // Debug.Log( Application.dataPath );
	    // Debug.Log(_bytes.Length/1024  + "Kb was saved as: " + Application.dataPath + _fileName + ".png");
	 }

	//LOAD PNG
	public static Texture2D LoadPNG(string filePath) {

	 Texture2D tex = null;
	 byte[] fileData;

	 if (File.Exists(filePath))     {
	     fileData = File.ReadAllBytes(filePath);
	     tex = new Texture2D(512, 424);
	     tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
	 }
	 return tex;
	}

    //---------------------------------
    //HASH GENERATOR/ GET A UNIQUE AND RANDOM NAME
    public string GetHash(int length){
    	string hash = _prefix; //hash that is going to get returned
    	string st = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    	int hashLength = length;

    	//iterate through an array
    	for(int i = 0; i < hashLength; i++){
    		//socket is the final generated letter or number 
    		string socket;
    		//decide whether its a letter or number
    		int binary = UnityEngine.Random.Range(0,2);

    		//if its 0 then its a random number
    		if(binary == 0)
    		{
    			int generateNumber = UnityEngine.Random.Range(0,9);
    			socket = generateNumber.ToString();
    			hash = hash + socket;
    		}

    		//if its 1 then its a letter
    		if(binary == 1)
    		{
    			char c = st[UnityEngine.Random.Range(0,st.Length)];
    			socket = c.ToString();
    			hash = hash + socket; 
    		}
    	}

    	//DEBUG
    	lastPngSaved = hash;

    	return hash;
    }
}
