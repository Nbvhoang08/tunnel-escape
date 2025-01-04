using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLeg : MonoBehaviour
{
    public Enemy enemy;
    public float damage = 10;
    private bool canTakeDamage = true; // Biến cờ để kiểm tra xem có thể nhận damage hay không

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra va chạm với Player hoặc vũ khí của Player
        if (other.gameObject.CompareTag("PlayerLeg") && canTakeDamage)
        {
            enemy.HitOnLeg(); // Trúng các phần còn lại
            enemy.hp -= damage;
            StartCoroutine(DamageCooldown());
        }
    }

    // Coroutine để đặt lại biến canTakeDamage sau 0.5 giây
    private IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(0.5f);
        canTakeDamage = true;
    }
}
