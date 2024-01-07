using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _raycastDistance = 2f;
    [SerializeField] private LayerMask _raycastLayerMask;
    private DraggableObject _currentlyDraggedObject = null;
    [SerializeField] private float _draggableObjectDistance = 1f;

    private void FixedUpdate()
    {
        if (_currentlyDraggedObject != null)
        {
            Vector3 targetPosition = _mainCamera.transform.position + _mainCamera.transform.forward * _draggableObjectDistance;
            _currentlyDraggedObject.SetTargetPosition(targetPosition);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit hit;
            if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit, _raycastDistance, _raycastLayerMask))
            {
                if (hit.collider.TryGetComponent(out OpenableObject openable))
                {
                    openable.OpenOrClose();
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit, _raycastDistance,
                LayerMask.GetMask("DraggableObject")))
            {
                if (hit.collider.TryGetComponent(out DraggableObject draggableObject))
                {
                    draggableObject.StartFollowingObject();
                    _currentlyDraggedObject = draggableObject;
                }
            }
        }

        
       
        if (Input.GetMouseButtonUp(0))
        {
            if(_currentlyDraggedObject != null)
            {
                _currentlyDraggedObject.StopFollowingObject();
                _currentlyDraggedObject = null;
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(_mainCamera.transform.position, _mainCamera.transform.forward * _raycastDistance);
    }
}
