using System.Collections;
using UnityEngine;

public enum EnemyType
{
    TypeA,
    TypeB,
    TypeC,
    TypeD
}

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyType _type;
    [SerializeField] private int _weight;
    [SerializeField] private int _price;

    [Space]
    [SerializeField] private int _damage;
    [SerializeField] private EnemyMakeDamage _makeDamage;
    [Space]
    [SerializeField] private float _invulnerableTime = 0.5f;
    [SerializeField] private float _speedBoostPercent = 0.15f;

    [Space]
    [SerializeField] private Renderer _enemyRenderer;
    [SerializeField] private Material _deadMaterial, _defaultMaterial;
    [SerializeField] private ParticleSystem _deadEffect;
    [SerializeField] private CoinHeap _goldHeap;
    [SerializeField] private Animator _animator;

    public EnemyType Type => _type;
    public int Weight => _weight;
    public int Price => _price;

    private EnemyMove _enemyMove;
    private bool _invulnerable = false;

    private void Awake()
    {
        _enemyMove = GetComponent<EnemyMove>();
        _makeDamage.SetDamage(_damage);
    }

    private void OnEnable()
    {
        _makeDamage.enabled = true;
        _enemyMove.enabled = true;
        _animator.enabled = true;
        _enemyRenderer.enabled = true;
        _enemyRenderer.material = _defaultMaterial;
    }

    private void OnDisable()
    {
        _enemyMove.SetSpeedToDefault();
    }

    public void SetTarget(BasePlayer target)
    {
        _enemyMove.SetTarget(target);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    [ContextMenu("TakeDamage")]
    public void TakeDamage()
    {
        if (_invulnerable)
            return;

        if (OrderManager.Order == (int)Type)
        {
            Die();
        }
        else
        {
            Boost();
        }
    }

    [ContextMenu("Die")]
    private void Die()
    {

        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        _enemyMove.enabled = false;
        _animator.enabled = false;
        _makeDamage.enabled = false;

        _enemyRenderer.material = _deadMaterial;

        yield return new WaitForSeconds(0.5f);

        _enemyRenderer.enabled = false;
        _deadEffect.Play();

        yield return new WaitForSeconds(0.2f);

        //EventManager.SendEnemyReturnToPool(this);
        EventManager.SendEnemyKilled(this);

        CoinHeap coinHeap = Instantiate(_goldHeap, transform.position, Quaternion.identity);
        coinHeap.SetGoldCount(_price);

        yield return null;
    }

    private void Boost()
    {
        StartCoroutine(BoostCoroutine());
    }

    IEnumerator BoostCoroutine()
    {
        _invulnerable = true;

        yield return new WaitForSeconds(_invulnerableTime);

        _enemyMove.RaiseSpeed(_speedBoostPercent);

        _invulnerable = false;
    }
}
