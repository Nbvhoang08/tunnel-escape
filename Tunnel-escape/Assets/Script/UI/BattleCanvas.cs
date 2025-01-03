using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCanvas : CanvasUI
{
    public Slider playerHealth;
    public Slider enemyHealth;
    public Slider playerEnergy;
    public Slider enemyEnergy;
    public Enemy enemy;
    public Player player;
    public void Awake()
    {
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
    }
    public void Update()
    {   
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
    
    }

    public void UpBtn()
    {
        player.highKick();
       
    }
    public void DownBtn()
    {
        player.lowKick();

    }
    public void leftBtn()
    {
        player.AvoidHit();

    }
    public void RightBtn()
    {
        player.midleKick();

    }
}
