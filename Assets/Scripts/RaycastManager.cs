using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class RaycastManager : MonoBehaviour
{
    [SerializeField] private ARRaycastManager _raycastManager;
    [SerializeField] private AnchorManager _anchorManager;
    [SerializeField] private GameObject _creatingObject;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public async Task ShootRayAsync(Transform startPoint)
    {
        Ray ray = new Ray(startPoint.position, startPoint.forward);
        if (_raycastManager.Raycast(ray, hits, TrackableType.AllTypes))
        {
            await _anchorManager.CreateAnchoredObject(_creatingObject, hits[0]);
        }
    }
}
