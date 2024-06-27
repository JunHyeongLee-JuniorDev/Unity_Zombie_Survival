using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieControl : LivingEntity
{
    /*
     1. �׺���̼� Agent�� ����� ã�������� ��ã�� ���� ����
     2. ��� ã�� ����� -> player��� ���̾ �΋H���� player�� �������� �׺���̼����� ã���� �� ��
     3. �׺���̼����� ã��
     4. player�� �������� �˰� �ִٸ� �� �̻� Overlap�� ���� ���� ��
     */

    [Header("������ ��� ���̾�")]
    public LayerMask TargetLayer;
    private LivingEntity targetEntity;

    /*
     * ����
            *�״°�
            *�´°�
     + �´� effect

     * ���� �ִ� / ���� �����
     
     * ���� ������
     * ���� ���� ��Ÿ��
     * ������ ���� �ð� ����
     
     */
    private NavMeshAgent agent;

    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private ParticleSystem bloodEffect;

    private Animator zombieAni;
    private AudioSource ZombieAudio;

    [SerializeField] private float Damage = 20f;
    [SerializeField] private float timeBetAttack = 0.5f;
    private float LastAttack_timebet;

    private bool isTarget
    {
        get
        {
            if (targetEntity != null && !targetEntity.isDead)
                return true;

            else
                return false;
        }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        ZombieAudio = GetComponent<AudioSource>();
        zombieAni = GetComponent<Animator>();
        LastAttack_timebet = 0;
    }

    /*
     * ������ �޼ҵ�
     * ������ �޴�
     * �״�
     * ã���� �����ϴ� ontriggerStay
     * Ÿ���� ã�������� �ٸ��� �� ������Ʈ��
     * �ڷ�ƾ���� Ÿ�� Ž��
     * ��ŸƮ���� �ڷ�ƾ ����
     */

    public override void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        /*
         * ������ ����
         * ���� �ʾҰ� �ǵ� 0 �̻��̾�� �ǰ� ����
         * ������ ������Ʈ ���� ������Ʈ�� ����
         */

        if(!isDead)
        {
            bloodEffect.transform.position = hitPosition;
            bloodEffect.transform.rotation = Quaternion.LookRotation(hitNormal);

            bloodEffect.Play();

            ZombieAudio.PlayOneShot(hitClip);
        }

        base.OnDamage(damage, hitPosition, hitNormal);
    }

    public override void Die()
    {
        base.Die();
        Collider[] col = GetComponents<Collider>();
        /*
         for -> ���� ���� : index ����
         foreach -> ���� ���� : ������Ʈ

        �� �� �ӵ� ���� ����
        */

        foreach(Collider c in col)
        {
            c.enabled = false;
        }

        agent.isStopped = true;
        agent.enabled = false;
        zombieAni.SetTrigger("Die");
        ZombieAudio.PlayOneShot(deathClip);
    }

    private void OnTriggerStay(Collider other)
    {
        if(!isDead && Time.time >= LastAttack_timebet + timeBetAttack)
        {
            if(other.TryGetComponent(out LivingEntity e))
            {
                if(e.Equals(targetEntity))
                {
                    LastAttack_timebet = Time.time;

                    //closestpoint -> ������ �ǰ���ġ�� �ǰ� ������ �ٻ簪���� ���
                    Vector3 hitpoint = other.ClosestPoint(transform.position);
                    Vector3 hitnormal = transform.position - other.transform.position;

                    e.OnDamage(Damage, hitpoint, hitnormal);
                }
            }
        }
    }

    private void Update()
    {
        zombieAni.SetBool("HasTarget", isTarget);
    }

    private void Start()
    {
        StartCoroutine(Update_Target_position_co());
    }

    private IEnumerator Update_Target_position_co()
    {
        while(!isDead)
        {
            if(isTarget)
            {
                agent.isStopped = false;
                agent.SetDestination(targetEntity.transform.position);
            }

            else
            {
                agent.isStopped = true;
                Collider[] col =
                    Physics.OverlapSphere(transform.position, 20f, TargetLayer);

                for(int i=0;i<col.Length;i++)
                {
                    if(col[i].TryGetComponent(out LivingEntity e))
                    {
                        if(!e.isDead)
                        {
                            targetEntity = e;
                            break;
                        }
                    }
                }
            }

            yield return null; // �� �����Ӹ� ����
        }
    }
}
