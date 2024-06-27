using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    /*
     �ʿ��� ������'
    1. ü��
    2. �����
    3. �ִ�
    4. ����
    5. ��
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
        base.OnEnable(); // �θ��� Ŭ���� �޼ҵ带 ȣ��!

        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = startHealth;
        healthSlider.value = Health;

        //�׾��� �� move shooter�� ��Ȱ��ȭ �� ���̱� ������
        // ���⼭ Ȯ���� Ȱ��ȭ
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

        // �����̴� �� ������Ʈ
        healthSlider.value = Health;
    }

    public override void Die()
    {
        base.Die();

        healthSlider.gameObject.SetActive(false);
        
        //�ð�ȿ�� �߰�
        playerAni.SetTrigger("Die");
        playerAudio.PlayOneShot(deathClip);
        // ���� �� ������Ʈ ��Ȱ��ȭ

        playerMove.enabled = false;
        player_shooter.enabled = false;
    }
}
