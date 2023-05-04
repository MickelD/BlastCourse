using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    #region Fields
    [Space(5), Header("Damage Multipliers"), Space(3)]
    [SerializeField] protected float _playerDamageMultiplier;
    [SerializeField] protected float _enemyDamageMultiplier;
    [SerializeField] protected float _environmentDamageMultiplier;

    [Space(5), Header("Health"), Space(3)]
    [SerializeField] protected float _maxHealth;
    [SerializeField] protected float _health;

    [Space(5), Header("Invulnerability Frames"), Space(3)]
    [SerializeField] protected float _invulnerability = 0.5f;

    #endregion

    #region Variables
    public enum Source
    {
        PLAYER,
        ENEMY,
        ENVIRONMENT,
        HEAL
    }

    protected float _invincibleTimer;

    protected bool _alive = true;
    
    #endregion

    #region UnityFunctions
    public void Awake()
    {
        _health = _maxHealth;
    }

    //FOR TESTING PURPOSES

    protected void Update()
    {
        if(_invincibleTimer > 0)
        {
            _invincibleTimer -= Time.deltaTime;
        }
    }
    #endregion

    #region Methods
    public virtual void SufferDamage(float amount, Source source)
    {
        if(_invincibleTimer <= 0)
        {
            switch (source)
            {
                case Source.PLAYER:
                    _health -= amount * _playerDamageMultiplier;
                    _invincibleTimer = _invulnerability;
                    break;
                case Source.ENEMY:
                    _health -= amount * _enemyDamageMultiplier;
                    _invincibleTimer = _invulnerability;
                    break;
                case Source.ENVIRONMENT:
                    _health -= amount * _environmentDamageMultiplier;
                    _invincibleTimer = _invulnerability;
                    break;
                case Source.HEAL:
                default:
                    _health -= amount;
                    break;
            }
            _health = Mathf.Clamp(_health, 0, _maxHealth);

            if (_health <= 0 && _alive)
            {
                Die();
                _alive = false;
            }
        }
        
    }
    public void Heal(float amount)
    {
        _invincibleTimer = 0;
        SufferDamage(-amount, Source.HEAL);
    }
    public virtual void Die()
    {
        Debug.Log("You Died");
    }

    #endregion

    #region Getter/Setter Methods
    public void SetHealth(float value)
    {
        _health = value;
    }
    public float GetHealth()
    {
        return _health;
    }
    public void SetMaxHealth(float value)
    {
        _maxHealth = value;
    }
    public float GetMaxHealth()
    {
        return _maxHealth;
    }


    #endregion

}
