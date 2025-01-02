using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    // Start is called before the first frame update
    public Animator anim;
    public bool canAction;
    void Start()
    {
        canAction = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void lowKick()
    {
        if (!canAction) return;
        canAction = false;
        anim.SetTrigger("lowKick");
        SoundManager.Instance.PlayVFXSound(1);
    }
    public void midleKick()
    {
        if (!canAction) return;
        canAction = false;
        anim.SetTrigger("midleKick");
        SoundManager.Instance.PlayVFXSound(1);
    }

    public void highKick() 
    {
        if (!canAction) return;
        canAction = false;
        anim.SetTrigger("highKick");
        SoundManager.Instance.PlayVFXSound(1);
    }
    public void AvoidHit()
    {
        if (!canAction) return;
        canAction = false;
        anim.SetTrigger("avoid");
    }
    public void Hitted()
    {
        anim.SetTrigger("hitted");
    }
    public void ResetAction() { canAction = true; }
    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu Enemy va chạm
        if (other.CompareTag("Enemy")) // Đảm bảo Enemy có tag là "Enemy"
        {
            // Lấy vị trí va chạm
            Vector3 contactPoint = other.ClosestPoint(transform.position);

            // Spawn hiệu ứng máu
            SpawnBloodEffect(contactPoint);
        }
    }
    public GameObject bloodEffectPrefab;
    // Hàm spawn hiệu ứng máu
    private void SpawnBloodEffect(Vector3 position)
    {
        if (bloodEffectPrefab != null)
        {
            // Instantiate hiệu ứng máu tại vị trí va chạm
            Instantiate(bloodEffectPrefab, position, Quaternion.identity);
        }
    }
}
