using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;
    public bool canAction;
    public Collider Col;
    public float hp;
    public float maxHp;
    public float n; 
    public float maxEnergy;    
    public float energy;
    public float restoreDuration = 1f; // Duration over which energy is restored
    public bool isDead => hp <= 0;     

    void Start()
    {
        canAction = true;
        Col = GetComponent<Collider>();
        energy = maxEnergy;
        hp = maxHp;
    }

    // Update is called once per frame
    public void lowKick()
    {
        if (!canAction) return;
        canAction = false;
        anim.SetTrigger("lowKick");
        SoundManager.Instance.PlayVFXSound(1);
        StartCoroutine(ReduceAndRestoreEnergy());
    }
    public void midleKick()
    {
        if (!canAction) return;
        canAction = false;
        anim.SetTrigger("midleKick");
        SoundManager.Instance.PlayVFXSound(1);
        StartCoroutine(ReduceAndRestoreEnergy());
    }
    private IEnumerator ReduceAndRestoreEnergy()
    {
        float originalEnergy = energy;
        float reduceDuration = 1f; // Duration over which energy is reduced to 0

        float elapsedTime = 0f;

        // Gradually reduce energy to 0
        while (elapsedTime < 0.5f)
        {
            energy = Mathf.Lerp(originalEnergy, 0, elapsedTime / reduceDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        energy = 0; // Ensure energy is fully reduced
        yield return new WaitForSeconds(1f); // Wait for n seconds

        elapsedTime = 0f;
        // Gradually restore energy to original value
        while (elapsedTime < restoreDuration)
        {
            energy = Mathf.Lerp(0, originalEnergy, elapsedTime / restoreDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canAction = true;
        energy = originalEnergy; // Ensure energy is fully restored
    }
    public void highKick() 
    {
        if (!canAction) return;
        canAction = false;
        anim.SetTrigger("highKick");
        StartCoroutine(ReduceAndRestoreEnergy());
        SoundManager.Instance.PlayVFXSound(1);
    }
    public void AvoidHit()
    {
        if (!canAction) return;
        anim.SetTrigger("avoid");
    }
    public void UnActiveColider()
    {
        Col.enabled = false;
        
    }
    public void ActiveColider()
    {
        Col.enabled = true;
        Debug.Log(Col.enabled); 
    }
    public void Hitted()
    {
        anim.SetTrigger("hitted");
    }

    private void OnTriggerEnter(Collider other)
    {
         // Xử lý va chạm cho PlayerLeg
  
        // Xử lý va chạm cho bodyPlayer
        if (this.gameObject.CompareTag("Player"))
        {
            if (other.CompareTag("Arm"))
            {
                Hitted();
                hp -= 50;
            }
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
