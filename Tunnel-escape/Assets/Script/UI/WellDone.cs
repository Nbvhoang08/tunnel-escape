using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;  
public class WellDone : CanvasUI
{
    public void NextLevel()
    {
        Time.timeScale = 1;
        StartCoroutine(NextSceneAfterDelay(0.5f));
        SoundManager.Instance.PlayClickSound();
    }
    
    private IEnumerator NextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UIManager.Instance.CloseAll();
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0; // Nếu không có scene kế tiếp, load scene đầu tiên
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
