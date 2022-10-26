using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{

    [Range(1, 100)] [SerializeField] private int _amountUnits;
    [SerializeField] private GameObject _unitPrefab;
     private void Start()
    {
        for (int i = 0; i < _amountUnits; i++) {
            Vector3 randomPosition = new Vector3(Random.Range(-22f, 22f), 0f, Random.Range(-22f, 22f));
            Instantiate(_unitPrefab, randomPosition, Quaternion.identity);
        }
    }

    
}
