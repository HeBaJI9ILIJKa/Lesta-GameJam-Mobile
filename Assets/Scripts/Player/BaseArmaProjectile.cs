using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseArmaProjectile : MonoBehaviour
{
    [SerializeField] private float _speed = 5f, _lifeTime = 5f;
    private Rigidbody _rigidBody = null;
    

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Invoke("AutoDestroy", _lifeTime);  
    }

    private void AutoDestroy()
    {
        Destroy(gameObject);
    }

    public void Launch()
    {
        _rigidBody.AddForce(transform.forward * _speed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        var enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.TakeDamage();
        }

        Destroy(gameObject);
    }
}
