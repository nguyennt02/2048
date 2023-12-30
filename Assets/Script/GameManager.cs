using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get => instance; }

    public TileBoard board;
    public CanvasGroup gameOver;

    public TextMeshProUGUI txtBestScore;
    public TextMeshProUGUI txtScore;

    public GameObject audioControler;
    private int score;


    void Start()
    {
        if(instance) Debug.LogError("Ton tai mot GameManager"); 
        instance = this;
        NewGame();
    }

    public void NewGame(){
        gameOver.alpha = 0f;
        gameOver.interactable = false;

        SetScore(0);
        txtBestScore.text = LoadBestScore().ToString();

        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true;
        AudioManager.Instance.PlayMusic("Theme");
    }

    public void GameOver(){
        board.enabled = false;
        gameOver.interactable = true;

        StartCoroutine(Fade(gameOver, 1f, 1f));
    }

    private IEnumerator Fade(CanvasGroup gameOver, float to, float delay){
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        float duration = 0.5f;

        float from = gameOver.alpha;
        while(elapsed < duration){
            gameOver.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        gameOver.alpha = to;
    }

    public void IncreaseScore(int poins){
        SetScore(score + poins);
    }

    private void SetScore(int score){
        this.score = score;
        txtScore.text = score.ToString();

        SaveBestScore();
    }

    private void SaveBestScore(){
        int bestScore = LoadBestScore();
        if(score > bestScore){
            PlayerPrefs.SetInt("bestScore", score);
        }
    }

    private int LoadBestScore(){
        return PlayerPrefs.GetInt("bestScore", 0);
    }

    public void AtivePanlAudioController(){
        audioControler.SetActive(true);
    }
    public void UnAtivePanlAudioController(){
        audioControler.SetActive(false);
    }
}
