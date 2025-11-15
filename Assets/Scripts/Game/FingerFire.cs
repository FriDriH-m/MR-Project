using Unity.VisualScripting;
using UnityEngine;

public class FingerFire : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;
    private bool _play;
    public void PlaySound(bool isPlaying)
    {
        _play = isPlaying;
    }
    private void Update()
    {
        if (_play)
        {
            _audio.Play();
        }
        else _audio.Stop();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.TakeDamage(20 * Time.deltaTime);
        }
    }
}
