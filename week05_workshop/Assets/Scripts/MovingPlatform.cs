using UnityEngine;
using DG.Tweening;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform _movingPlatform = null;
    [SerializeField] private Transform _targetTransform = null;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _waitDuration = 2f;

    private Coroutine _movingCoroutine = null;
    private bool _moveToTarget = true;

    private void OnEnable()
    {
        ClearCorouting();
        if (_moveToTarget)
        {
            MoveToTarget();
        }
        else
        {
            MoveToStart();
        }
    }

    private void OnDisable()
    {
        ClearCorouting();
    }

    private void ClearCorouting()
    {
        if ( _movingCoroutine != null )
        {
            StopCoroutine( _movingCoroutine );
        }
        _movingCoroutine = null;
    }

    private void MoveToTarget()
    {
        _moveToTarget = true;
        ClearCorouting();
        _movingCoroutine = StartCoroutine(MovePlatform());
    }

    private void MoveToStart()
    {
        _moveToTarget = false;
        ClearCorouting();
        _movingCoroutine = StartCoroutine(MovePlatform());
    }

    IEnumerator MovePlatform()
    {
        while(true)
        {
            Vector3 targetPos = (_moveToTarget) ? _targetTransform.position : transform.position;
            //pause here for a frame, next frame start next line
            yield return null;

            float duration = (_movingPlatform.transform.position - targetPos).magnitude / _moveSpeed;
            Tween moveTween = _movingPlatform.DOMove(targetPos, duration);
            yield return new WaitForSeconds(duration);

            moveTween.Kill();
            yield return new WaitForSeconds(_waitDuration);
            _moveToTarget = !_moveToTarget;
        }
    }
}
