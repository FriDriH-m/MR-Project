using System.Collections;
using UnityEngine;

public class FingerGun : MonoBehaviour
{
    [SerializeField] private RaycastManager _raycastManager;
    private bool _isShooting;
    private Coroutine _corr;
    public void IsShooting(bool isShooting)
    {
        _isShooting = isShooting;        
    }
    private void Update()
    {
        if (_isShooting && _corr == null)
        {
            _corr = StartCoroutine(Shoot());
        }
    }
    private IEnumerator Shoot()
    {
        _raycastManager.ShootRayWithoutAnchor(transform);
        yield return new WaitForSeconds(0.3f);
        _corr = null;
    }
}
