using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class BoundingBoxFollower : MonoBehaviour
{
    private ARBoundingBox _boundingBox;

    private void Awake()
    {
        _boundingBox = GetComponentInParent<ARBoundingBox>();
    }

    private void Update()
    {
        if (_boundingBox == null) return;

        Transform targetTransform = _boundingBox.transform;
        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;
        transform.localScale = _boundingBox.size;
    }
}
