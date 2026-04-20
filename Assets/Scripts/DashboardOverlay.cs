using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using OpenVRUtils;

public class DashboardOverlay : MonoBehaviour
{
    private ulong dashboardHandle = OpenVR.k_ulOverlayHandleInvalid;
    private ulong thumbnailHandle = OpenVR.k_ulOverlayHandleInvalid;

    // Start is called before the first frame update
    void Start()
    {
        OpenVRUtils.System.InitOpenVR();

        var error = OpenVR.Overlay.CreateDashboardOverlay("PhoneTrackVRDashboardKey", "PhoneTrackVR Dashboard", ref dashboardHandle, ref thumbnailHandle);
        OpenVRUtils.Overlay.CheckOverlayError("Failed to create dashboard overlay", error);

        // draw icon
        var filePath = Application.streamingAssetsPath + "/test.jpg";
        OpenVRUtils.Overlay.SetOverlayFromFile(thumbnailHandle, filePath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        OpenVRUtils.System.ShutdownOpenVR();
    }

    private void OnApplicationQuit()
    {
        OpenVRUtils.Overlay.DestroyOverlay(dashboardHandle);
    }
}
