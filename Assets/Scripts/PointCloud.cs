using UnityEngine;
using UnityEditor;
using System.Collections;
using Windows.Kinect;

public class PointCloud : MonoBehaviour
{
    [Header("MultiSourceManager")]
    public GameObject MultiSourceManager;

    private KinectSensor _Sensor;
    private CoordinateMapper _Mapper;

    private MultiSourceManager _MultiManager;

    FrameDescription depthFrameDesc;
    CameraSpacePoint[] cameraSpacePoints;

    [Header("Kinect Properties")]
    private int depthWidth;
    private int depthHeight;
    private Vector2Int _depthResolution = new Vector2Int(64,64);
    public int _downSampling = 1;

    public Color color = Color.white;
    public float size = 0.2f;
    public float scale = 10f;

    [Header("PointCloud")]
    //point cloud mesh
    public GameObject _dotMesh;
    public GameObject[] _pointCloudDots;
    public GameObject[] _placeHolder;
    public int increment = 64;

    [Header("Cutoff Points")]
    //cutoff points for downsampling
    [Range(0,1.0f)]
    public float _depthSensitivity = 1;
    [Range(0,300f)]
    public float _wallDepth = 100;

    [Range(-1,1f)]
    public float _topCutOff = 1;
    [Range(-1,1f)]
    public float _bottomCutOff = -1;
    [Range(-1,1f)]
    public float _leftCutOff = -1;
    [Range(-1,1f)]
    public float _rightCutOff = 1;

    void Start()
    {
        //init the kinect sensor and everything else
        _Sensor = KinectSensor.GetDefault();
        if (_Sensor != null)
        {
            _Mapper = _Sensor.CoordinateMapper;

            depthFrameDesc = _Sensor.DepthFrameSource.FrameDescription;
            depthWidth = depthFrameDesc.Width;
            depthHeight = depthFrameDesc.Height;

            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }

            // particles = new ParticleSystem.Particle[depthWidth * depthHeight];
            _pointCloudDots = new GameObject[(_depthResolution.x/_downSampling) * (_depthResolution.y/_downSampling)];
            cameraSpacePoints = new CameraSpacePoint[depthWidth * depthHeight];

            //initialize the pointcloud
            for(int i = 0; i < _pointCloudDots.Length; i++){
                _pointCloudDots[i] = (GameObject)Instantiate(_dotMesh, gameObject.transform.position, gameObject.transform.rotation);
            }

            //debug
            Debug.Log("width" + depthWidth);
            Debug.Log("height" + depthHeight);
            Debug.Log("pointCloudDotsLength" + _pointCloudDots.Length);
            Debug.Log("cameraSpacePoints" + cameraSpacePoints.Length);
        }
    }


    void Update()
    {

        //checks for managers
        if (_Sensor == null) return;
        if (MultiSourceManager == null) return;

        _MultiManager = MultiSourceManager.GetComponent<MultiSourceManager>();
        if (_MultiManager == null) return;

        // gameObject.GetComponent<Renderer>().material.mainTexture = _MultiManager.GetColorTexture();

        ushort[] rawdata = _MultiManager.GetDepthData();

        _Mapper.MapDepthFrameToCameraSpace(rawdata, cameraSpacePoints);

        // for (int i = 0; i < cameraSpacePoints.Length; i++)
        // {
        //     // _pointCloudDots[i].transform.position = new Vector3(cameraSpacePoints[i].X * scale, cameraSpacePoints[i].Y * scale, cameraSpacePoints[i].Z * scale);

        //     // if (rawdata[i] == 0) _pointCloudDots[i].transform.localScale = new Vector3(0,0,0);


        // }

        //filter and downsample
        for(int x = 0; x < _depthResolution.x/_downSampling; x++){
            for(int y = 0; y < _depthResolution.y/_downSampling; y++){

                //adjust 2d array to 1d
                int sampleIndex = (y * (_depthResolution.x/_downSampling)) + x;
                // sampleIndex *= _downSampling;

                //get rid of the infinity error
                if (_pointCloudDots[sampleIndex].transform.position.x > 100000000 || _pointCloudDots[sampleIndex].transform.position.x < 100000000)
                    _pointCloudDots[sampleIndex].transform.position = new Vector3(0,0,0);

                //set the positions based on z-threshold
                if(cameraSpacePoints[sampleIndex].Z < _wallDepth)
                    _pointCloudDots[sampleIndex].transform.position = new Vector3(cameraSpacePoints[sampleIndex].X * scale, cameraSpacePoints[sampleIndex].Y * scale, cameraSpacePoints[sampleIndex].Z * scale);

                //debug
                // Debug.Log("Z axis" + cameraSpacePoints[sampleIndex].Z);
            }
        }


        StartCoroutine("Delay");

        //parent and save on command 
        if(Input.GetKeyDown(KeyCode.UpArrow))
            parentAndSave(_placeHolder);

        //debug
        if(Input.GetKeyDown("space"))
            Debug.Log(cameraSpacePoints);

    }

    //parents the objects and saves them
    public void parentAndSave(GameObject[] pointCloud){

        //specify the local path where you want to save the prefab
        string path = "Assets/Data" + gameObject.name + ".prefab";

        //create an empty
        GameObject emptyParent; 
        emptyParent = new GameObject();

        //parent to empty
        for(int i = 0; i < pointCloud.Length; i++){
            pointCloud[i].transform.SetParent(emptyParent.transform);
        }

        //save them to directory
        if (AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)))
            CreateNew(emptyParent, path);

    }

    //create prefab to specified local path 
    static void CreateNew(GameObject obj, string localPath)
    {
        //Create a new Prefab at the path given
        Object prefab = PrefabUtility.CreatePrefab(localPath, obj);
        PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ConnectToPrefab);
    }

    private IEnumerator Delay() {
        yield return new WaitForSeconds(1.0f);
    }

    //when the app quits
    void OnApplicationQuit()
    {
        if (_Mapper != null)
        {
            _Mapper = null;
        }

        if (_Sensor != null)
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }

            _Sensor = null;
        }
    }
}