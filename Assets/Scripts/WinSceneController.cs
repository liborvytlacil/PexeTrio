using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSceneController : MonoBehaviour
{
    private static string HIGH_SCORE_KEY = "highscore";
    [SerializeField]
    private TextMesh triesText;
    [SerializeField]
    private TextMesh highScoreText;
    [SerializeField]
    private TextMesh newHighScoreMessage;

    // Start is called before the first frame update
    void Start()
    {
        // Display the current tries count (score)
        int tries = GameState.GetTries();
        triesText.text = "Tries: " + tries;

        // Check if hiscore has been beaten
        if (!PlayerPrefs.HasKey(HIGH_SCORE_KEY))
        {
            SetHighScoreText(tries);
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, tries);
        } else
        {
            int highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY);
            if (tries < highScore)
            {
                SetHighScoreText(tries);
                PlayerPrefs.SetInt(HIGH_SCORE_KEY, tries);
            } else
            {
                SetHighScoreText(highScore);
                newHighScoreMessage.gameObject.SetActive(false);
            }

        }
    }

    private void SetHighScoreText(int value)
    {
        highScoreText.text = "HiScore: " + value;
    }

    public void Restart()
    {
        GameState.SetTries(0);
        SceneManager.LoadScene("SampleScene");
    }
}
