using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UnityEngine.Playables.PlayableDirector introTimeline; // Timeline
    public Enemy enemy;
    public Player player;
    public bool GameOver { get; private set; } = false;
    public void Awake()
    {
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
    }
    private void Start()
    {
        // Lắng nghe sự kiện kết thúc Timeline
        if (introTimeline != null)
        {
            introTimeline.stopped += OnIntroFinished;
            introTimeline.Play();
        }
    }
    void Update()
    {
        if(GameOver) return;
        CheckWinLoseCondition();
    }

    private void CheckWinLoseCondition()
    {
        if (player.isDead && !GameOver)    
        {
            Debug.Log("Player thua cuộc!");
            // Thực hiện các hành động khi thua cuộc, ví dụ: hiển thị màn hình thua cuộc
            // ...
            GameOver = true;
            StartCoroutine(LoseAction());
        }
        else if (enemy.isDead && !GameOver) 
        {
            Debug.Log("Player chiến thắng!");
            GameOver = true;
           

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
}
