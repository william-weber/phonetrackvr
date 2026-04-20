using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using System;
using System.Runtime.InteropServices;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using OpenVRUtils;

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

        OpenVRUtils.System.InitOpenVR();

        overlayHandle = OpenVRUtils.Overlay.CreateOverlay("PhoneTrackVROverlayKey", "PhoneTrackVR");

        OpenVRUtils.Overlay.SetOverlaySize(overlayHandle, size);

        OpenVRUtils.Overlay.ShowOverlay(overlayHandle);

    }

    // Update is called once per frame
    void Update()
    {
        var leftControllerIndex = OpenVR.System.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.LeftHand);
        if (leftControllerIndex != OpenVR.k_unTrackedDeviceIndexInvalid)
        {
            var position = new UnityEngine.Vector3(x, y, z);
            var rotation = UnityEngine.Quaternion.Euler(rotationX, rotationY, rotationZ);
            OpenVRUtils.Overlay.SetOverlayTransformRelative(overlayHandle, leftControllerIndex, position, rotation);
        }

        OpenVRUtils.Overlay.SetOverlayRenderTexture(overlayHandle, renderTexture);
    }

    private void OnDestroy()
    {
        if (OpenVR.System == null) return; // Not initialized
        OpenVRUtils.System.ShutdownOpenVR();
    }

    private void OnApplicationQuit()
    {
        OpenVRUtils.Overlay.DestroyOverlay(overlayHandle);
    }
}