using System.Collections;
using UnityEngine;

public class EnemyMakeDamage : MonoBehaviour
{
    [SerializeField] private float _attackCooldown = 0.5f;

    private int _damage;
    private bool _damaging = true;
    private Coroutine _damageCoroutine;
    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<BasePlayer>();
        if(player)
        {
            _damaging = true;
            _damageCoroutine = StartCoroutine(DamageCoroutine(player));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<BasePlayer>();
        if (player)
        {
            _damaging = false;
            StopCoroutine(_damageCoroutine);
        }
    }
    private IEnumerator DamageCoroutine(BasePlayer player)
    {
        while(_damaging)
        {
            yield return new WaitForSeconds(_attackCooldown);

            player.TakeDamage(_damage);
        }
    }
}
