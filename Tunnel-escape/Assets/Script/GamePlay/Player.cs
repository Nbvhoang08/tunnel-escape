using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour , IObserver
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
    public Player Instance { get; private set; }   
    public float[] lanes = new float[] { -5f, 0f, 5f }; // Ba hàng Z: -1.5, 0, 1.5
    public int currentLane = 1; // Bắt đầu ở hàng giữa (Z = 0)
    public bool RunAway;
    public bool isMoving = false; // Kiểm tra trạng thái đang di chuyển
    public float damage = 10;
    public void Awake()
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
        RunAway = false;
    }
    public void OnDestroy()
    {
         if (Instance == this)
        {
            Subject.UnregisterObserver(this); // Hủy đăng ký observer
            Instance = null; // Làm rỗng instance
        }

    }
   public void OnNotify(string eventName, object eventData)
    {
        if (eventName == "RunAway")
        {
            anim.SetTrigger("run");
            ActiveColider();
            RunAway = true;
        }
    }
    public void HandleMovement(int currentLane)
    {
        if (isMoving) return; // Không làm gì nếu chưa nhận lệnh "RunAway" hoặc đang di chuyển

        // Tính toán vị trí đích dựa trên hàng hiện tại
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y,18.8f + lanes[currentLane]);
        
        // Bắt đầu di chuyển đến vị trí đích
        StartCoroutine(MoveToPosition(targetPosition));
    }
    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        isMoving = true; // Đánh dấu rằng người chơi đang di chuyển
        float moveSpeed = 10f; // Tốc độ di chuyển
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            // Di chuyển người chơi đến vị trí đích
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null; // Chờ một frame
        }

        // Đảm bảo người chơi đúng vị trí đích
        transform.position = targetPosition;

        isMoving = false; // Kết thúc di chuyển
    }
    void Start()
    {
        canAction = true;
        Col = GetComponent<Collider>();
        energy = maxEnergy;
        hp = maxHp;
        
    }
    void Update()
    {
        if (isDead)
        {
            Die();
            return;
        }
    }
    public GameObject dieEffectPrefab;
    public void Die()
    {
        VibrateDevice();
         // Spawn hiệu ứng chết
        if (dieEffectPrefab != null)
        {
            Instantiate(dieEffectPrefab, transform.position, Quaternion.identity);
        }
        // Vô hiệu hóa đối tượng Enemy
        gameObject.SetActive(false);
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
        while (elapsedTime < restoreDuration*StatsManager.Instance.GetCooldownReduction())
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
    public bool avoiding;
    public void AvoidHit()
    {
        if(avoiding) return;
        avoiding = true;
        anim.SetTrigger("avoid");
    }
    public void UnActiveColider()
    {
        Col.enabled = false;
        
    }
    public void ActiveColider()
    {
        if(avoiding)
        {
            avoiding= false;
        }
      
        if(!Col.enabled)
        {
            Col.enabled = true;
        }
        return;
    }
    public void Hitted()
    {
        anim.SetTrigger("hitted");
    }
    void VibrateDevice()
    {
        Handheld.Vibrate();
    }
    private void OnTriggerEnter(Collider other)
    {
        // Xử lý va chạm cho bodyPlayer
        if (this.gameObject.CompareTag("Player"))
        {
            if (other.CompareTag("Arm"))
            {
                Hitted();
                VibrateDevice();
                SoundManager.Instance.PlayVFXSound(4);
                hp -= damage -damage*StatsManager.Instance.GetDefense();
            }
        }
        
        if(other.CompareTag("coin"))
        {
            Destroy(other.gameObject);
            SoundManager.Instance.PlayVFXSound(3);
            CoinManager.Instance.AddCoins(20);
        }
        
        if(other.CompareTag("Pillar"))
        {
            Subject.NotifyObservers("End");
            StartCoroutine(Knockback());
            anim.SetTrigger("hitted");
            VibrateDevice();
            StartCoroutine(EndGame());  
            SoundManager.Instance.PlayVFXSound(4);
        }
        
    }
    private IEnumerator Knockback()
{
    float knockbackDuration = 0.25f;
    float elapsedTime = 0f;
    Vector3 originalPosition = transform.position;
    Vector3 knockbackDirection = new Vector3(-1,0,0) ; // Hướng ngược lại

    while (elapsedTime < knockbackDuration)
    {
        transform.position = originalPosition + knockbackDirection * (elapsedTime / knockbackDuration);
        elapsedTime += Time.deltaTime;
        yield return null;
    }

    // Đảm bảo vị trí cuối cùng sau knockback
    transform.position = originalPosition + knockbackDirection;
}
    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1);
        Time.timeScale = 0;
        UIManager.Instance.OpenUI<WellDone>();
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
