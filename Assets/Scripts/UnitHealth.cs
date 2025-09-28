using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitHealth
{
    public int _currentHealth;
    public int _currentMaxHealth;




    public int Health
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;
        }
    }
    public int MaxHealth
    {
        get
        {
            return _currentMaxHealth;
        }
        set
        {
            _currentMaxHealth = value;
        }
    }

    public UnitHealth(int health, int maxHealth)
    {
        _currentHealth = health;
        _currentMaxHealth = maxHealth;

    }
    public void DamageUnit(int damageAmount)
    {
        if (_currentHealth > 0)
        {
            _currentHealth -= damageAmount;
        }

    }

    public void addmaxHealth(int healthtoAdd)
    {
        _currentMaxHealth = healthtoAdd + _currentMaxHealth;
        MaxHealth = _currentMaxHealth;
        _currentHealth += healthtoAdd;
    }
    public void HealUnit(int healAmount)
    {
        if (_currentHealth < _currentMaxHealth)
        {
            _currentHealth += healAmount;
        }
        if (_currentHealth > _currentMaxHealth)
        {
            _currentHealth = _currentMaxHealth;
        }
    }







}
