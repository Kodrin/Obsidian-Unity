using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;


public class LoadData : MonoBehaviour
{

	[Header("Placement Properties")]

	public int _spacingX; //spacing between each of the point clouds 
    public int _spacingY;
    public int _spacingIncrement = 5; 
	public int _horizontalIncrement; //starts a new row after x amount of point clouds
	private UnityEngine.Object[] _loadedObjects; //preliminary loading of all the assets in the folder 
	public GameObject _pointCloudMuralPosition; //use an empty and reference it here
	public string _localPath = "PointClouds";

    // Start is called before the first frame update
    void Start()
    {
    	//pre-emptively load all the data from the folder
    	LoadObjectsFromDataFolder(_localPath);
    }

    // Update is called once per frame
    void Update()
    {
        //debug
        if(Input.GetKeyDown("f"))
            GetDirectoryInfo();
    	if(Input.GetKeyDown("space"))
        	PlacePointClouds(_loadedObjects);
    }

    //get a list in the directory
    public void GetDirectoryInfo(){
        var info = new DirectoryInfo(Application.dataPath);
        var fileInfo = info.GetFiles();

        foreach(var file in fileInfo){
            Debug.Log(file);
        }

        //debug
        // Debug.Log();
    }

    //load object from folder using ressource folder
    public void LoadObjectsFromDataFolder(string path){
    	_loadedObjects = Resources.LoadAll(path, typeof(GameObject));

    	foreach(var pointCloud in _loadedObjects){

    		//debug
    		Debug.Log(pointCloud.name);
    	}
    }

    public void PlacePointClouds(UnityEngine.Object[] PointClouds){
    	//row counter
    	int rowCount = 1;
    	int yModifier = 1;

    	//place those point clouds into a mural
    	for(int i = 0; i < PointClouds.Length; i++){
    		//set parent to gameobject
    		// PointClouds[i].transform.SetParent(_pointCloudMuralPosition.transform);

    		//instantiate those point clouds 
            Vector3 pointCloudPositionTest = new Vector3(_pointCloudMuralPosition.transform.position.x + _spacingX,_pointCloudMuralPosition.transform.position.y + _spacingY,_pointCloudMuralPosition.transform.position.z);

    		// Vector3 pointCloudPosition = new Vector3(_pointCloudMuralPosition.transform.position.x * rowCount,_pointCloudMuralPosition.transform.position.y * yModifier,_pointCloudMuralPosition.transform.position.z); //old code through multiplication
    		PointClouds[i] = (GameObject)Instantiate(PointClouds[i],pointCloudPositionTest, _pointCloudMuralPosition.transform.rotation);

            //SET THE MAT/TEXTURE/NAME FOR EACH ONE OF THEM
            //..........

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
}
