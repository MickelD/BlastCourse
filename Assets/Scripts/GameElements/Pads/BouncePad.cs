using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BouncePad : MonoBehaviour
{
    #region Fields
    [Space(5), Header("Handle"), Space(3)]
    [SerializeField] private Transform c_directionController;
    [SerializeField] private float _handleDistance;

    #endregion

    #region Variables

    private Vector3 _direction = Vector3.up;

    #endregion

    #region Method

    public Vector3 Reload()
    {
        _direction = c_directionController.position - transform.position;
        _direction.Normalize();
        _direction *= _handleDistance;

        if(Vector3.Angle(_direction, transform.up) > 90)
        {
            _direction *= -1;
        }

        c_directionController.position = transform.position + _direction;

        return _direction;
    }

    #endregion

    #region Collisions && Triggers

    private void OnTriggerEnter(Collider other)
    {
        RocketBase rocket = other.GetComponent<RocketBase>();
        if(rocket != null)
        {
            Vector3 ray = rocket.GetVelocity();
            float magnitude = ray.magnitude;
            if (Vector3.Angle(ray, transform.up) >= 90)
            {
                ray = _direction.normalized * magnitude;
                rocket.SetVelocity(ray);
            }
        }
    }

    #endregion

    #region Debug

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position,c_directionController.position);
    }

    #endregion
}
