using System;
using UnityEngine;

public class BasePlayer : MonoBehaviour
{
    [SerializeField] private FixedJoystick _moveJoystick;

    [SerializeField] private AbilitySystem _abilitySystem = null;

    [SerializeField] private Animator _animatorController = null;

    [Space]
    [Range(0.5f, 5f)]
    [SerializeField] private float _speed = 1f;

    [Range(0.1f, 100f)]
    [SerializeField] private float _speedRotation = 1f;

    public event Action OnUseAbility;

    public Vector3 Position => transform.position;

    private CharacterController _charController = null;

    private float horInput = 0f;
    private float vertInput = 0f;


    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        _abilitySystem.Initialize(this);
        EventManager.OnEnemyKilled.AddListener(EnemyKilled);
    }

    private void OnDisable()
    {
        _abilitySystem.Deinitialize();
    }

    void Update()
    {
        if (!GameParameters.GameRunning) return;

        Rotation();
        Movement();

        _animatorController.SetFloat("Vertical", vertInput);
        _animatorController.SetFloat("Horizontal", horInput);
    }

    #region Movement

    private void Rotation()
    {
        if (_moveJoystick.Horizontal != 0 || _moveJoystick.Vertical != 0)
        {
            var hitPosition = new Vector3(_moveJoystick.Direction.x, transform.position.y, _moveJoystick.Direction.y);

            var rotation = Quaternion.LookRotation(hitPosition);

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _speedRotation * Time.deltaTime);
        }        
    }
    private void Movement()
    {
        horInput = _moveJoystick.Horizontal;
        vertInput = _moveJoystick.Vertical;

        Vector3 move = new Vector3(horInput, 0, vertInput);

        Vector3 gravity = Vector3.zero;

        gravity = (transform.position.y > 0) ? Vector3.down : Vector3.zero;

        
        _charController.Move((move + gravity) * Time.deltaTime * _speed);
    }

    #endregion
    public void Shoot()
    {
        UseAbility();
    }
    private void UseAbility()
    {
        TakeDamage(_abilitySystem.AbilityPrice);
        OnUseAbility?.Invoke();
    }

    public void TakeDamage(int damage)
    {
        Score.GoldDecrease(damage);
    }

    private void EnemyKilled(Enemy enemy)
    {
        AddGold(enemy.Price);
    }

    public static void AddGold(int value)
    {
        Score.GoldIncrease(value);
    }
}
 