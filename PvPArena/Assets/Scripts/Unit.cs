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
    [SerializeField] private int _health = 5;
    private int _maxHealth;

    //private Unit _targetEnemy;
    public Unit _targetEnemy;

    private float _attackPeriod = 1f;
    private float _timer;

    [SerializeField] private float _distanceToAttack = 1f;

    [SerializeField] private GameObject _healthBarPrefab;
    private HealthBar _healthBar;

    private void Start() {
        _rendererUnit.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
        SetState(UnitState.Idle);

        _maxHealth = _health;
        GameObject healthBar = Instantiate(_healthBarPrefab);
        _healthBar = healthBar.GetComponent<HealthBar>();
        _healthBar.Setup(transform);
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
                if(distance < _distanceToAttack) {
                    SetState(UnitState.Attack);
                    _targetEnemy.SetState(UnitState.Attack);
                }
            } else {
                SetState(UnitState.Idle);
            }
        } else if (CurrentUnitState == UnitState.Attack) {
            _timer += Time.deltaTime;
            if(_timer >= _attackPeriod) {

            }
        }
    }

    void SetState(UnitState unitState) {
        CurrentUnitState = unitState;

        if (CurrentUnitState == UnitState.Idle) {
            _navMeshAgent.SetDestination(transform.position);
        } else if (CurrentUnitState == UnitState.WalkToEnemy) {
            
        } else if (CurrentUnitState == UnitState.Attack) {

        }
    }

    void FindClosestUnit() {
        Unit[] allUnits = FindObjectsOfType<Unit>();
        float minDistance = Mathf.Infinity;
        Unit closestUnit = null;

        for (int i = 0; i < allUnits.Length; i++) {
            if(allUnits[i].CurrentUnitState == UnitState.Attack) {
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
        //

    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, _distanceToAttack);
    }
#endif
}
