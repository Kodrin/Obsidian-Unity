using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCloudDuplicate : MonoBehaviour
{
	/* SCRIPT USED FOR PERFORMANCE PURPOSE SINCE
	WE DON'T WANT TO CALL THE DATA FROM THE KINECT 
	WITH EVERY OBJECT */

	[Header("Duplicate Source")]
	public GameObject _originalPointCloud;
	private Texture2D _duplicatedTexture; //to store the original texture onto the duplicate
	// private Material _mat; //the material for the shader
	private Mesh _mesh; //to duplicate the generated mesh also
	DepthSourceManagerMy _depthSourceManager; //script component from original PointCloud

	[SerializeField]
	private Material _mat; //the material for the shader

    // Start is called before the first frame update
    void Start()
    {
        //fetch the component
        _depthSourceManager = _originalPointCloud.GetComponent<DepthSourceManagerMy>();
    	_mesh = GetComponent<MeshFilter>().mesh;
    	_mat = GetComponent<Renderer>().material;
    	//duplicate the mesh
    	_mesh = _originalPointCloud.GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    void Update()
    {
        //update the duplicate pointcloud with the original point cloud texture
        _duplicatedTexture =_depthSourceManager._Texture;
        _mat.SetTexture("_MainTex", _duplicatedTexture);
    }
}
