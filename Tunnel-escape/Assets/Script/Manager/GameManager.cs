using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class GameManager : MonoBehaviour ,IObserver 
{
    [SerializeField] private UnityEngine.Playables.PlayableDirector introTimeline; // Timeline
    public UnityEngine.Playables.PlayableDirector outroTimeline;
    public Enemy enemy;
    [SerializeField] private bool isCountingDown = false;
    public float countdownTime = 30f;
    public Player player;
    public bool GameOver { get; private set; } = false;
    public static GameManager Instance { get; private set; }
    public void Awake()
    {
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
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
    public void OnNotify(string eventName,object eventData)
    {
        if(eventName == "End")
        {
            CancelInvoke("SpawnObject");
        }
        
    }
    private void StartCountdown()
    {
        countdownTime = 30f; // Đặt lại thời gian đếm ngược
        isCountingDown = true; // Bắt đầu đếm ngược
    }

    void Start()
    {
        // Đăng ký sự kiện khi Scene được chuyển đổi
          // Lắng nghe sự kiện kết thúc Timeline
        if (introTimeline != null)
        {
            introTimeline.stopped += OnIntroFinished;
            introTimeline.Play();
        }
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene current)
    {
        DOTween.KillAll(); // Hủy tất cả các tween khi Scene đóng.
    }

    private void OnDestroy()
    {
        // Đảm bảo gỡ sự kiện để tránh lỗi
        if (Instance == this)
        {
            Subject.UnregisterObserver(this); // Hủy đăng ký observer
            CancelInvoke("SpawnObject");
            Instance = null; // Làm rỗng instance
        }
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    
    void Update()
    {
        if (isCountingDown)
        {
            // Giảm thời gian đếm ngược
            countdownTime -= Time.deltaTime;
            // Kiểm tra nếu hết thDeời gian
            if (countdownTime <= 0)
            {
                EndGame();
                isCountingDown = false;
                return;
            }
        }
        if(GameOver) return;
        if(enemy == null)
        {
            enemy = FindObjectOfType<Enemy>();
        }
        if(player == null)
        {
            player = FindObjectOfType<Player>();
        }
       
        CheckWinLoseCondition();
    }
    private void EndGame()
    {
        Subject.NotifyObservers("End");
        CancelInvoke("SpawnObject");
        UIManager.Instance.OpenUI<WellDone>();
        Time.timeScale = 0;
    }

    private void CheckWinLoseCondition()
    {
        if (player.isDead && !GameOver)    
        {

            GameOver = true;
            StartCoroutine(LoseAction());
        }
        else if (enemy.isDead && !GameOver) 
        {
            GameOver = true;
            UIManager.Instance.CloseUI<BattleCanvas>(0.5f);
            StartCoroutine(WinAction());
        }
    }
    IEnumerator WinAction()
    {
        yield return new WaitForSeconds(3); 
 
        if (outroTimeline != null)
        {
            outroTimeline.stopped += OnOuttroFinished;
            outroTimeline.Play();
        }
        Subject.NotifyObservers("RunAway");    
    }
    IEnumerator LoseAction()
    {
        yield return new WaitForSeconds(1); 
        UIManager.Instance.OpenUI<Lose>();
        Time.timeScale = 0;
    }
    private void OnIntroFinished(UnityEngine.Playables.PlayableDirector director)
    {
        UIManager.Instance.OpenUI<StartCanvas>();
    }
    private void OnOuttroFinished(UnityEngine.Playables.PlayableDirector director)
    {
        StartCountdown();   
        Subject.NotifyObservers("StartEscape");
        InvokeRepeating("SpawnObject", 0f, spawnInterval);
    }
    
    public GameObject[] objectPrefabs; // Các loại vật thể để spawn
    public float spawnDistance = 50f; // Khoảng cách spawn vật thể trước Player
    public float spawnInterval = 2f; // Thời gian giữa mỗi lần spawn
    public float moveSpeed = 3f; // Tốc độ di chuyển của vật thể
    void SpawnObject()
    {
        // Chọn ngẫu nhiên vật thể để spawn
        GameObject obj = objectPrefabs[Random.Range(0, objectPrefabs.Length)];

        // Chọn ngẫu nhiên hàng (trục Z) để spawn
        float[] lanes = new float[] { -3f, 0f, 3f };
        float lane = lanes[Random.Range(0, lanes.Length)];

        // Spawn vật thể ở vị trí trước mặt Player
        Vector3 spawnPosition = new Vector3(transform.position.x + spawnDistance, 0.5f, 18.8f +lane);
        GameObject spawnedObject = Instantiate(obj, spawnPosition, obj.transform.rotation);

        // Cho vật thể di chuyển ngược lại về hướng Player
        spawnedObject.GetComponent<ObjectMover>().SetSpeed(moveSpeed);
    }
}
