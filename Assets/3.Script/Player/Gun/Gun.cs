using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    /*
     총게임 -> Raycast 

    총알 -> LineRender/Raycast
    총의 사거리
    발사될 위치
    GunData
    Effect 
    총의 상태-> Enum
        1. 재장전 / 2. 탄창이 비었을때/ 3. 발사준비
    audio Source

    method
       발사 -> fire or shot
       Reload
       effect Play
     */

    public enum State
    {
        Ready,//발사준비
        Empty,//총발 빔
        Reloading
    }
    public State state { get; private set; }

    [SerializeField]private float Distance = 50f;

    public LineRenderer lineRenderer;
    public Transform Fire_transform;

    public Gun_Data gundata;

    public ParticleSystem Shot_effect;
    public ParticleSystem Shell_effect;

    [SerializeField]private AudioSource audio_s;

    public int AmmoRemain;
    public int Magammo;

    private float LastFireTime;

    private void Awake()
    {
        audio_s = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        //컴포넌트 비활성화
        lineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        AmmoRemain = gundata.StartAmmoRemain;
        Magammo = gundata.MagCapacity;

        state = State.Ready;

        LastFireTime = 0;
    }

    public void Fire()
    {
        //플레이어의 현채 총상태가 준비상태이면서
        //마지막 발사시간이 현재 시간보다 작을 때 발사가능
        if(state.Equals(State.Ready)&&
            Time.time>=LastFireTime+gundata.Timebet_fire)
        {
            LastFireTime = Time.time;
            Shot();
        }
    }
    private void Shot()
    {
        RaycastHit hit;
        Vector3 hitPosition=Vector3.zero;

        //총 -> RayCast
        if(Physics.Raycast(Fire_transform.position,Fire_transform.forward,
            out hit,Distance))
        {
            //총알이 맞은 경우
            //우리가 만든 interface가지고 와서
            //맞은 오브젝트에게 데미지를 줘햐함. 
            
            if(hit.collider.TryGetComponent(out IDamage d))
            {
                d.OnDamage(gundata.Damage, hit.point, hit.normal);
            }

            hitPosition = hit.point;
        }
        else
        {
            //Ray가 다른 객체와 충돌이 되지 않았을 경우
            //총알이 최대 사정거리까지 날라갔을 경우
            hitPosition = 
                Fire_transform.position + 
                Fire_transform.forward * Distance;
        }
        //총쏘는 이펙트~
        StartCoroutine(shotEffect(hitPosition));
        Magammo--;
        if(Magammo<=0)
        {
            state = State.Empty;
        }
    }
    private IEnumerator shotEffect(Vector3 hitposition)
    {
        Shot_effect.Play();
        Shell_effect.Play();
        //소리 빵빵
        audio_s.PlayOneShot(gundata.Shotclip);

        lineRenderer.SetPosition(0, Fire_transform.position);
        lineRenderer.SetPosition(1, hitposition);
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.03f);
        lineRenderer.enabled = false;

    }


    public bool Reload()
    {
        //현재 재장전이 필요한지 안한지 Retrun 할 메소드
        //이미 재장전 중이거나
        //가지고있는 총알이 없거나
        //탄창에 이미 총알이 가득한 경우(30발이 장전되어 있는 경우)
        //=> false

        if(state.Equals(State.Reloading)||AmmoRemain<=0||Magammo>=gundata.MagCapacity)
        {
            return false;
        }
        //총알가는 갈수 있는 조건
        StartCoroutine(Reload_co());
        return true;
    }

    private IEnumerator Reload_co()
    {
        state = State.Reloading;
        audio_s.PlayOneShot(gundata.Reload_clip);

        yield return new WaitForSeconds(gundata.Reload_Time);

        //재장전 후에 계산
        int ammofill = gundata.MagCapacity - Magammo;
        if(AmmoRemain<ammofill)
        {
            ammofill = AmmoRemain;
        }
        //탄창을 채우고 전체 탄창수를 줄인다. 
        Magammo += ammofill;
        AmmoRemain -= ammofill;

        state = State.Ready;


    }


}
