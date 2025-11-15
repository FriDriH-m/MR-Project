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
        if (startPoint == null) { Debug.LogError("[Raycast] startPoint is NULL"); return; }
        if (_raycastManager == null) { Debug.LogError("[Raycast] ARRaycastManager is NULL"); return; }
        if (_anchorManager == null) { Debug.LogError("[Raycast] AnchorManager is NULL"); return; }
        if (_creatingObject == null) { Debug.LogWarning("[Raycast] Creating prefab is NULL Ч инстансить нечего"); }

        if (_planeManager != null)
            Debug.Log($"[Raycast] ARSession.state={ARSession.state}, planesTracked={_planeManager.trackables.count}");
        else
            Debug.Log("[Raycast] ARPlaneManager is NULL (счЄтчик плоскостей недоступен)");

        _hits.Clear();
        var ray = new Ray(startPoint.position, startPoint.forward);

        if (_raycastManager.Raycast(ray, _hits, TrackableType.PlaneWithinPolygon))
        {
            var h = _hits[0];
            var plane = h.trackable as ARPlane;
            Debug.Log($"[Raycast] Hit: dist={h.distance:F3}m, pos={h.pose.position}, rotEuler={h.pose.rotation.eulerAngles}, " +
                      $"trackableId={h.trackableId}, isPlane={(plane != null)}, planeAlign={(plane ? plane.alignment.ToString() : "n/a")}");

            Debug.Log("[Raycast] —павн попыткаЕ");
            var go = _anchorManager.CreateOnPlane(_creatingObject, h);
            if (go == null) Debug.LogWarning("[Raycast] Spawn failed (anchor or instantiate returned null)");
            else Debug.Log($"[Raycast] Spawn OK -> {go.name}");
        }
        else
        {
            
            int planes = _planeManager ? _planeManager.trackables.count : -1;
            Debug.Log($"[Raycast] No plane hit. planesTracked={planes}, mask=PlaneWithinPolygon");
        }
    }
    public void ShootRayWithoutAnchor(Transform startPoint)
    {
        _hits.Clear();
        var ray = new Ray(startPoint.position, startPoint.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 50f))
        {
            Instantiate(_creatingObject, hit.point, Quaternion.identity);
            if (hit.collider.gameObject.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.TakeDamage(5);
            }
        }
        else if (_raycastManager.Raycast(ray, _hits, TrackableType.PlaneWithinPolygon))
        {
            var h = _hits[0];
            Instantiate(_creatingObject, h.pose.position, Quaternion.identity);
        }
    }

    private void Update()
    {
    }
}