using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class BattleCanvas : CanvasUI
{
    public Slider playerHealth;
    public Slider enemyHealth;
    public Slider playerEnergy;
    public Slider enemyEnergy;
    public Enemy enemy;
    public Player player;
    public Text LevelName;
    public Button buffButton;
    public Button upButton;
    public Button downButton;
    public Button leftButton;
    public Button rightButton;
    public void Awake()
    {
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
    }
    public void Update()
    {   
        if(enemy == null)
        {
            enemy = FindObjectOfType<Enemy>();
        }
        if(player == null)
        {
            player = FindObjectOfType<Player>();
        }

        if(playerHealth != null)
        {
            playerHealth.value = player.hp / player.maxHp;
        }
        if(enemyHealth != null)
        {
    ;
            enemyHealth.value = enemy.hp / enemy.maxHp;
        }
        if(playerEnergy != null)
        {
            playerEnergy.value = player.energy/player.maxEnergy;
        }if(enemyEnergy != null)
        {
            enemyEnergy.value = enemy.energy;
        }
        if (LevelName != null)
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            LevelName.text = "Level " + (currentSceneIndex + 1);
        }
    
    }
    public void buffBtn()
    {
        UIManager.Instance.OpenUI<Buff>();
        Time.timeScale = 0;
        ApplyHoverEffect(buffButton);
    }
    public void UpBtn()
    {
        player.highKick();
       ApplyHoverEffect(upButton);
    }
    public void DownBtn()
    {
        player.lowKick();
        ApplyHoverEffect(downButton);
    }
    public void leftBtn()
    {
        player.AvoidHit();
        ApplyHoverEffect(leftButton);
    }
    public void RightBtn()
    {
        player.midleKick();
        ApplyHoverEffect(rightButton);
    }
     // Phương thức để áp dụng hiệu ứng hover
    private void ApplyHoverEffect(Button button)
    {
        button.transform.DOScale(1.2f, 0.2f).OnComplete(() =>
        {
            button.transform.DOScale(1f, 0.2f);
        });
    }
}
