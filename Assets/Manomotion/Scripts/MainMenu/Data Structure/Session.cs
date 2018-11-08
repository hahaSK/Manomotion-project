using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides information regarding the platform that the SDK is currently being deployed to.
/// </summary>
public enum Platform
{
    UNITY_ANDROID,
    UNITY_IOS
};

public enum AddOn
{
    DEFAULT,
    AR_KIT,
    AR_CORE,
    VUFORIA
};


/// <summary>
/// Provides information regarding the different orientation types supported by the SDK.
/// </summary>
public enum SupportedOrientation
{
    LANDSCAPE_LEFT = 3,
    LANDSCAPE_RIGHT = 4,
    PORTRAIT = 1,
    PORTRAIT_INVERTED = 2
};

/// <summary>
/// Provides information regarding the image format the SDK can process in order to have a seemless integration.
/// </summary>
public enum ImageFormat
{
    BGRA_IMAGE = 5,
    GRAYSCALE = 4,
    DEPTH_MAP = 3,
    YUV_IMAGE = 2,
    VUFORIA_IMAGE = 1,
    RGBA_IMAGE = 0
};

/// <summary>
/// Provides additional information regarding the lincenses taking place in this application.
/// </summary>
public enum Flags
{
    FLAG_LICENSE_OK = 30,
    FLAG_LICENSE_KEY_NOT_FOUND = 31,
    FLAG_LICENSE_EXPIRED_WARNING = 32,
    FLAG_LICENSE_INVALID_PLAN = 33,
    FLAG_LICENSE_KEY_BLOCKED = 34,
    FLAG_INVALID_ACCESS_TOKEN = 35,
    FLAG_LICENSE_ACCESS_DENIED = 36,
    FLAG_LICENSE_MAX_NUM_DEVICES = 37,
    FLAG_UNKNOWN_SERVER_REPLY = 38,
    FLAG_LICENSE_PRODUCT_NOT_FOUND = 39,
    FLAG_LICENSE_INCORRECT_INPUT_PARAMETER = 40,
    FLAG_LICENSE_INTERNET_REQUIRED = 41,
    FLAG_BOUNDLE_ID_DOESENT_MATCH = 42,
    FLAG_IMAGE_SIZE_IS_ZERO = 1000,
    FLAG_IMAGE_IS_TOO_SMALL = 1001
};

public struct Session
{
    public Flags flag;
    public Platform current_plataform;
    public ImageFormat image_format;
    public SupportedOrientation orientation;
    public AddOn add_on;
    public int version;
    public float smoothing_controller;
}