using DG.Tweening;
using UnityEngine;
public class CamSetUp : MonoBehaviour, IObserver
{
   public static CamSetUp Instance { get; private set; }
    private void Awake()
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
    }
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Subject.UnregisterObserver(this); // Hủy đăng ký observer
            DOTween.Kill(this); // Hủy tất cả tween liên quan đến object này
            Instance = null; // Làm rỗng instance
        }

    }
    public void OnNotify(string eventName, object eventData)
    {
        if (eventName == "startGame")
        {
            SetUP();
            // Subject.UnregisterObserver(this);
        }if (eventName == "StartEscape")
        {
            InitiateCameraMovement();
            Subject.UnregisterObserver(this);
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
    public Vector3 targetPosition = new Vector3(18, 2, 19); 
    // Thời gian di chuyển
    public float moveDuration = 1.5f;

    private void InitiateCameraMovement()
    {
        // Tạo một sequence cho DOTween
        Sequence cameraSequence = DOTween.Sequence();
        
        // Di chuyển camera đến vị trí mới
        cameraSequence.Join(transform.DOMove(targetPosition, moveDuration).SetEase(Ease.InOutQuad));
        
        // Xoay camera tới góc quay mới
        cameraSequence.Join(transform.DORotate(new Vector3(0,90,0), moveDuration, RotateMode.FastBeyond360));

        // Thực hiện hành động khi kết thúc
        cameraSequence.OnComplete(() =>
        {
            if (this == null) return; // Đảm bảo object vẫn tồn tại
            // Thực hiện hành động sau khi di chuyển hoàn tất
            UIManager.Instance.OpenUI<BattleCanvas>(); // Mở UI BattleCanvas
            Debug.Log("Camera moved to target position and rotated to target angle");   
        });

    }

   
}
