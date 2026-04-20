using UnityEngine;
using Valve.VR;
using System;

namespace OpenVRUtils
{
    public static class System
    {
        public static void InitOpenVR()
        {
            if (OpenVR.System != null) return;

            var initError = EVRInitError.None;
            OpenVR.Init(ref initError, EVRApplicationType.VRApplication_Overlay);
            CheckError("Failed to initialize OpenVR", initError);
        }

        public static void ShutdownOpenVR()
        {
            if (OpenVR.System == null) return;

            OpenVR.Shutdown();
        }

        public static void CheckError(string message, EVRInitError error)
        {
            if (error != EVRInitError.None)
            {
                throw new Exception(message + ": " + error.ToString());
            }
        }
    }

    public static class Overlay
    {
        public static ulong CreateOverlay(string key, string name)
        {
            var handle = OpenVR.k_ulOverlayHandleInvalid;
            var error = OpenVR.Overlay.CreateOverlay(key, name, ref handle);
            CheckOverlayError("Failed to create overlay", error);
            return handle;
        }

        public static void SetOverlayRenderTexture(ulong handle, RenderTexture renderTexture)
        {
            if (!renderTexture.IsCreated()) return;
            
            var nativeTexturePtr = renderTexture.GetNativeTexturePtr();
            var texture = new Texture_t
            {
                handle = nativeTexturePtr,
                eType = ETextureType.DirectX,
                eColorSpace = EColorSpace.Auto
            };
            var error = OpenVR.Overlay.SetOverlayTexture(handle, ref texture);
            CheckOverlayError("Failed to set overlay texture", error);
        }

        public static void SetOverlayFromFile(ulong handle, string path)
        {
            var error = OpenVR.Overlay.SetOverlayFromFile(handle, path);
            CheckOverlayError("Failed to set overlay image", error);
        }

        public static void ShowOverlay(ulong handle)
        {
            var error = OpenVR.Overlay.ShowOverlay(handle);
            CheckOverlayError("Failed to show overlay", error);
        }

        public static void SetOverlaySize(ulong handle, float size)
        {
            var error = OpenVR.Overlay.SetOverlayWidthInMeters(handle, size);
            CheckOverlayError("Failed to set overlay width", error);
        }

        public static void SetOverlayTransformAbsolute(ulong handle, UnityEngine.Vector3 position, UnityEngine.Quaternion rotation)
        {
            var rigidTransform = new SteamVR_Utils.RigidTransform(position, rotation);
            var matrix = rigidTransform.ToHmdMatrix34();
            var error = OpenVR.Overlay.SetOverlayTransformAbsolute(handle, ETrackingUniverseOrigin.TrackingUniverseStanding, ref matrix);
            CheckOverlayError("Failed to set overlay transform", error);
        }

        public static void SetOverlayTransformRelative(ulong handle, uint deviceIndex, UnityEngine.Vector3 position, UnityEngine.Quaternion rotation)
        {
            var rigidTransform = new SteamVR_Utils.RigidTransform(position, rotation);
            var matrix = rigidTransform.ToHmdMatrix34();
            var error = OpenVR.Overlay.SetOverlayTransformTrackedDeviceRelative(handle, deviceIndex, ref matrix);
            CheckOverlayError("Failed to set overlay transform", error);
        }

        public static void DestroyOverlay(ulong handle)
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

        public static void CheckOverlayError(string message, EVROverlayError error)
        {
            if (error != EVROverlayError.None)
            {
                throw new Exception(message + ": " + error.ToString());
            }
        }
    }
}
