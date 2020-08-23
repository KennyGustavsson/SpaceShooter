using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject gameOverScreen;
    public Text finalScoreText;
    public int score;

    private Text _scoreText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        _scoreText = GameObject.Find("ScoreDisplay").GetComponent<Text>();
        Cursor.visible = false;
    }

    public void RestartGame()
    {
        finalScoreText.text= $"Score: {score}";
        gameOverScreen.SetActive(true);
        StartCoroutine(RestartTimer());
    }

    public void AddScore(int scorePoints)
    {
        score += scorePoints;
        _scoreText.text = $"Score: {score}";
    }

    private IEnumerator RestartTimer()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }
}