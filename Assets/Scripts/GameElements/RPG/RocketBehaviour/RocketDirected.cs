using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketDirected : RocketBase
{
    #region Fields

    [Space(5), Header("Extra Variables"), Space(3)]
    [SerializeField] float _turnSpeed;
    [SerializeField] float _acceleration;

    #endregion

    #region Variables

    private Vector3 _target;
    private bool _isTargeting;
    private float _speed;

    #endregion

    #region UnityFunctions

    private void Update()
    {
        if (_isTargeting)
        {
            Vector3 direction = Vector3.RotateTowards(transform.forward, _target - transform.position, _turnSpeed * Time.deltaTime, Mathf.Infinity);
            SetVelocity(direction.normalized * _speed);
        }

        _speed += _acceleration * Time.deltaTime;
    }

    #endregion

    #region Getters/Setters

    public void SetTarget(Vector3 target)
    {
        _isTargeting = true;
        _target = target;
    }

    public void SetIsTargeting(bool isTargeting)
    {
        _isTargeting = isTargeting;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    #endregion
}
