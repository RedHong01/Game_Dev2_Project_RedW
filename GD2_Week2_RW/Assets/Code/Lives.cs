using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour,Damageable
{
    public float startHealth;
    protected float health;
    protected bool dead;

    public event System.Action OnDeath;

    protected virtual void Start()
    {
        health = startHealth;
    }


    public void TakeHit(float damage, RaycastHit hit)
    {
        //for the particle effects and others

        TakeDamage(damage);
    }
    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0 && !dead)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        dead = true;
        if(OnDeath != null)
        {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }
    // Start is called before the first frame update
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
