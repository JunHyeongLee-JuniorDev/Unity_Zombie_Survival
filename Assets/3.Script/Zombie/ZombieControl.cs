using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieControl : LivingEntity
{
    /*
     1. 네비게이션 Agent는 대상을 찾기전까진 길찾기 실행 ㄴㄴ
     2. 대상 찾는 방법이 -> player라는 레이어가 부딫히면 player의 포지션을 네비게이션으로 찾도록 할 것
     3. 네비게이션으로 찾기
     4. player의 포지션을 알고 있다면 더 이상 Overlap을 하지 않을 것
     */

    [Header("추적할 대상 레이어")]
    public LayerMask TargetLayer;
    private LivingEntity targetEntity;

    /*
     * 사운드
            *죽는거
            *맞는거
     + 맞는 effect

     * 좀비 애니 / 좀비 오디오
     
     * 좀비 데미지
     * 좀비 공격 쿨타임
     * 마지막 공격 시간 변수
     
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
     * 구현할 메소드
     * 데미지 받는
     * 죽는
     * 찾으면 공격하는 ontriggerStay
     * 타겟을 찾고있으면 다리는 거 업데이트에
     * 코루틴으로 타겟 탐색
     * 스타트에서 코루틴 실행
     */

    public override void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        /*
         * 좀비의 입장
         * 죽지 않았고 피도 0 이상이어야 피가 깍임
         * 죽으면 컴포넌트 끄고 에이전트도 끄기
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
         for -> 기준 접근 : index 기준
         foreach -> 기준 접근 : 오브젝트

        둘 다 속도 차이 없음
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

                    //closestpoint -> 상대방의 피격위치와 피격 방향을 근사값으로 계산
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

            yield return null; // 한 프레임만 쉰다
        }
    }
}
