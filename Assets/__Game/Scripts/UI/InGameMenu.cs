using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    public GameObject canvasObj;

    public void Pause()
    {
        canvasObj.SetActive(!canvasObj.activeSelf);
        if (canvasObj.activeSelf)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
        }
        else 
        {
            Time.timeScale = 1;
            Cursor.visible = false;
        }
    }

    public void MainMenu()
    {
        Debug.Log("MainMenu");
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        Debug.Log("Restart");
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}
