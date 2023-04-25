using UnityEngine;
using TMPro;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private Config gameConfig;
    [SerializeField] private Score score;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highscoreText;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject newHighscore;
    private Animator deathScreenAnim;
    bool isHighscore;

    void Start()
    {
        PlayerController.Death += playerDeath;
        gameConfig.highscore = PlayerPrefs.GetInt("Highscore", 0);
        deathScreenAnim = deathScreen.GetComponent<Animator>();
    }

    private void playerDeath()
    {
        deathScreen.SetActive(true);
        scoreText.text = "Score: " + score.score; 
        isHighscore = (score.score > gameConfig.highscore);
        if (isHighscore)
        {
            gameConfig.highscore = score.score;
            PlayerPrefs.SetInt("Highscore", gameConfig.highscore);
            PlayerPrefs.Save();
        }
        highscoreText.text = "Highscore: " + gameConfig.highscore;
        newHighscore.SetActive(isHighscore);
        deathScreenAnim.SetTrigger("Died");
    }

    void OnDisable()
    {
        PlayerController.Death -= playerDeath;
    }
}
