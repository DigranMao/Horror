using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RackCase : OpenableObject
{
   
    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    [SerializeField] private float _moveForwardBy = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        
        _startPosition = transform.position;
        _targetPosition = transform.position - transform.forward * _moveForwardBy;
    }

    public override IEnumerator Open()
    {
        while (openToCloseLerp < 1)
        {
            openToCloseLerp += Time.deltaTime / _openCloseTime;
            transform.position = Vector3.Lerp(_startPosition,_targetPosition, openToCloseLerp);
            yield return null;
        }
    }
    public override IEnumerator Close()
    {
        while (openToCloseLerp > 0)
        {
            openToCloseLerp -= Time.deltaTime / _openCloseTime;
            transform.position = Vector3.Lerp(_startPosition, _targetPosition, openToCloseLerp);
            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
    }
    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}
