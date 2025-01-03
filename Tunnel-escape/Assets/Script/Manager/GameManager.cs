using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class GameManager : MonoBehaviour
{
    [SerializeField] private UnityEngine.Playables.PlayableDirector introTimeline; // Timeline
    public UnityEngine.Playables.PlayableDirector outroTimeline;
    public Enemy enemy;
    public Player player;
    public bool GameOver { get; private set; } = false;
    public void Awake()
    {
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();

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
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    
    void Update()
    {
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
             if (outroTimeline != null)
            {
                outroTimeline.stopped += OnOuttroFinished;
                outroTimeline.Play();
            }
            Subject.NotifyObservers("RunAway");
           
        }
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
        float[] lanes = new float[] { -1.5f, 0f, 1.5f };
        float lane = lanes[Random.Range(0, lanes.Length)];

        // Spawn vật thể ở vị trí trước mặt Player
        Vector3 spawnPosition = new Vector3(transform.position.x + spawnDistance, 8.2f, transform.position.z +lane);
        GameObject spawnedObject = Instantiate(obj, spawnPosition, Quaternion.identity);

        // Cho vật thể di chuyển ngược lại về hướng Player
        spawnedObject.GetComponent<ObjectMover>().SetSpeed(moveSpeed);
    }
}
