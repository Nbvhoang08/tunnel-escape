using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour ,IObserver
{
    [SerializeField] private Animator animator;
    [SerializeField]private int headHitCount = 0; // Số lần trúng đòn vào đầu
    private bool isStunned = false;
    public Collider Leg; 
    [SerializeField] private float stunDuration = 3f; // Thời gian bị stun
    [SerializeField] private float minAttackInterval = 1f; // Khoảng thời gian tối thiểu giữa các đòn đánh
    [SerializeField] private float maxAttackInterval = 3f; // Khoảng thời gian tối đa giữa các đòn đánh
    private bool isAttacking = false;
    public GameObject StunEffect;
    public float hp;
    public float maxHp;
    public bool isDead =>hp <=0;
    public float damage = 10;
    public float energy;
    public static Enemy Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Subject.RegisterObserver(this); // Đăng ký observer
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Subject.UnregisterObserver(this); // Hủy đăng ký observer
            Instance = null; // Làm rỗng instance
        }

    }
    public void OnNotify(string eventName,object eventData)
    {
        if(eventName == "LetBattle")
        {
            StartCoroutine(RandomAttack());
        }
    }
    void Start(){
        StunEffect.SetActive(false);
        hp = maxHp;
        isKnee= false;
    }
    // Update is called once per frame
    void Update()
    {
        // Nếu đang bị stun, không thực hiện hành động nào khác
        animator.SetBool("Stun", isStunned);
 
        if (isStunned)return;    
        if (isDead)
        {
            Die();
            return;
          
        }
   
    }
   
   
    // Xử lý va chạm Trigger
    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra va chạm với Player hoặc vũ khí của Player
        if (other.gameObject.CompareTag("PlayerLeg") && !isKnee)
        {
            Hit(); // Trúng các phần còn lại
            hp -= damage;
            Debug.Log("body");
            return;

        }
    }
    // Hành động khi bị trúng đòn vào chân
    public bool isKnee= false;
    public void HitOnLeg()
    {
        if (isStunned) return;
        isKnee = true;
        animator.SetTrigger("stuned"); // Trigger animation ngã quỵ

    }
    public GameObject dieEffectPrefab;
    public void Die()
    {
         // Spawn hiệu ứng chết
        VibrateDevice();
        if (dieEffectPrefab != null)
        {
            Instantiate(dieEffectPrefab, transform.position, Quaternion.identity);
        }
        // Vô hiệu hóa đối tượng Enemy
        gameObject.SetActive(false);
    }
    // Hành động khi bị trúng đòn bình thường
    public bool isTakeDamage = false;
    public void Hit()
    {
        if (isStunned) return;
        if(isKnee) return;
        animator.SetTrigger("hitted"); // Trigger animation trúng đòn
        isTakeDamage = true;
        StartCoroutine(ResetTakeDamage());
    }

    // Hành động khi bị trúng đòn vào đầu
    IEnumerator ResetTakeDamage()
    {
        yield return new WaitForSeconds(1);
        isTakeDamage = false;
    }   
    public void HitOnHead()
    {
        if (isStunned) return;
        if(isKnee) return;
        animator.SetTrigger("hitted");
        headHitCount++;
        isTakeDamage = true;
        // Kiểm tra nếu trúng đầu quá 3 lần thì bị stun
        if (headHitCount >= 3)
        {
            VibrateDevice();
            StartCoroutine(Stun());
            return;
        }
        StartCoroutine(ResetTakeDamage());
       // Trigger animation trúng đòn
    }
    private IEnumerator RandomAttack()
    {
        while (true)
        {
            // Chờ cho đến khi isAttacking là false
            while (isAttacking || isStunned || isKnee || isTakeDamage)    
            {
                yield return null;
            }

            // Chờ một khoảng thời gian ngẫu nhiên giữa min và max
            float waitTime = UnityEngine.Random.Range(minAttackInterval, maxAttackInterval);
            yield return new WaitForSeconds(waitTime);
            if (isDead) yield break; // Thoát khỏi vòng lặp nếu Enemy đã chết
            if (!isStunned && !isAttacking)
            {
                PerformAttack();
            }
        }
    }
    
    void VibrateDevice()
    {
        Handheld.Vibrate();
    }
    // Thực hiện hành động tấn công
    private void PerformAttack()
    {
        isAttacking = true;
        Leg.enabled = false;
        animator.SetTrigger("atk"); // Trigger animation tấn công
        // Đợi animation tấn công hoàn thành trước khi cho phép tấn công lần sau
        StartCoroutine(ResetAttackState());
    }

    // Đặt lại trạng thái tấn công sau khi hoàn thành
    private IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(2f); // Thời gian đủ để hoàn thành animation tấn công
        isAttacking = false;
        Leg.enabled = true; 
    }
    
    // Xử lý stun
    private IEnumerator Stun()
    {
        isStunned = true; // Trigger animation bị stun
        StunEffect.SetActive(true);
        yield return new WaitForSeconds(stunDuration);
        StunEffect.SetActive(false);
        isStunned = false;
        headHitCount = 0; // Reset số lần trúng đầu sau khi hết stun
        animator.SetTrigger("idle");
    }
}
