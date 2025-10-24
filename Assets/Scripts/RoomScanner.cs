using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.OpenXR.Features.Meta;

public class RoomScanner : MonoBehaviour
{
    private ARSession _arSession;

    private void Awake()
    {
        _arSession = FindAnyObjectByType<ARSession>();
        ScanRoom();
    }
    private void ScanRoom()
    {
        MetaOpenXRSessionSubsystem subsystem = _arSession?.subsystem as MetaOpenXRSessionSubsystem;
        if (subsystem != null)
        {
            bool ok = subsystem.TryRequestSceneCapture();
        }
    }
}