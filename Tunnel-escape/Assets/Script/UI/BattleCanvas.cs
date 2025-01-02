using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCanvas : MonoBehaviour
{
    public void UpBtn()
    {
        Player.Instance.highKick();
       
    }
    public void DownBtn()
    {
        Player.Instance.lowKick();

    }
    public void leftBtn()
    {
        Player.Instance.AvoidHit();

    }
    public void RightBtn()
    {
        Player.Instance.midleKick();

    }
}
