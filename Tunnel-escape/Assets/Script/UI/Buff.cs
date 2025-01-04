using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buff : CanvasUI
{
    public Slider DefSlider;
    public Slider CDSlider;
    public Text Coins;
    void OnEnable()
    {
        UpdateCoins();
   
    }
    public void Resume()
    {
        Time.timeScale = 1;
        UIManager.Instance.CloseUI<Buff>(0.2f);

    }
    public void buffDefBtn()
    {
        SoundManager.Instance.PlayClickSound();
        if(CoinManager.Instance.GetCoins() > 150)
        {
            StatsManager.Instance.IncreaseDefense(0.1f);
            CoinManager.Instance.RemoveCoins(150);
            UpdateDefSlider();
            UpdateCoins();
        }
        return;
    }
    public void reduceCoolDown()
    {
        SoundManager.Instance.PlayClickSound();
        if(CoinManager.Instance.GetCoins() >=100)
        {
            
            StatsManager.Instance.IncreaseCooldownReduction(0.05f);
            CoinManager.Instance.RemoveCoins(0);
            UpdateCDSlider();
            UpdateCoins();
        }
        return;
    
    }
    private void UpdateCoins()
    {
        Coins.text = CoinManager.Instance.GetCoins().ToString();    
    }
    private void UpdateDefSlider()
    {
        // Giả sử StatsManager.Instance.GetDefense() trả về giá trị phòng thủ hiện tại
        DefSlider.value = StatsManager.Instance.GetDefense()/0.8f;
    }

    private void UpdateCDSlider()
    {
        // Giả sử StatsManager.Instance.GetCooldownReduction() trả về giá trị giảm thời gian hồi chiêu hiện tại
        CDSlider.value = (1-StatsManager.Instance.GetCooldownReduction())/1f;
    }
}
