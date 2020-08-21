using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour, IPointerDownHandler
{
    public enum Buttons
    {
        none,
        Continue,
        MainMenu,
        Restart
    }
    public Buttons select;

    public GameObject canvasObj;

    public void Pause()
    {
        canvasObj.SetActive(!canvasObj.active);
        if (canvasObj.active) Time.timeScale = 0;
        else Time.timeScale = 1;
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

    public void OnPointerDown(PointerEventData eventData)
    {
        switch ((int)select)
        {
            case 1:
                Pause();
                break;
            case 2:
                MainMenu();
                break;
            case 3:
                Restart();
                break;
        }
    }
}
