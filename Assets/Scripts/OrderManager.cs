using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private float _cooldownChangeOrder = 5f;

    public static int Order { get; private set; }

    private float _currentCooldown;
    private static int _orderCount;

    private void Awake()
    {
        _orderCount = Enum.GetNames(typeof(EnemyType)).Length;
        _currentCooldown = _cooldownChangeOrder;
    }

    private void OnEnable()
    {
        //SetStartOrder();
        EventManager.OnEnemyKilled.AddListener(SetNewOrder);
    }

    private void Start()
    {
        ChangeOrder();
        //StartCoroutine(ChangeOrderPeriodic());
    }

    //public static void SetStartOrder()
    //{
    //    Order = Random.Range(0, _maxOrder);
    //    EventManager.SendOrderChanged();
    //}

    private void SetNewOrder(Enemy enemy)
    {
        ChangeOrder();
    }

    private void ChangeOrder()
    {
        int newOrder = Random.Range(0, _orderCount);
        if (newOrder != Order)
        {
            _currentCooldown = _cooldownChangeOrder;
            Order = newOrder;
            EventManager.SendOrderChanged();
        }
        else
        {
            ChangeOrder();
        }
     }

    private void Update()
    {
        if (!GameParameters.GameRunning)
            return;

        _currentCooldown -= Time.deltaTime;
        if (_currentCooldown <= 0)
        {
            ChangeOrder();
        }
    }
}
