using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class PointCloud : MonoBehaviour
{
    public GameObject MultiSourceManager;

    private KinectSensor _Sensor;
    private CoordinateMapper _Mapper;

    private MultiSourceManager _MultiManager;

    FrameDescription depthFrameDesc;
    CameraSpacePoint[] cameraSpacePoints;

    private int depthWidth;
    private int depthHeight;

    ParticleSystem _particleSystem;
    private ParticleSystem.Particle[] particles;

    public Color color = Color.white;
    public float size = 0.2f;
    public float scale = 10f;

    void Start()
    {
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

            particles = new ParticleSystem.Particle[depthWidth * depthHeight];

            cameraSpacePoints = new CameraSpacePoint[depthWidth * depthHeight];
        }
    }


    void Update()
    {

        if (_Sensor == null) return;
        if (MultiSourceManager == null) return;

        _MultiManager = MultiSourceManager.GetComponent<MultiSourceManager>();
        if (_MultiManager == null) return;

        gameObject.GetComponent<Renderer>().material.mainTexture = _MultiManager.GetColorTexture();

        ushort[] rawdata = _MultiManager.GetDepthData();

        _Mapper.MapDepthFrameToCameraSpace(rawdata, cameraSpacePoints);

        for (int i = 0; i < cameraSpacePoints.Length; i++)
        {

            particles[i].position = new Vector3(cameraSpacePoints[i].X * scale, cameraSpacePoints[i].Y * scale, cameraSpacePoints[i].Z * scale);
            //particles[i].position = Random.insideUnitSphere * 10;
            Debug.Log(cameraSpacePoints.Length);
            particles[i].startColor = color;
            particles[i].startSize = size;
            if (rawdata[i] == 0) particles[i].startSize = 0;
        }

        _particleSystem = gameObject.GetComponent<ParticleSystem>();

        _particleSystem.SetParticles(particles, particles.Length);


        StartCoroutine("Delay");

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