using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
// using System;
// using System.IO;


public class LoadData : MonoBehaviour
{

	[Header("Placement Properties")]

	public int _spacing; //spacing between each of the point clouds 
	public int _horizontalIncrement; //starts a new row after x amount of point clouds
	private Object[] _loadedObjects; //preliminary loading of all the assets in the folder 
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
    	if(Input.GetKeyDown("space"))
        	PlacePointClouds(_loadedObjects);
    }

    public void LoadObjectsFromDataFolder(string path){
    	_loadedObjects = Resources.LoadAll(path, typeof(GameObject));

    	foreach(var pointCloud in _loadedObjects){

    		//debug
    		Debug.Log(pointCloud.name);
    	}
    }

    public void PlacePointClouds(Object[] PointClouds){
    	//row counter
    	int rowCount = 1;
    	int yModifier = 1;

    	//place those point clouds into a mural
    	for(int i = 0; i < PointClouds.Length; i++){
    		//set parent to gameobject
    		// PointClouds[i].transform.SetParent(_pointCloudMuralPosition.transform);

    		//instantiate those point clouds 
    		Vector3 pointCloudPosition = new Vector3(_pointCloudMuralPosition.transform.position.x * rowCount,_pointCloudMuralPosition.transform.position.y * yModifier,_pointCloudMuralPosition.transform.position.z);
    		PointClouds[i] = (GameObject)Instantiate(PointClouds[i],pointCloudPosition, _pointCloudMuralPosition.transform.rotation);

    		//start a new row based on the increment value
    		if(rowCount % _horizontalIncrement == 0){
    			//y axis is incremented 
    			yModifier++;
    		}

    		//increment the row count and reset it when it reaches horizontal increment point
    		if(rowCount == _horizontalIncrement){
    			//reset the row count
    			rowCount = 1;
    		}
    		else{
    			//increment the row count
    			rowCount++;
    		}
    		
    	}

    }
}
