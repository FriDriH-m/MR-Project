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

        // ѕытаемс€ прив€затьс€ к плоскости
        Debug.Log($"[Anchor] Attach at pose pos={hit.pose.position}, rotEuler={hit.pose.rotation.eulerAngles}");

        ARAnchor anchor = null;

#if UNITY_XR_ARFOUNDATION_5_0_OR_NEWER
        if (!_anchorManager.TryAttachAnchor(plane, hit.pose, out anchor))
        {
            Debug.LogWarning("[Anchor] TryAttachAnchor returned FALSE Ч делаем свободный anchor");
        }
#else
        anchor = _anchorManager.AttachAnchor(plane, hit.pose);
        if (anchor == null)
        {
            Debug.LogWarning("[Anchor] AttachAnchor returned NULL Ч делаем свободный anchor");
        }
#endif

        // ‘ќЋЅЁ : свободный €корь (без прив€зки к плоскости)
        if (anchor == null)
        {
#if UNITY_XR_ARFOUNDATION_5_0_OR_NEWER
            if (!_anchorManager.TryAddAnchor(hit.pose, out anchor))
            {
                Debug.LogWarning("[Anchor] TryAddAnchor (loose) тоже не создал €корь");
                return null;
            }
#else
            var loose = new GameObject("LooseAnchor");
            loose.transform.SetPositionAndRotation(hit.pose.position, hit.pose.rotation);
            anchor = loose.AddComponent<ARAnchor>(); // менеджер подхватит компонент и создаст XR-anchor
            if (anchor == null)
            {
                Debug.LogWarning("[Anchor] Ќе удалось добавить компонент ARAnchor на свободный объект");
                Destroy(loose);
                return null;
            }
#endif
        }

        Debug.Log($"[Anchor] Anchor OK: go='{anchor.gameObject.name}' (parent for spawned prefab)");
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
