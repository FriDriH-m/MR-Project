using System.Collections;
using UnityEngine;

public class FingerGun : MonoBehaviour
{
    [SerializeField] private RaycastManager _raycastManager;
    [SerializeField] private AudioSource _audio;
    private bool _isShooting = true;
    private Coroutine _corr;
    public void IsShooting(bool isShooting)
    {
        _isShooting = isShooting;        
    }
    private void Update()
    {
        if (_isShooting && _corr == null)
        {
            Debug.Log("Shooting");
            _corr = StartCoroutine(Shoot());
        }
    }
    private IEnumerator Shoot()
    {
        _raycastManager.ShootRayWithoutAnchor(transform);
        _audio.Play();
        yield return new WaitForSeconds(0.3f);
        _corr = null;
    }
}
