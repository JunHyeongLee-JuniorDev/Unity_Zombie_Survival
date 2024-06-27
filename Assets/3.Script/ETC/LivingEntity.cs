using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LivingEntity : MonoBehaviour, IDamage
{
    /*
     ������ ����
    1. ��ü ü��
    2. ���� ü��
    3. �׾����� ��Ҵ��� -> �̺�Ʈ�� ���� C#�� Action ����ؼ�

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
            //�״� �޼ҵ� ȣ��
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
