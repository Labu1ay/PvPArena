using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum UnitState {
    Idle,
    WalkToEnemy,
    Attack
}
public class Unit : MonoBehaviour {
    public UnitState CurrentUnitState;

    [SerializeField] private Renderer _rendererUnit;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private Unit _targetEnemy;
    private float _distanceToAttack = 2f;

    [SerializeField] private GameObject _healthBarPrefab;
    private HealthBar _healthBar;

    [SerializeField] private Animator _sword;


    [Space(10)]
    [Header("Unit Value")]
    [Space(10)]
    [SerializeField] private int _health;
    private int _maxHealth;
    [SerializeField] private float _attackPeriod;
    private float _timer;
    [SerializeField] private int _damage;

    private void Start() {
        _rendererUnit.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
        SetState(UnitState.Idle);
        SetInitialValues();
        CreateHealthBar();

    }
    private void Update() {
        if (CurrentUnitState == UnitState.Idle) {
            FindClosestUnit();
            if (_targetEnemy) {
                SetState(UnitState.WalkToEnemy);
            }
        } else if (CurrentUnitState == UnitState.WalkToEnemy) {
            FindClosestUnit();
            if (_targetEnemy) {
                _navMeshAgent.SetDestination(_targetEnemy.transform.position);
                float distance = Vector3.Distance(transform.position, _targetEnemy.transform.position);
                if (distance < _distanceToAttack) {
                    SetState(UnitState.Attack);
                    _targetEnemy.SetState(UnitState.Attack);
                }
            } else {
                SetState(UnitState.Idle);
            }
        } else if (CurrentUnitState == UnitState.Attack) {
            if (_targetEnemy) {
                _timer += Time.deltaTime;
                if (_timer >= _attackPeriod) {
                    _timer = 0f;
                    _sword.SetTrigger("Attack");
                    _targetEnemy.TakeDamage(_damage);
                   
                }
            } else {
                SetState(UnitState.Idle);
            }
        }
    }

    void SetState(UnitState unitState) {
        CurrentUnitState = unitState;

        if (CurrentUnitState == UnitState.Idle) {
            _navMeshAgent.SetDestination(transform.position);
        } else if (CurrentUnitState == UnitState.WalkToEnemy) {

        } else if (CurrentUnitState == UnitState.Attack) {
            _timer = 0f;
        }
    }

    void FindClosestUnit() {
        Unit[] allUnits = FindObjectsOfType<Unit>();
        float minDistance = Mathf.Infinity;
        Unit closestUnit = null;

        for (int i = 0; i < allUnits.Length; i++) {
            if (allUnits[i].CurrentUnitState == UnitState.Attack) {
                continue;
            }
            float distance = Vector3.Distance(transform.position, allUnits[i].transform.position);
            if (distance == 0f) {
                continue;
            }
            if (distance < minDistance) {
                minDistance = distance;

                closestUnit = allUnits[i];
            }
        }
        _targetEnemy = closestUnit;
    }
    public void TakeDamage(int damageValue) {
        _health -= damageValue;
        _healthBar.SetHealth(_health, _maxHealth);
        if (_health <= 0) {
            if (_healthBar) {
                Destroy(_healthBar.gameObject);
            }
            Destroy(gameObject);
        }

    }
    private void SetInitialValues() {
        _health = Random.Range(7, 15);
        _maxHealth = _health;
        _attackPeriod = Random.Range(0.5f, 1.3f);
        _damage = Random.Range(1, 4);
    }

    private void CreateHealthBar() {
        GameObject healthBar = Instantiate(_healthBarPrefab);
        _healthBar = healthBar.GetComponent<HealthBar>();
        _healthBar.Setup(transform);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, _distanceToAttack);
    }
#endif
}
