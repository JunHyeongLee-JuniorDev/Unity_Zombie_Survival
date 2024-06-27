using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    /*
     필요한 변수들'
    1. 체력
    2. 오디오
    3. 애니
    4. 무브
    5. 슛
     */
    
    [SerializeField]private Slider healthSlider;

    [SerializeField]private AudioClip deathClip;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip ItemGetClip;

    private AudioSource playerAudio;
    private Animator playerAni;

    private Player_movement playerMove;
    private PlayerShooter player_shooter;

    private void Awake()
    {
        playerAudio = GetComponent<AudioSource>();
        playerAni = GetComponent<Animator>();
        playerMove = GetComponent<Player_movement>();
        player_shooter = GetComponent<PlayerShooter>();
    }

    protected override void OnEnable()
    {
        base.OnEnable(); // 부모의 클래스 메소드를 호출!

        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = startHealth;
        healthSlider.value = Health;

        //죽었을 때 move shooter을 비활성화 할 것이기 때문에
        // 여기서 확인차 활성화
        playerMove.enabled = true;
        player_shooter.enabled = true;
    }

    public override void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        if(!isDead)
        {
            playerAudio.PlayOneShot(hitClip);
        }
        
        base.OnDamage(damage, hitPosition, hitNormal);

        // 슬라이더 바 업데이트
        healthSlider.value = Health;
    }

    public override void Die()
    {
        base.Die();

        healthSlider.gameObject.SetActive(false);
        
        //시각효과 추가
        playerAni.SetTrigger("Die");
        playerAudio.PlayOneShot(deathClip);
        // 죽은 후 컴포넌트 비활성화

        playerMove.enabled = false;
        player_shooter.enabled = false;
    }
}
