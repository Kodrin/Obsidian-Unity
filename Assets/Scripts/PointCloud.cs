using UnityEngine;
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

    private int depthWidth;
    private int depthHeight;

    [Header("Particle System")]
    //particle system variables
    ParticleSystem _particleSystem;
    private ParticleSystem.Particle[] particles;

    public Color color = Color.white;
    public float size = 0.2f;
    public float scale = 10f;

    [Header("PointCloud")]
    //point cloud mesh
    public GameObject _dotMesh;
    public GameObject[][] _pointCloudDots;
    public int increment = 64;

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
            // _pointCloudDots = new _pointCloudDots[depthWidth * depthHeight];
            cameraSpacePoints = new CameraSpacePoint[depthWidth * depthHeight];

            //initialize the pointcloud
            // for(int x = 0; x < depthWidth; x+=increment){
            //     for(int y = 0; y < depthHeight; y+=increment){
            //         _pointCloudDots[x][y] = new (GameObject)Instantiate(_dotMesh, gameObject.transform.position, gameObject.transform.rotation);
            //     }
            // }

            //debug
            Debug.Log("width" + depthWidth);
            Debug.Log("height" + depthHeight);
            Debug.Log("cameraSpacePoints" + cameraSpacePoints.Length);
        }
    }


    void Update()
    {

        if (_Sensor == null) return;
        if (MultiSourceManager == null) return;

        _MultiManager = MultiSourceManager.GetComponent<MultiSourceManager>();
        if (_MultiManager == null) return;

        // gameObject.GetComponent<Renderer>().material.mainTexture = _MultiManager.GetColorTexture();

        ushort[] rawdata = _MultiManager.GetDepthData();

        _Mapper.MapDepthFrameToCameraSpace(rawdata, cameraSpacePoints);

        for (int i = 0; i < cameraSpacePoints.Length; i++)
        {
            if(Input.GetKeyDown("space"))
                Debug.Log(cameraSpacePoints[i].X + "" + cameraSpacePoints[i].Y + "" + cameraSpacePoints[i].Z);
            // particles[i].position = new Vector3(cameraSpacePoints[i].X * scale, cameraSpacePoints[i].Y * scale, cameraSpacePoints[i].Z * scale);
            //particles[i].position = Random.insideUnitSphere * 10;
            // Debug.Log(cameraSpacePoints.Length);
            // particles[i].startColor = color;
            // particles[i].startSize = size;
            // if (rawdata[i] == 0) particles[i].startSize = 0;
        }

        // _particleSystem = gameObject.GetComponent<ParticleSystem>();

        // _particleSystem.SetParticles(particles, particles.Length);


        StartCoroutine("Delay");

        //debug
        // if(Input.GetKeyDown("space"))
        //     Debug.Log(cameraSpacePoints);

    }

    private IEnumerator Delay() {
        yield return new WaitForSeconds(1.0f);
    }
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