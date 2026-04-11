using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using System;

public class Overlay : MonoBehaviour
{
    private ulong overlayHandle = OpenVR.k_ulOverlayHandleInvalid;
    // Start is called before the first frame update
    void Start()
    {

        InitOpenVR();

        var key = "PhoneTrackVR.Overlay";
        var name = "PhoneTrackVR Overlay";
        var error = OpenVR.Overlay.CreateOverlay(key, name, ref overlayHandle);
        if (error != EVROverlayError.None)
        {
            throw new Exception("Failed to create overlay: " + error.ToString());
        }

    }

    private void OnApplicationQuit()
    {
        if (overlayHandle != OpenVR.k_ulOverlayHandleInvalid)
        {
            var error = OpenVR.Overlay.DestroyOverlay(overlayHandle);
            if (error != EVROverlayError.None)            {
                Debug.LogError("Failed to destroy overlay: " + error.ToString());
                throw new Exception("Failed to destroy overlay: " + error.ToString());
            }
        }
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
}
