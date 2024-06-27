using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LivingEntity : MonoBehaviour, IDamage
{
    /*
     구현할 사항
    1. 전체 체력
    2. 현제 체력
    3. 죽었는지 살았는지 -> 이벤트로 구현 C#용 Action 사용해서

    IDamage
     */

    [SerializeField]public float startHealth = 100f;

    public float Health { get; private set; }
    public bool isDead { get; private set; }

    public event Action OnDead;


    protected virtual void OnEnable()
    {
        isDead = false;
        Health = startHealth;
    }

    public virtual void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        Health -= damage;

        if(Health <= 0 && !isDead)
        {
            //죽는 메소드 호출
            Die();
        }
    }

    public virtual void Die()
    {
        if(OnDead != null)
        {
            OnDead();
        }
        isDead = true;
    }

    public virtual void Restore_health(float newHealth)
    {
        if(isDead)
        {
            return;
        }
        Health += newHealth;
    }
}
