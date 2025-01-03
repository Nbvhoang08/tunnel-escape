using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  
public class CamSetUp : MonoBehaviour, IObserver
{
   public static CamSetUp Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Hủy các instance cũ
            return;
        }

        Instance = this; // Gán instance mới
        DontDestroyOnLoad(gameObject); // Giữ lại qua các scene
        Subject.RegisterObserver(this); // Đăng ký observer

        SceneManager.sceneLoaded += OnSceneLoaded; // Đăng ký sự kiện load scene
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Subject.UnregisterObserver(this); // Hủy đăng ký observer
            DOTween.Kill(this); // Hủy tất cả tween liên quan đến object này
            Instance = null; // Làm rỗng instance
        }

        SceneManager.sceneLoaded -= OnSceneLoaded; // Hủy đăng ký sự kiện
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene {scene.name} loaded. Làm mới tham chiếu DOTween nếu cần.");
        ReinitializeTweens(); // Làm mới các tween khi cần
    }

    public void OnNotify(string eventName, object eventData)
    {
        if (eventName == "startGame")
        {
            SetUP();
        }
    }

    private void SetUP()
    {
        Sequence sequence = DOTween.Sequence();
        Vector3 targetPosition = new Vector3(0.5f, 1f, 17f);
        float duration = 1.5f;

        sequence.Join(transform.DOMove(targetPosition, duration).SetEase(Ease.InOutQuad));
        sequence.OnComplete(() =>
        {
            if (this == null) return; // Đảm bảo object vẫn tồn tại
            UIManager.Instance.OpenUI<BattleCanvas>();
            Subject.NotifyObservers("LetBattle");
        });
    }

    private void ReinitializeTweens()
    {
        DOTween.Clear(true); // Dọn dẹp các tween cũ
        DOTween.Init(); // Khởi tạo lại DOTween
        Debug.Log("DOTween đã được làm mới sau khi load scene.");
    }
}
