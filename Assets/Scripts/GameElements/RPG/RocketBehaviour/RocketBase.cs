using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBase : MonoBehaviour
{
    #region Fields

    [SerializeField] protected Explosion _explosion;

    //[Space(5), Header("CollisionRaycast"), Space(3)]
    //[SerializeField] protected LayerMask _layersToCheck;
    //[SerializeField] protected float _rayLength;

    [Space(5), Header("Stats"), Space(3)]
    [SerializeField] protected float _killTime = 12;

    [Space(5), Header("Components"), Space(3)]
    [SerializeField] protected Rigidbody c_rb;
    [SerializeField] protected ParticleSystem c_rocketParticles;
    [SerializeField] protected GameObject g_diffuseParticle;

    #endregion

    #region UnityFunctions


    protected void Start()
    {
        Destroy(gameObject, _killTime);
    }

    #endregion

    #region Methods

    public virtual void Diffuse()
    {
        Destroy(Instantiate(g_diffuseParticle,transform.position,Quaternion.identity),2f);
        c_rocketParticles.Stop();
        Destroy(c_rocketParticles.gameObject, 2f);
        c_rocketParticles.gameObject.transform.parent = null;

        Destroy(gameObject);
    }

    public virtual void Explode(Vector3 center, Vector3 direction)
    {
        _explosion.Explode(center,direction);

        c_rocketParticles.Stop();
        Destroy(c_rocketParticles.gameObject, 2f);
        c_rocketParticles.gameObject.transform.parent = null;

        Destroy(gameObject);
    }

    #endregion

    #region Getters && Setters

    public void SetVelocity(Vector3 velocity)
    {
        c_rb.velocity = velocity;
        transform.rotation = Quaternion.LookRotation(velocity.normalized);
    }
    public Vector3 GetVelocity()
    {
        return c_rb.velocity;
    }

    #endregion

    #region Collisions && Triggers

    protected void OnCollisionEnter(Collision collision)
    {
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, transform.forward, out hit, _rayLength, _layersToCheck))
        //{
        //    Explode(hit.point, hit.normal);
        //}
        //else
        //{
            Explode(collision.contacts[0].point, collision.contacts[0].normal);
        //}
        
    }

    #endregion
}
