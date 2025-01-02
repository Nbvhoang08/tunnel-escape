using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private int headHitCount = 0; // Số lần trúng đòn vào đầu
    private bool isStunned = false;

    [SerializeField] private float stunDuration = 3f; // Thời gian bị stun
    [SerializeField] private string headTag = "Head"; // Tag cho phần đầu
    [SerializeField] private string legTag = "Leg";   // Tag cho phần chân
    [SerializeField] private float minAttackInterval = 1f; // Khoảng thời gian tối thiểu giữa các đòn đánh
    [SerializeField] private float maxAttackInterval = 3f; // Khoảng thời gian tối đa giữa các đòn đánh
    private bool isAttacking = false;
    // Update is called once per frame
    private void Start()
    {
        // Bắt đầu chuỗi tấn công ngẫu nhiên
        StartCoroutine(RandomAttack());
    }
    void Update()
    {
        // Nếu đang bị stun, không thực hiện hành động nào khác
        if (isStunned)return;
    }

    // Xử lý va chạm Trigger
    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra va chạm với Player hoặc vũ khí của Player
        if (other.CompareTag("PlayerLeg"))
        {
            // Kiểm tra phần Enemy bị trúng đòn dựa trên Tag của Collider
            if (this.CompareTag(headTag))
            {
                HitOnHead(); // Trúng đầu
            }
            else if (this.CompareTag(legTag))
            {
                HitOnLeg(); // Trúng chân
            }
            else
            {
                Hit(); // Trúng các phần còn lại
            }
        }
        // Kiểm tra va chạm với Player hoặc vũ khí của Player
        if (other.CompareTag("Player"))
        {
            // Kiểm tra phần Enemy bị trúng đòn dựa trên Tag của Collider
            if (this.CompareTag("Arm"))
            {
                HitOnHead(); // Trúng đầu
            }
          
        }
    }

    // Hành động khi bị trúng đòn vào chân
    public void HitOnLeg()
    {
        if (isStunned) return;
        animator.SetTrigger("strun"); // Trigger animation ngã quỵ
        Debug.Log("Enemy hit on leg and fell to knees.");
    }

    // Hành động khi bị trúng đòn bình thường
    public void Hit()
    {
        if (isStunned) return;
        animator.SetTrigger("TakeHit"); // Trigger animation trúng đòn
    }

    // Hành động khi bị trúng đòn vào đầu
    public void HitOnHead()
    {
        if (isStunned) return;

        headHitCount++;
        // Kiểm tra nếu trúng đầu quá 3 lần thì bị stun
        if (headHitCount >= 3)
        {
            StartCoroutine(Stun());
            return;
        }
        animator.SetTrigger("TakeHit"); // Trigger animation trúng đòn
    }

    // Hành động tấn công
    // Hành động tấn công ngẫu nhiên
    private IEnumerator RandomAttack()
    {
        while (true)
        {
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
        animator.SetTrigger("Attack"); // Trigger animation tấn công
        Debug.Log("Enemy performed a random attack.");

        // Đợi animation tấn công hoàn thành trước khi cho phép tấn công lần sau
        StartCoroutine(ResetAttackState());
    }

    // Đặt lại trạng thái tấn công sau khi hoàn thành
    private IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(1f); // Thời gian đủ để hoàn thành animation tấn công
        isAttacking = false;
    }

    // Xử lý stun
    private IEnumerator Stun()
    {
        isStunned = true;
        animator.SetTrigger("Stunned"); // Trigger animation bị stun
        Debug.Log("Enemy is stunned.");

        yield return new WaitForSeconds(stunDuration);

        isStunned = false;
        headHitCount = 0; // Reset số lần trúng đầu sau khi hết stun
        animator.SetTrigger("idle");
    }
    // Hành động tấn công
    
}
