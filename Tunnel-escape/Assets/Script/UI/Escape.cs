using UnityEngine;
using UnityEngine.UI;

public class Escape : CanvasUI
{
    public Text  textCoin;
    public Text textTimer;
    public Player player;
    public GameManager gameManager; 
    public void PauseBtn()
    {
        UIManager.Instance.OpenUI<Pause>();
        Time.timeScale = 0;
    }
    public void Awake()
    {
        player = FindObjectOfType<Player>();
        gameManager = FindObjectOfType<GameManager>();
    }
   
    void Update()
    {
        if(player == null)
        {
            player = FindObjectOfType<Player>();
        }
        if(gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        // Cập nhật thời gian đếm ngược
        if (gameManager != null)
        {
            int countdownTime = Mathf.Max(0, Mathf.FloorToInt(gameManager.countdownTime));
            textTimer.text = countdownTime.ToString();
           
        }
        textCoin.text =CoinManager.Instance.GetCoins().ToString();
    }
    public void LeftArrow()
    {
        if (player.currentLane < player.lanes.Length - 1  && !player.isMoving)
        {
            player.currentLane++;
            player.HandleMovement(player.currentLane);
        }
       
        return;
       
    }
    public void RightArrow()
    {
        if ( player.currentLane > 0 && !player.isMoving)
        {
            player.currentLane--;
            player.HandleMovement(player.currentLane);
        }
        return; 
   
    }
}
