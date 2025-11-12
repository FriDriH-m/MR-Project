using Unity.VisualScripting;
using UnityEngine;

public class FingerFire : MonoBehaviour
{
    private void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.TakeDamage(10 * Time.deltaTime);
        }
    }
}
