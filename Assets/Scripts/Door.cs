using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : OpenableObject
{
    private Vector3 _startRotation;
    private Vector3 _targetRotation;
    [SerializeField] private float _rotateByDegrees = -90f;
 
    private void Start()
    {
        _startRotation = transform.rotation.eulerAngles;
        _targetRotation = _startRotation + Vector3.up * _rotateByDegrees;
    }
    
    public override IEnumerator Open()
    {
        while (openToCloseLerp < 1)
        {
            openToCloseLerp += Time.deltaTime / _openCloseTime;
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(_startRotation), Quaternion.Euler(_targetRotation), openToCloseLerp);
            yield return null;
        }
    }
    public override IEnumerator Close()
    {
        while (openToCloseLerp > 0)
        {
            openToCloseLerp -= Time.deltaTime / _openCloseTime;
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(_startRotation), Quaternion.Euler(_targetRotation), openToCloseLerp);
            yield return null;
        }
    }
}
