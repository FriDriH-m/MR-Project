using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _healthCanvas;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _health;
    private float _maxHealth;
    private Transform _target;
    private NavMeshAgent _agent;    
    public event Action<Enemy> IsDead;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _maxHealth = _health;
    }
    private void Start()
    {
        //StartCoroutine(Minus());
        _animator.SetFloat("Speed", 1.6f);
    }
    public void SetTarget(Transform target)
    {
        _target = target;
    }
    public void TakeDamage(float damage)
    {
        if (_health - damage > 0)
        {
            _health -= damage;
            _healthCanvas.transform.localScale = new Vector3(Mathf.InverseLerp(0, _maxHealth, _health - damage), 1f, 1f);
        }
        else 
        {
            Debug.Log("Умер");
            Die();
            StopAllCoroutines();
        }
    }
    private void Die()
    {
        IsDead?.Invoke(this);
        Destroy(gameObject);
    }
    private void Update()
    {
        _agent.destination = _target.position;
        if (Vector3.Distance(_target.position, transform.position) < 1f)
        {
            _agent.speed = 0;
            _animator.SetFloat("Speed", 0f);
            _animator.SetBool("Attack", true);
        }
        else
        {
            _agent.speed = 0.7f;
            _animator.SetFloat("Speed", 1.6f);
            _animator.SetBool("Attack", false);
        }
    }
    private IEnumerator Minus()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            TakeDamage(10f);
        }
        
    }
}
