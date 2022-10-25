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
    [SerializeField] private int _health;
    private int _maxHealth;

    //private Unit _targetEnemy;
    public Unit _targetEnemy;

    [SerializeField] private float _distanceToAttack = 1f;

    private void Start() {
        _rendererUnit.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
        SetState(UnitState.Idle);
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

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, _distanceToAttack);
    }
#endif
}
