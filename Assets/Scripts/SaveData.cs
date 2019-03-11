using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class SaveData : MonoBehaviour
{
	[Header("Data Properties")]

	public GameObject _sampleObj;
	public GameObject _spawnLocation;
	public GameObject[] _dataArray;
	public int _sizeOfData = 10;
	public string _localPath = "Assets/Data/";
	public string _prefix = ".prefab";



    // Start is called before the first frame update
    void Start()
    {
    	//populate the array
    	populateDebug();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space"))
        	parentAndSave(_dataArray);
    }

    //function that populate empty array
    public void populateDebug(){
    	_dataArray = new GameObject[_sizeOfData];

    	for(int i = 0; i < _dataArray.Length; i++){
    		_dataArray[i] = (GameObject)Instantiate(_sampleObj, _spawnLocation.transform.position, _spawnLocation.transform.rotation);
    	}

    }

    //parents the objects and saves them
    public void parentAndSave(GameObject[] pointCloud){

        //create an empty
        GameObject emptyParent; 
        emptyParent = new GameObject();

        //specify the local path where you want to save the prefab
        int randomNumber = Random.Range(0, 10);
        string randomName = "00000" + randomNumber;
        string path = _localPath + randomName + _prefix;

        //parent to empty
        for(int i = 0; i < pointCloud.Length; i++){
            pointCloud[i].transform.SetParent(emptyParent.transform);
        }

        //save them to directory
        // if (AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)))
        //     CreateNew(emptyParent, path);

        //debug
        Debug.Log(path);
    }

    //create prefab to specified local path 
    // public void CreateNew(GameObject obj, string localPath)
    // {
    //     //Create a new Prefab at the path given
    //     Object prefab = PrefabUtility.CreateEmptyPrefab(localPath);
    //     PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ConnectToPrefab);
    // }
}
