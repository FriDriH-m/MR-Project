using UnityEngine;

public class StartButton : MonoBehaviour
{
    [SerializeField] private MatchManager _matchManager;
    private float time;
    private void Update()
    {
        time += Time.deltaTime;
        if (time >= 3)
        {
            _matchManager.Init();
            gameObject.SetActive(false);
        }
    }
}
