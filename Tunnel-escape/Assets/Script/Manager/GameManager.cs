using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Subject
{
    [SerializeField] private UnityEngine.Playables.PlayableDirector introTimeline; // Timeline


    private bool battleStarted = false;

    private void Start()
    {
      
        // Lắng nghe sự kiện kết thúc Timeline
        if (introTimeline != null)
        {
            introTimeline.stopped += OnIntroFinished;
            introTimeline.Play();
        }
    }

    private void OnIntroFinished(UnityEngine.Playables.PlayableDirector director)
    {
        Debug.Log("Intro finished. Showing Start Battle button...");

        UIManager.Instance.OpenUI<StartCanvas>();
    }

    // Gọi từ UI Button để bắt đầu trận đấu
    public void StartBattle()
    {
        if (battleStarted) return; // Đảm bảo không gọi nhiều lần
        battleStarted = true;

        Debug.Log("Start Battle clicked. Notifying observers...");
        NotifyObservers("StartBattle", null); // Thông báo sự kiện "StartBattle"
    }
}
