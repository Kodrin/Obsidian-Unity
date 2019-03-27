using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;


public class LoadData : MonoBehaviour
{
	[Header("Am I debugging?")]
	public bool _isDebugging = false;
    public Texture2D _debugText;

	[Header("Placement Properties")]

	public int _spacingX; //spacing between each of the point clouds 
    public int _spacingY;
    public int _spacingIncrement = 5;
	public int _horizontalIncrement; //starts a new row after x amount of point clouds
    public bool _invert = false;
	public int _invertedIncrement = 15; //point at which the mural inverts

	public GameObject _pointCloudMuralPosition; //use an empty and reference it here
	public GameObject _pointCloudMuralPositionInverted; //use an empty and reference it here
    public GameObject _pointCloudTemplate; //prefab of the point cloud
    public GameObject[] _pointClouds; // array to store all the static point clouds
    public GameObject _pointCloudParent; //parent where the point cloud will get stored

	public string _localPath = "PointClouds";

	private UnityEngine.Object[] _loadedObjects; //preliminary loading of all the assets in the folder 
	public Texture2D[] _loadedTextures; //to store all the loaded objects

	[Header("Shader Properties")]
	public Shader _assignedShader;
    public Material _assignedMat;

    void Awake()
    {
        //pre-emptively load all the data from the folder
        LoadObjectsFromDataFolder(_localPath);
        PlacePointClouds(_loadedTextures);
    }


    // Start is called before the first frame update
    void Start()
    {
        // PlaceMaterialOnPointCloud(_loadedTextures);
    }

    // Update is called once per frame
    void Update()
    {
        //DEBUG
        if(_isDebugging){
	    	if(Input.GetKeyDown("space"))
	        	PlacePointClouds(_loadedTextures);
            if(Input.GetKeyDown("s"))
                ShiftData(_debugText);
        }
    }

    //-----------------------------------------------
    // lOADING RESSOURCES
    //load object from folder using ressource folder
    public void LoadObjectsFromDataFolder(string path){

    	//load from ressources
    	_loadedObjects = Resources.LoadAll(path, typeof(Texture2D));
		//initialize the array
		_loadedTextures = new Texture2D[_loadedObjects.Length];
		_pointClouds = new GameObject[_loadedObjects.Length];

		//assing the object to the texture list
    	for(int i = 0; i < _loadedObjects.Length; i ++){
    		//convert to object to texture and cast it
    		_loadedTextures[i] = (Texture2D) _loadedObjects[i];
    	}

    	//DEBUG
    	if(_isDebugging){
	    	foreach(var pointCloud in _loadedObjects){
	    		//debug
	    		Debug.Log(pointCloud.name);
	    	}
    	}
    }

    //-----------------------------------------------------------
    // PLACE THE POINT CLOUD ACCORDING TO MURAL POSITION
    //-----------------------------------------------------------
    public void PlacePointClouds(Texture2D[] PointCloudsTextures){
    	//row counter
    	int rowCount = 1;
    	int yModifier = 1;

    	bool alternatePosition = false;

    	//place those point clouds into a mural
    	for(int i = 0; i < PointCloudsTextures.Length; i++){

    		//instantiate those point clouds 
            Vector3 pointCloudPosition = new Vector3(_pointCloudMuralPosition.transform.position.x,_pointCloudMuralPosition.transform.position.y + _spacingY,_pointCloudMuralPosition.transform.position.z+ _spacingX);
            Vector3 pointCloudPositionInverted = new Vector3(_pointCloudMuralPositionInverted.transform.position.x + _spacingX,_pointCloudMuralPositionInverted.transform.position.y + _spacingY,_pointCloudMuralPositionInverted.transform.position.z);

            //INVERT increments
            if(_invert){
                if(i > _invertedIncrement){
                	alternatePosition = true;
                }
                
                if(_spacingX/_spacingIncrement % _invertedIncrement == 0){
                	_spacingX = 0;
                	_spacingY = 0;
                }
            }

            //IF INVERT IS ON , THEN STACK THE DATA ON 2 PLANES, ELSE STACK IT ON 1 PLANE (MURAL)
            if(_invert){
                if(!alternatePosition){
        			_pointClouds[i] = (GameObject)Instantiate(_pointCloudTemplate,pointCloudPosition, _pointCloudMuralPosition.transform.rotation);
        		} else {
        			_pointClouds[i] = (GameObject)Instantiate(_pointCloudTemplate,pointCloudPositionInverted, _pointCloudMuralPositionInverted.transform.rotation);
                }
            } else {
                _pointClouds[i] = (GameObject)Instantiate(_pointCloudTemplate,pointCloudPosition, _pointCloudMuralPosition.transform.rotation);
            }

            //SET THE MAT/TEXTURE/NAME FOR EACH ONE OF THEM
            //..........
            SetSubjectName _setSubjName = _pointClouds[i].GetComponent<SetSubjectName>();
            _setSubjName.SetName(GetHash(10));
            Renderer _pcRend = _pointClouds[i].GetComponent<Renderer>(); //fetch the renderer to assign material
            // _pcRend.material = new Material(_assignedShader);
            _pcRend.material = _assignedMat;
            _pcRend.material.SetTexture("_MainTex",PointCloudsTextures[i]); //assign the material

            //PARENT THE POINT CLOUD 
            _pointClouds[i].transform.SetParent(_pointCloudParent.transform);

    		//start a new row based on the increment value
    		if(rowCount % _horizontalIncrement == 0){
    			//y axis is incremented 
    			yModifier++;
                //add the spacing on the y
                _spacingY += _spacingIncrement;
    		}

    		//increment the row count and reset it when it reaches horizontal increment point
    		if(rowCount == _horizontalIncrement){
    			//reset the row count
    			rowCount = 1;
                //reset the spacing on the x
                _spacingX = 0;
    		}
    		else{
    			//increment the row count
    			rowCount++;
                //add the spacing on the x
                _spacingX += _spacingIncrement;
    		}
    		
    	}
    }

    private void PlaceMaterialOnPointCloud(Texture2D[] PointCloudsTextures){
        for(int i = 0; i < _pointClouds.Length; i++){
            Renderer _pcRend = _pointClouds[i].GetComponent<Renderer>(); //fetch the renderer to assign material
            _pcRend.material = new Material(_assignedShader);
            _pcRend.material.SetTexture("_MainTex",PointCloudsTextures[i]); //assign the material
        }
    }

    //---------------------------------
    //SHIFT POINT CLOUD DATA

    /*
    param: _newParticipant
    param: _loadedTextures;
    */

    public void ShiftData(Texture2D newParticipant){
        //temp var to store index
        Texture2D currentIndexText = newParticipant;

        //SHIFT THE ARRAY
        for(int i = 0; i < _loadedTextures.Length - 1; i++){
                _loadedTextures[i] =  _loadedTextures[i+1];
        }
        _loadedTextures[_loadedTextures.Length -1] = currentIndexText;
        
        ////apply the new texture
        for(int i = 0; i < _pointClouds.Length; i++){
            Renderer _pcRend = _pointClouds[i].GetComponent<Renderer>(); 
            _pcRend.material.SetTexture("_MainTex",_loadedTextures[i]); 
        }
    }


    //---------------------------------
    //HASH GENERATOR/ GET A UNIQUE AND RANDOM NAME
    public string GetHash(int length){
    	string hash = "o_"; //hash that is going to get returned
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

    	return hash;
    }
}
