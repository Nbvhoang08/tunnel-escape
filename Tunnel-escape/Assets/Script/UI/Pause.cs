using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : CanvasUI
{
    // Start is called before the first frame update
    public  void Resume()
    {
        Time.timeScale = 1;
        UIManager.Instance.CloseUI<Pause>(0.1f);
        SoundManager.Instance.PlayClickSound();
    }

}
