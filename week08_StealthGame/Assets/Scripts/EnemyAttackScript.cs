using System;
using System.Collections;
using UnityEngine;

public class EnemyAttackScript : MonoBehaviour
{
    [SerializeField] private Animator _animator = null;
    private bool _isAttacking = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && !_isAttacking)
        {
            PlayerScript player = other.gameObject.GetComponent<PlayerScript>();
            if (player != null)
            {
                StartCoroutine(AttackSequence(player));
            }
        }
    }

    private IEnumerator AttackSequence(PlayerScript player)
    {
        _isAttacking = true;

        if (_animator != null)
        {
            _animator.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(1.5f);

        if (player != null)
        {
            player.Respawn();
        }

        _isAttacking = false;
    }
}