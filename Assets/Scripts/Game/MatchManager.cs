using NUnit.Framework;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public interface IState
{
    public void Start(MatchManager manager);
    public void Stop(MatchManager manager);
    public void Current(MatchManager manager);
}

public class FirstState : IState
{
    public void Current(MatchManager manager)
    {
        if (manager.CheckToResults()) manager.SwitchState();
    }

    public void Start(MatchManager manager)
    {
        manager.SpawnEnemies(3);
    }

    public void Stop(MatchManager manager)
    {
        manager.ClearList();
    }
}
public class SecondState : IState
{
    public void Current(MatchManager manager)
    {
        if (manager.CheckToResults()) manager.SwitchState();
    }
    public void Start(MatchManager manager)
    {
        manager.SpawnEnemies(5);
    }
    public void Stop(MatchManager manager)
    {
        manager.ClearList();
    }
}
public class ThirdState : IState
{
    public void Current(MatchManager manager)
    {
        if (manager.CheckToResults()) manager.SwitchState();
    }
    public void Start(MatchManager manager)
    {
        manager.SpawnEnemies(7);
    }
    public void Stop(MatchManager manager)
    {
        manager.ClearList();
    }
}

public class MatchManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _centerObject;
    [SerializeField] private float _spawnRadius = 10f;
    [SerializeField] private GameObject _winCanvas;
    private List<Enemy> _enemies;
    private List<IState> _states;
    private int _currentState = 0;
    private IState _firstState = new FirstState();
    private IState _secondState = new SecondState();    
    private IState _thirdState = new ThirdState();

    private void Start()
    {
        _enemies = new List<Enemy>();
        _states = new List<IState> { _firstState, _secondState, _thirdState };
        _states[_currentState].Start(this);
    }
    private void Update()
    {
        Debug.Log(_currentState + " " + _states.Count);
        _states[_currentState].Current(this);
    }

    private void RemoveEnemyFromList(Enemy enemy)
    {
        _enemies.Remove(enemy);
    }
    public void ClearList()
    {
        _enemies.Clear();
    }
    public bool CheckToResults()
    {
        if (_enemies.Count == 0) return true;
        else return false;
    }
    public void SwitchState()
    {        
        if (_currentState + 1 >= _states.Count) 
        {
            _winCanvas.SetActive(true);
            return;
        }
        _currentState++;
        _states[_currentState].Start(this);
    }
    public void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject enemy = Instantiate(_enemyPrefab, Vector3.zero, Quaternion.identity);
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            _enemies.Add(enemyComponent);
            enemyComponent.SetTarget(_centerObject);
            enemyComponent.IsDead += RemoveEnemyFromList;
            Vector3 spawnPos = GetSpawnPosition();
            NavMesh.SamplePosition(spawnPos, out var hit, 50f, NavMesh.AllAreas);
            enemy.transform.position = hit.position;
        }
    }

    private Vector3 GetSpawnPosition()
    {
        // Кольцо: от _spawnRadius+2 до _spawnRadius+5
        float distance = _spawnRadius + Random.Range(2f, 5f);
        Vector2 dir2D = Random.insideUnitCircle;
        if (dir2D.sqrMagnitude < 1e-6f) dir2D = Vector2.right; // защита от нуля
        dir2D.Normalize();

        Vector3 offset = new Vector3(dir2D.x, 0f, dir2D.y) * distance;
        return _centerObject.position + offset;
    }
}
