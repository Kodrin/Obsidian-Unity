using System;
using System.IO;
using UnityEngine;
using Windows.Kinect;

public class DepthSourceManagerMy : MonoBehaviour
{
	//debug
	[Header("Capture Object")]
	public GameObject captureObject;
	public string pathToTexture;
	public string pngFileName;
	public bool doNotSetTexture = false;

	private KinectSensor _Sensor;
	private DepthFrameReader _Reader;
	private ushort[] _Data;
	private byte[] _RawData;
	public Texture2D _Texture; //made it public so the PointCloudDuplicate.cs can access it

	[SerializeField]
	private Material mat;

	public ushort[] GetData()
	{
		return _Data;
	}

	void Start()
	{
		_Sensor = KinectSensor.GetDefault();

		if (_Sensor != null)
		{
			_Reader = _Sensor.DepthFrameSource.OpenReader();
			var frameDesc = _Sensor.DepthFrameSource.FrameDescription;
			_Data = new ushort[frameDesc.LengthInPixels];
			_RawData = new byte[frameDesc.LengthInPixels * 2];

			// 16bit のテクスチャ。適切な単色のフォーマットがないので
			// RGBA4444 or ARGB4444 or R16 で16bit分確保する
			_Texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.RGBA4444, false);
			//_Texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.ARGB4444, false);
			// _Texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.R16, false);

			if (!_Sensor.IsOpen)
			{
				// スタート
				_Sensor.Open();
			}
		}

		//do not set texture bool
		if(doNotSetTexture != true){
			_Texture.LoadRawTextureData(_RawData);
			_Texture.Apply();
			mat.SetTexture("_MainTex", _Texture);
		}
	}

	void Update()
	{
		if (_Reader != null)
		{
			var frame = _Reader.AcquireLatestFrame();
			var frameDesc = _Sensor.DepthFrameSource.FrameDescription;
			if (frame != null)
			{
				frame.CopyFrameDataToArray(_Data);
				
				// ushort(16bit) のアレーをそのまま byte(8bit)のアレーとしてコピーする
				// https://stackoverflow.com/questions/37213819/convert-ushort-into-byte-and-back
				// https://msdn.microsoft.com/en-us/library/system.buffer.blockcopy(v=vs.110).aspx
				Buffer.BlockCopy(_Data, 0, _RawData, 0, _Data.Length * 2);

				// byteのデータを使ってテクスチャデータを更新する
				// https://docs.unity3d.com/ScriptReference/Texture2D.LoadRawTextureData.html
				_Texture.LoadRawTextureData(_RawData);
				_Texture.Apply();

				frame.Dispose();
				frame = null;
			}
		}
	}

	void OnApplicationQuit()
	{
		if (_Reader != null)
		{
			_Reader.Dispose();
			_Reader = null;
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
