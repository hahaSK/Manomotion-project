using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

#if UNITY_IOS
using UnityEngine.iOS;
#endif

[AddComponentMenu("ManoMotion/ManoMotion Manager")]
[RequireComponent(typeof(ManoEvents))]
public class ManomotionManager : ManomotionBase
{



    #region Singleton
    protected static ManomotionManager instance;
    #endregion


    #region consts
    const int STARTING_WIDTH = 640;
    const int STARTING_HEIGHT = 480;
    #endregion


    #region variables
    protected HandInfoUnity[] hand_infos;
    protected VisualizationInfo visualization_info;
    protected Session manomotion_session;

    protected int _frame_number;
    protected int _width = STARTING_WIDTH;
    protected int _height = STARTING_HEIGHT;
    protected int _fps;
    protected int _processing_time;

    protected WebCamTexture _mCamera = null;
    private float fpsCooldown = 0;
    private int frameCount = 0;
    private List<int> processing_time_list = new List<int>();

    protected Color32[] _pixels;
    protected Color32[] framePixelColors;

    #endregion

    #region imports

#if UNITY_IOS
    const string library = "__Internal";
#elif UNITY_ANDROID
    const string library = "manomotion";
#else
    const string library = "manomotion";
#endif


    [DllImport(library)]
    private static extern void processFrame(ref HandInfo hand_info0, ref Session manomotion_session);

    [DllImport(library)]
    private static extern void setFrameArray(Color32[] frame);

    [DllImport(library)]
    private static extern void setResolution(int width, int height);

    #endregion

    #region init_wrappers

    [DllImport(library)]
    private static extern int init(string serial_key);

    protected void SetResolution(int width, int height)
    {
        Debug.Log("Set resolution " + width + "," + height);
#if !UNITY_EDITOR

        setResolution(width, height);
#endif
    }

    protected void SetFrameArray(Color32[] pixels)
    {
#if !UNITY_EDITOR
       
        setFrameArray(pixels);

#endif
    }

    #endregion

    #region Propperties
    internal int Processing_time
    {
        get
        {
            return _processing_time;
        }

    }

    internal int Fps
    {
        get
        {
            return _fps;
        }
    }

    internal int Height
    {
        get
        {
            return _height;
        }
    }

    internal int Width
    {
        get
        {
            return _width;
        }
    }

    internal int Frame_number
    {
        get
        {
            return _frame_number;
        }
    }

    internal VisualizationInfo Visualization_info
    {
        get
        {
            return visualization_info;
        }
    }

    internal HandInfoUnity[] Hand_infos
    {
        get
        {
            return hand_infos;
        }


    }

    public Session Manomotion_Session
    {
        get
        {
            return manomotion_session;
        }

    }

    public static ManomotionManager Instance
    {
        get
        {
            return instance;
        }


    }
    public string Serial_key
    {
        get
        {
            return serial_key;
        }

        set
        {
            serial_key = value;
        }
    }



    protected virtual void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            //   Destroy(this.gameObject);
            Debug.LogWarning("More than 1 Manomotionmanager in scene");
        }

    }

    protected void Start()
    {

        StartWebCamTexture();
        PickResolution(STARTING_WIDTH, STARTING_HEIGHT);
        SetUnityConditions();
    }


    /// <summary>
    /// Starts the camera for input.
    /// </summary>
    protected void StartWebCamTexture()
    {
        _mCamera = new WebCamTexture(WebCamTexture.devices[0].name, _width, _height);
        _mCamera.requestedFPS = 300;
        _mCamera.Play();
    }

    /// <summary>
    /// Picks the resolution.
    /// </summary>
    /// <param name="width">Requires a width value.</param>
    /// <param name="height">Requires a height value.</param>
    protected override void PickResolution(int width, int height)
    {
        _width = width;
        _height = height;

        InstantiateSession();
        InstantiateHandInfos();
        InstantiateVisualisationInfo();
        InitiateLibrary();
    }

    /// <summary>
    /// Instantiates the manager info.
    /// </summary>
    protected override void InstantiateSession()
    {
        manomotion_session = new Session();
#if UNITY_ANDROID
        manomotion_session.current_plataform = Platform.UNITY_ANDROID;
#elif UNITY_IOS
        manomotion_session.current_plataform = Platform.UNITY_IOS;
#endif
        manomotion_session.flag = Flags.FLAG_LICENSE_OK;
        manomotion_session.image_format = ImageFormat.RGBA_IMAGE;
        manomotion_session.orientation = SupportedOrientation.LANDSCAPE_LEFT;
        manomotion_session.add_on = AddOn.DEFAULT;
        manomotion_session.smoothing_controller = 0.30f;

    }

    /// <summary>
    /// Initializes the values for the hand information.
    /// </summary>
    private void InstantiateHandInfos()
    {


        hand_infos = new HandInfoUnity[1];
        for (int i = 0; i < hand_infos.Length; i++)
        {
            hand_infos[i].hand_info = new HandInfo();
            hand_infos[i].hand_info.gesture_info = new GestureInfo();
            hand_infos[i].hand_info.gesture_info.mano_class = ManoClass.NO_HAND;
            hand_infos[i].hand_info.gesture_info.hand_side = HandSide.None;
            hand_infos[i].hand_info.tracking_info = new TrackingInfo();
            hand_infos[i].hand_info.tracking_info.bounding_box = new BoundingBox();
            hand_infos[i].hand_info.tracking_info.bounding_box.top_left = new Vector3();

        }
    }

    /// <summary>
    /// Instantiates the visualisation info.
    /// </summary>
    private void InstantiateVisualisationInfo()
    {
        visualization_info = new VisualizationInfo();
        visualization_info.rgb_image = new Texture2D(_width, _height);
    }

    /// <summary>
    /// Initiates the library.
    /// </summary>
    protected void InitiateLibrary()
    {
        Debug.Log("Initiating ManoMotion SDK with serial key " + serial_key + " bundle id :" + Application.identifier);
        Init(serial_key);
        SetVariables();

    }

    /// <summary>
    /// Sets the variables.
    /// </summary>
    private void SetVariables()
    {
        SetResolution(_width, _height);
        SetVisualizationInfo();
    }


    /// <summary>
    /// Initializes the dimension of the pixel color array. 
    /// </summary>
    private void SetVisualizationInfo()
    {

        _pixels = new Color32[_width * _height];
        SetFrameArray(_pixels);
    }

    /// <summary>
    /// Sets the Application to not go to sleep mode as well as the requested framerate.
    /// </summary>
    protected override void SetUnityConditions()
    {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    #endregion


    bool _initialized;
    #region update_methods
    protected void Update()
    {
        if (!ManomotionManager.Instance)
        {
            instance = this;
            Debug.LogWarning("ManomotionManager.Instance");

        }

        if (_initialized)
        {
            if (!isPaused)
            {
                CalculateFPSAndProcessingTime();
                GetCameraFramePixelColors();
                UpdateTexturesWithNewInfo();
                ProcessManomotion();
                UpdateOrientation();

            }


        }

    }

    /// <summary>
    /// Updates the orientation information as captured from the device to the Session
    /// </summary>
    protected void UpdateOrientation()
    {
        switch (Input.deviceOrientation)
        {
            case DeviceOrientation.Unknown:
                break;
            case DeviceOrientation.Portrait:
                manomotion_session.orientation = (SupportedOrientation)Input.deviceOrientation;
                break;
            case DeviceOrientation.PortraitUpsideDown:
                manomotion_session.orientation = (SupportedOrientation)Input.deviceOrientation;
                break;
            case DeviceOrientation.LandscapeLeft:
                manomotion_session.orientation = (SupportedOrientation)Input.deviceOrientation;
                break;
            case DeviceOrientation.LandscapeRight:
                manomotion_session.orientation = (SupportedOrientation)Input.deviceOrientation;
                break;
            case DeviceOrientation.FaceUp:
                break;
            case DeviceOrientation.FaceDown:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Updates the RGB Frame of Visualization Info with the pixels captured from the camera
    /// </summary>
    protected override void UpdateTexturesWithNewInfo()
    {
        //RGB Frame
        Color32[] rgb_frame;
        rgb_frame = _pixels;
        visualization_info.rgb_image.SetPixels32(rgb_frame);
        visualization_info.rgb_image.Apply();

    }

    /// <summary>
    /// Gets the camera frame pixel colors.
    /// </summary>
    protected void GetCameraFramePixelColors()
    {

        framePixelColors = _mCamera.GetPixels32();

        if (framePixelColors.Length != Width * Height)
        {
            PickResolution(_mCamera.width, _mCamera.height);
        }

        framePixelColors.CopyTo(_pixels, 0);


    }

    /// <summary>
    /// Evaluates the dimension of the pixel color array and if it matches the dimensions proceeds with Processing the Frame. 
    /// </summary>
    protected override void ProcessManomotion()
    {
        if (_pixels.Length == Width * Height)
        {
            try
            {
                long start = System.DateTime.UtcNow.Millisecond + System.DateTime.UtcNow.Second * 1000 + System.DateTime.UtcNow.Minute * 60000;
                ProcessFrame();
                long end = System.DateTime.UtcNow.Millisecond + System.DateTime.UtcNow.Second * 1000 + System.DateTime.UtcNow.Minute * 60000;
                if (start < end)
                    processing_time_list.Add((int)(end - start));

            }
            catch (System.Exception ex)
            {
                Debug.Log("exeption: " + ex.ToString());

            }
        }
        else
        {
            Debug.Log("camera size doesent match: " + _pixels.Length + " != " + Width * Height);
        }
    }



    /// <summary>
    /// Calculates the Frames Per Second in the application and retrieves the estimated Processing time.
    /// </summary>
    protected override void CalculateFPSAndProcessingTime()
    {
        fpsCooldown += Time.deltaTime;
        frameCount++;
        if (fpsCooldown >= 1)
        {
            _fps = frameCount;
            frameCount = 0;
            fpsCooldown -= 1;
            CalculateProcessingTime();
        }
    }

    /// <summary>
    /// Calculates the elapses time needed for processing the frame.
    /// </summary>
    protected void CalculateProcessingTime()
    {
        if (processing_time_list.Count > 0)
        {
            int sum = 0;
            for (int i = 0; i < processing_time_list.Count; i++)
            {
                sum += processing_time_list[i];
            }
            sum /= processing_time_list.Count;
            _processing_time = sum;
            processing_time_list.Clear();
        }
    }
    #endregion

    #region update_wrappers

    /// <summary>
    /// Wrapper method that calls the ManoMotion core tech to process the frame in order to perform hand tracking and gesture analysis
    /// </summary>
    protected void ProcessFrame()
    {
#if !UNITY_EDITOR || UNITY_STANDALONE
 processFrame(ref hand_infos[0].hand_info, ref manomotion_session);
#endif

    }

    #endregion


    protected override void Init(string serial_key)
    {

#if !UNITY_EDITOR || UNITY_STANDALONE

            init(serial_key);
        _initialized = true;

#endif
    }

    public void SetManoMotionSmoothingValue(Slider slider)
    {
        manomotion_session.smoothing_controller = slider.value;
    }


    bool isPaused = false;
    void OnGUI()
    {
        if (!isPaused)
        {
            GUI.Label(new Rect(100, 100, 50, 30), "Mobile support only");

        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        isPaused = !hasFocus;
        if (isPaused && _mCamera != null)
        {
            if (_mCamera.isPlaying)
                _mCamera.Stop();
        }
        else
        {
            if (_mCamera != null && !_mCamera.isPlaying)
            {
                _mCamera.Play();
            }
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        isPaused = pauseStatus;
        if (isPaused && _mCamera != null)
        {
            if (_mCamera.isPlaying)
                _mCamera.Stop();
        }
        else
        {
            if (_mCamera != null && !_mCamera.isPlaying)
            {
                _mCamera.Play();
            }
        }
    }




}
