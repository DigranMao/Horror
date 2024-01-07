using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableObject : MonoBehaviour
{
    protected CodeLock _codeLock;
    public bool _canOpen = true;
    [SerializeField] private bool _isOpened;
    [SerializeField] protected float _openCloseTime = 1f;
    protected float openToCloseLerp;
  
    public virtual IEnumerator Open()
    {
        yield return null;
    }
    public virtual IEnumerator Close()
    {
        yield return null;
    }
    public void OpenOrClose()
    {
        if (!_canOpen)
        {
            if(_codeLock!=null)
                ZoomCameraToCodeLock();
            return;
        }

        _isOpened = !_isOpened;

        StopAllCoroutines();

        if (_isOpened)
        {
            StartCoroutine(Open());
        }
        else
        {
            StartCoroutine(Close());
        }
    }

    private void ZoomCameraToCodeLock()
    {
        _codeLock.SetCodeLockCameraAsMain();
    }

    public void Unlock()
    {
        _canOpen = true;
    }

    public void Lock(CodeLock codeLock = null)
    {
        _canOpen = false;

        if (codeLock != null)
            _codeLock = codeLock;
    }
}

