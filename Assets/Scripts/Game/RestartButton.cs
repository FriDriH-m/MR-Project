using UnityEngine;


public class RestartButton : MonoBehaviour
{
    [SerializeField] private MatchManager _matchManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _matchManager.Init();
            gameObject.SetActive(false);
        }
    }
}
