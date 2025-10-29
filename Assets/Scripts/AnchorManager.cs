using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class AnchorManager : MonoBehaviour
{
    [SerializeField] GameObject _cubeExample;
    [SerializeField] private ARAnchorManager _anchorManager;

    private void Awake()
    {
        
    }
    private void Start()
    {
        var pose = new Pose(Vector3.zero, Quaternion.identity);
        var awaiter = _anchorManager.TryAddAnchorAsync(pose).GetAwaiter();
        if (awaiter.IsCompleted)
        {
            ARAnchor anchor = awaiter.GetResult().value;
            Instantiate(_cubeExample, anchor.transform.position, anchor.transform.rotation, anchor.transform);
        }
    }
    public async Task<GameObject> CreateAnchoredObject(GameObject obj, ARRaycastHit hit)
    {
        try
        {
            if (hit.trackable is ARPlane plane)
                return CreateAnchoredObjectOnPlane(plane, obj, hit.pose);

            return await CreateAnchoredObjectAsync(obj, hit);
        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex);
            return null;
        }
    }
    private async Task<GameObject> CreateAnchoredObjectAsync(GameObject obj, ARRaycastHit hit)
    {
        var pose = hit.pose;
        var awaiter = _anchorManager.TryAddAnchorAsync(pose).GetAwaiter();
        if (awaiter.IsCompleted)
        {
            ARAnchor anchor = awaiter.GetResult().value;
            return Instantiate(obj, anchor.transform.position, anchor.transform.rotation, anchor.transform);
        }
        else return null;
    }
    private GameObject CreateAnchoredObjectOnPlane(ARPlane plane, GameObject obj, Pose pose)
    {
        var anchor = _anchorManager.AttachAnchor(plane, pose);
        if (anchor != null)
        {
            return Instantiate(obj, pose.position, pose.rotation, anchor.transform);
        }
        else return null;
    }
}
