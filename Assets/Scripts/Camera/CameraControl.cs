using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Vector3 _distance;
    [SerializeField] private float _speed = 10f;

    private void LateUpdate()
    {
        Movement();

        //Rotation();
    }

    private void Movement()
    {
        transform.position = _player.transform.position + _distance;
    }

    private void Rotation()
    {
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, -_speed * Time.deltaTime, 0);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, _speed * Time.deltaTime, 0);
        }
    }
}
