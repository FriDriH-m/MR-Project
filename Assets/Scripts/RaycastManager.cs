using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class RaycastManager : MonoBehaviour
{
    [SerializeField] private ARRaycastManager _raycastManager;
    [SerializeField] private ARPlaneManager _planeManager;
    [SerializeField] private AnchorManager _anchorManager;
    [SerializeField] private GameObject _creatingObject;

    private readonly List<ARRaycastHit> _hits = new();

    public void ShootRay(Transform startPoint)
    {
        _hits.Clear();
        var ray = new Ray(startPoint.position, startPoint.forward);

        // Только реальная геометрия плоскостей
        if (_raycastManager.Raycast(ray, _hits, TrackableType.AllTypes))
        {
            Debug.Log("Спавн");
            var go = _anchorManager.CreateOnPlane(_creatingObject, _hits[0]);
            if (go == null) Debug.LogWarning("Spawn failed (anchor null)");
        }
        else
        {
            Debug.Log($"No plane hit. planes tracked: ");
        }
    }
}
