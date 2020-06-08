using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText = null;
    [SerializeField] Text _ammoText = null;

    [SerializeField] private Sprite[] _liveSprites = null;
    [SerializeField] private Image _livesIMG = null;
    [SerializeField] GameObject _gameOverText = null;
    [SerializeField] GameObject _restartText = null;
    [SerializeField] private GameManager gameManager = null;

    private PlayerController player;


    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score " + 0;
        _gameOverText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        // display img sprite
        //give it a new one based on the currentLives index
        _livesIMG.sprite = _liveSprites[currentLives];

        if(currentLives == 0)
        {
            GameOverSequence();
        }

    }

    void GameOverSequence()
    {
        StartCoroutine(GameOverFlicker());
        _restartText.SetActive(true);
        gameManager.IsGameOver();
    }

    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOverText.SetActive(true);
            yield return new WaitForSeconds(.5f);
            _gameOverText.SetActive(false);
            yield return new WaitForSeconds(.5f);
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

}
