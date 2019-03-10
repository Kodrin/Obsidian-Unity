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
	private Texture2D _Texture;

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
			//_Texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.RGBA4444, false);
			//_Texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.ARGB4444, false);
			_Texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.R16, false);

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

		//debug
		//save texture
		if(Input.GetKeyDown(KeyCode.S) && doNotSetTexture != true)
			SaveTextureAsPNG(_Texture, pathToTexture, pngFileName);
		//capture
		if(Input.GetKeyDown("space") && doNotSetTexture != true)
			captureData(LoadPNG(Application.dataPath + pathToTexture + pngFileName + ".png"));
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

	//debug and test
	public void captureData(Texture2D loadedTexture){
		//get components 
		Renderer captMat = captureObject.GetComponent<Renderer>();
		// captMat.material.SetTexture("_MainTex", _Texture);
		captMat.material.SetTexture("_MainTex", loadedTexture);



	}

	//save png
	public static void SaveTextureAsPNG(Texture2D _texture, string _fullPath, string _fileName)
	 {
	     byte[] _bytes =_texture.EncodeToPNG();
	     System.IO.File.WriteAllBytes(Application.dataPath + _fullPath + _fileName + ".png", _bytes);
	     Debug.Log(_bytes.Length/1024  + "Kb was saved as: " + _fullPath);
	 }

	//load png
	public static Texture2D LoadPNG(string filePath) {

	 Texture2D tex = null;
	 byte[] fileData;

	 if (File.Exists(filePath))     {
	     fileData = File.ReadAllBytes(filePath);
	     tex = new Texture2D(2, 2);
	     tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
	 }
	 return tex;
	}
}
