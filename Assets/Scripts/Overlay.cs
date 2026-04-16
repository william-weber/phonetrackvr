using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using System;
using System.Runtime.InteropServices;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

public class Overlay : MonoBehaviour
{
    public Camera camera;
    public RenderTexture renderTexture;
    private ulong overlayHandle = OpenVR.k_ulOverlayHandleInvalid;

    // wrist overlay positioning
   [Range(0, 0.5f)] public float size = 0.2f;
   [Range(-0.2f, 0.2f)] public float x = -0.007f;
   [Range(-0.2f, 0.2f)] public float y = 0.084f;
   [Range(-0.2f, 0.2f)] public float z = -0.2f;
   [Range(0, 360)] public int rotationX = 71;
   [Range(0, 360)] public int rotationY = 157;
   [Range(0, 360)] public int rotationZ = 53;


    // Start is called before the first frame update
    void Start()
    {

        InitOpenVR();

        overlayHandle = CreateOverlay("PhoneTrackVROverlayKey", "PhoneTrackVR");

        // draw sample image
        //var filePath = Application.streamingAssetsPath + "/test.jpg";
        //SetOverlayFromFile(overlayHandle, filePath);

        SetOverlaySize(overlayHandle, size);

        ShowOverlay(overlayHandle);

    }

    private void OnApplicationQuit()
    {
        DestroyOverlay(overlayHandle);
    }

    // Update is called once per frame
    void Update()
    {
        SetOverlaySize(overlayHandle, size);

        var position = new UnityEngine.Vector3(x, y, z);
        var rotation = UnityEngine.Quaternion.Euler(rotationX, rotationY, rotationZ);
        var leftControllerIndex = OpenVR.System.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.LeftHand);
        if (leftControllerIndex != OpenVR.k_unTrackedDeviceIndexInvalid)
        {
            SetOverlayTransformRelative(overlayHandle, leftControllerIndex, position, rotation);
        }

        if (!renderTexture.IsCreated()) return;

        var nativeTexturePtr = renderTexture.GetNativeTexturePtr();
        var texture = new Texture_t
        {
            eColorSpace = EColorSpace.Auto,
            eType = ETextureType.DirectX,
            handle = nativeTexturePtr
        };

        var error = OpenVR.Overlay.SetOverlayTexture(overlayHandle, ref texture);
        if (error != EVROverlayError.None)
        {
            throw new Exception("Failed to set overlay texture: " + error.ToString());
        }
    }

    private void OnDestroy()
    {
        if (OpenVR.System == null) return; // Not initialized
        OpenVR.Shutdown();
    }

    private void InitOpenVR()
    {
        var error = EVRInitError.None;
        OpenVR.Init(ref error, EVRApplicationType.VRApplication_Overlay);

        if (error != EVRInitError.None)
        {
            throw new Exception("Failed to initialize OpenVR: " + error.ToString());
        }
    }

    private ulong CreateOverlay(string key, string name)
    {
        var handle = OpenVR.k_ulOverlayHandleInvalid;
        var error = OpenVR.Overlay.CreateOverlay(key, name, ref handle);
        if (error != EVROverlayError.None)
        {
            throw new Exception("Failed to create overlay: " + error.ToString());
        }
        return handle;
    }

    private void DestroyOverlay(ulong handle)
    {
        if (handle != OpenVR.k_ulOverlayHandleInvalid)
        {
            var error = OpenVR.Overlay.DestroyOverlay(handle);
            if (error != EVROverlayError.None)
            {
                throw new Exception("Failed to destroy overlay: " + error.ToString());
            }
        }
    }

    private void SetOverlayFromFile(ulong handle, string path)
    {
        var error = OpenVR.Overlay.SetOverlayFromFile(handle, path);
        if (error != EVROverlayError.None)
        {
            throw new Exception("Failed to set overlay image: " + error.ToString());
        }
    }

    private void ShowOverlay(ulong handle)
    {
        var error = OpenVR.Overlay.ShowOverlay(handle);
        if (error != EVROverlayError.None)
        {
            throw new Exception("Failed to show overlay: " + error.ToString());
        }
    }

    private void SetOverlaySize(ulong handle, float size)
    {
        var error = OpenVR.Overlay.SetOverlayWidthInMeters(handle, size);
        if (error != EVROverlayError.None)
        {
            throw new Exception("Failed to set overlay width: " + error.ToString());
        }
    }

    private void SetOverlayTransformAbsolute(ulong handle, UnityEngine.Vector3 position, UnityEngine.Quaternion rotation)
    {
        var rigidTransform = new SteamVR_Utils.RigidTransform(position, rotation);
        var matrix = rigidTransform.ToHmdMatrix34();
        var error = OpenVR.Overlay.SetOverlayTransformAbsolute(handle, ETrackingUniverseOrigin.TrackingUniverseStanding, ref matrix);
        if (error != EVROverlayError.None)
        {
            throw new Exception("Failed to set overlay transform: " + error.ToString());
        }
    }

    private void SetOverlayTransformRelative(ulong handle, uint deviceIndex, UnityEngine.Vector3 position, UnityEngine.Quaternion rotation)
    {
        var rigidTransform = new SteamVR_Utils.RigidTransform(position, rotation);
        var matrix = rigidTransform.ToHmdMatrix34();
        var error = OpenVR.Overlay.SetOverlayTransformTrackedDeviceRelative(handle, deviceIndex, ref matrix);
        if (error != EVROverlayError.None)
        {
            throw new Exception("Failed to set overlay transform: " + error.ToString());
        }
    }
}