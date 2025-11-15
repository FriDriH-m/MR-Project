using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private MatchManager m_MatchManager;
    [SerializeField] private GameObject _deathCanvas;
    [SerializeField] private GameObject _restartButton;
    [SerializeField] private GameObject _playerHealth;
    private int _health = 10000;
    private bool _isImmune = false;
    private void OnTriggerEnter(Collider other)
    {
        if (_health - 10 <= 0) StartCoroutine(PlayerDeath(this));
        if (other.CompareTag("Punch") && !_isImmune)
        {
            _playerHealth.transform.localScale = new Vector3(Mathf.InverseLerp(0, 100, _health - 10), 1, 1);
            _health -= 10;
            _isImmune = true;
            StartCoroutine(NonHitable());
        }
    }
    private IEnumerator NonHitable()
    {
        yield return new WaitForSeconds(0.5f);
        _isImmune = false;
    }
    private IEnumerator Restart()
    {
        yield return new WaitForSeconds(3f);
        _health = 100;
        _restartButton.SetActive(false);
        m_MatchManager.Restart();
    }

    private IEnumerator PlayerDeath(MonoBehaviour mono)
    {
        _deathCanvas.SetActive(true);
        yield return new WaitForSeconds(2f);
        _deathCanvas.SetActive(false);
        _restartButton.SetActive(true);
        //mono.StartCoroutine(Restart());

    }
}
