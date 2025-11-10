using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class AnchorManager : MonoBehaviour
{
    [SerializeField] private ARAnchorManager _anchorManager;

    public GameObject CreateOnPlane(GameObject prefab, ARRaycastHit hit)
    {
        if (_anchorManager == null) { Debug.LogError("[Anchor] ARAnchorManager is NULL"); return null; }
        if (prefab == null) { Debug.LogWarning("[Anchor] Prefab is NULL Ч инстансить нечего"); }

        var plane = hit.trackable as ARPlane;
        if (plane == null)
        {
            Debug.LogWarning($"[Anchor] trackable is not ARPlane (id={hit.trackableId}) Ч веро€тно FeaturePoint/NULL");
            return null;
        }

        Debug.Log($"[Anchor] Plane: id={plane.trackableId}, alignment={plane.alignment}, trackingState={plane.trackingState}");

        int hops = 0;
        while (plane.subsumedBy != null) { plane = plane.subsumedBy; hops++; }
        if (hops > 0) Debug.Log($"[Anchor] Plane was subsumed, hops={hops}, newId={plane.trackableId}");

        Debug.Log($"[Anchor] AttachAnchor at pose pos={hit.pose.position}, rotEuler={hit.pose.rotation.eulerAngles}");
        var anchor = _anchorManager.AttachAnchor(plane, hit.pose);
        if (anchor == null)
        {
            Debug.LogWarning("[Anchor] AttachAnchor returned NULL (плохой трекинг/невалидна€ поза/plane)");
            return null;
        }

        Debug.Log($"[Anchor] Anchor OK: go='{anchor.gameObject.name}'");
        var go = Instantiate(prefab, hit.pose.position, hit.pose.rotation, anchor.transform);
        if (go == null)
        {
            Debug.LogWarning("[Anchor] Instantiate returned NULL");
            return null;
        }

        Debug.Log($"[Anchor] Spawned '{go.name}' under anchor '{anchor.gameObject.name}'");
        return go;
    }
}