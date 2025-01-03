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
    
    public float energy;
    private void Awake()
    {
        Subject.RegisterObserver(this);
    }
    void Start(){
        StunEffect.SetActive(false);
        hp = maxHp;
    }
    // Update is called once per frame
    void Update()
    {
        // Nếu đang bị stun, không thực hiện hành động nào khác
        animator.SetBool("Stun", isStunned);
        if (isStunned)return;

        
    }
    public void OnNotify(string eventName,object eventData)
    {
        if(eventName == "LetBattle")
        {
            StartCoroutine(RandomAttack());
        }
    }
    // Xử lý va chạm Trigger
    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra va chạm với Player hoặc vũ khí của Player
        if (other.gameObject.CompareTag("PlayerLeg"))
        {
            Hit(); // Trúng các phần còn lại
            hp -= 5;
        }
    }
    // Hành động khi bị trúng đòn vào chân
    public void HitOnLeg()
    {
        if (isStunned) return;
        animator.SetTrigger("stuned"); // Trigger animation ngã quỵ

    }

    // Hành động khi bị trúng đòn bình thường
    public void Hit()
    {
        if (isStunned) return;
        
        animator.SetTrigger("hitted"); // Trigger animation trúng đòn
    }

    // Hành động khi bị trúng đòn vào đầu
    public void HitOnHead()
    {
        if (isStunned) return;
        animator.SetTrigger("hitted");
        headHitCount++;
        // Kiểm tra nếu trúng đầu quá 3 lần thì bị stun
        if (headHitCount >= 3)
        {
            StartCoroutine(Stun());
            return;
        }
       // Trigger animation trúng đòn
    }
    private IEnumerator RandomAttack()
    {
        while (true)
        {
            // Chờ cho đến khi isAttacking là false
            while (isAttacking || isStunned)    
            {
                yield return null;
            }

            // Chờ một khoảng thời gian ngẫu nhiên giữa min và max
            float waitTime = Random.Range(minAttackInterval, maxAttackInterval);
            yield return new WaitForSeconds(waitTime);

            if (!isStunned && !isAttacking)
            {
                PerformAttack();
            }
        }
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
