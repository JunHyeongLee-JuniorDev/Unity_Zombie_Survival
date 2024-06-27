using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    /*
     �Ѱ��� -> Raycast 

    �Ѿ� -> LineRender/Raycast
    ���� ��Ÿ�
    �߻�� ��ġ
    GunData
    Effect 
    ���� ����-> Enum
        1. ������ / 2. źâ�� �������/ 3. �߻��غ�
    audio Source

    method
       �߻� -> fire or shot
       Reload
       effect Play
     */

    public enum State
    {
        Ready,//�߻��غ�
        Empty,//�ѹ� ��
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
        //������Ʈ ��Ȱ��ȭ
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
        //�÷��̾��� ��ä �ѻ��°� �غ�����̸鼭
        //������ �߻�ð��� ���� �ð����� ���� �� �߻簡��
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

        //�� -> RayCast
        if(Physics.Raycast(Fire_transform.position,Fire_transform.forward,
            out hit,Distance))
        {
            //�Ѿ��� ���� ���
            //�츮�� ���� interface������ �ͼ�
            //���� ������Ʈ���� �������� ������. 
            
            if(hit.collider.TryGetComponent(out IDamage d))
            {
                d.OnDamage(gundata.Damage, hit.point, hit.normal);
            }

            hitPosition = hit.point;
        }
        else
        {
            //Ray�� �ٸ� ��ü�� �浹�� ���� �ʾ��� ���
            //�Ѿ��� �ִ� �����Ÿ����� ������ ���
            hitPosition = 
                Fire_transform.position + 
                Fire_transform.forward * Distance;
        }
        //�ѽ�� ����Ʈ~
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
        //�Ҹ� ����
        audio_s.PlayOneShot(gundata.Shotclip);

        lineRenderer.SetPosition(0, Fire_transform.position);
        lineRenderer.SetPosition(1, hitposition);
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.03f);
        lineRenderer.enabled = false;

    }


    public bool Reload()
    {
        //���� �������� �ʿ����� ������ Retrun �� �޼ҵ�
        //�̹� ������ ���̰ų�
        //�������ִ� �Ѿ��� ���ų�
        //źâ�� �̹� �Ѿ��� ������ ���(30���� �����Ǿ� �ִ� ���)
        //=> false

        if(state.Equals(State.Reloading)||AmmoRemain<=0||Magammo>=gundata.MagCapacity)
        {
            return false;
        }
        //�Ѿ˰��� ���� �ִ� ����
        StartCoroutine(Reload_co());
        return true;
    }

    private IEnumerator Reload_co()
    {
        state = State.Reloading;
        audio_s.PlayOneShot(gundata.Reload_clip);

        yield return new WaitForSeconds(gundata.Reload_Time);

        //������ �Ŀ� ���
        int ammofill = gundata.MagCapacity - Magammo;
        if(AmmoRemain<ammofill)
        {
            ammofill = AmmoRemain;
        }
        //źâ�� ä��� ��ü źâ���� ���δ�. 
        Magammo += ammofill;
        AmmoRemain -= ammofill;

        state = State.Ready;


    }


}
