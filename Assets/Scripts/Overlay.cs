using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using System;
using System.Runtime.InteropServices;
using System.Numerics;

public class Overlay : MonoBehaviour
{
    private ulong overlayHandle = OpenVR.k_ulOverlayHandleInvalid;
    // Start is called before the first frame update
    void Start()
    {

        InitOpenVR();

        overlayHandle = CreateOverlay("PhoneTrackVROverlayKey", "PhoneTrackVR");

        // draw sample image
        var filePath = Application.streamingAssetsPath + "/vrchat.png";
        var error = OpenVR.Overlay.SetOverlayFromFile(overlayHandle, filePath);
        if (error != EVROverlayError.None)        {
            throw new Exception("Failed to set overlay image: " + error);
        }


        //SetOverlayFromFile(overlayHandle, filePath);

        //SetOverlaySize(overlayHandle, 0.5f);

        //var position = new  UnityEngine.Vector3(0, 2, 3);
        //var rotation = UnityEngine.Quaternion.Euler(0, 0, 45);
        //SetOverlayTransformAbsolute(overlayHandle, position, rotation);

        ShowOverlay(overlayHandle);

    }

    private void OnApplicationQuit()
    {
        DestroyOverlay(overlayHandle);
    }

    // Update is called once per frame
    void Update()
    {
        
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

    // private void SetOverlayTransformAbsolute(ulong handle, UnityEngine.Vector3 position, UnityEngine.Quaternion rotation)
    // {
    //     var rigidTransform = new SteamVR_Utils.RigidTransform(position, rotation);
    //     var matrix = rigidTransform.ToHmdMatrix34();
    //     var error = OpenVR.Overlay.SetOverlayTransformAbsolute(handle, ETrackingUniverseOrigin.TrackingUniverseStanding, ref matrix);
    //     if (error != EVROverlayError.None)
    //     {
    //         throw new Exception("Failed to set overlay transform: " + error.ToString());
    //     }
    // }
}