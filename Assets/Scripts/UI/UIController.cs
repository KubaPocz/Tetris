using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Player1")]
    private string player1Nickname = "Player1";
    public TextMeshProUGUI player1ScoreText;
    public GameObject player1BlockPreview;
    public TextMeshProUGUI player1NicknameText;


    [Header("Player2")]
    private string player2Nickname = "Player2";
    public TextMeshProUGUI player2ScoreText;
    public GameObject player2BlockPreview;
    public TextMeshProUGUI player2NicknameText;

    [Header("Result")]
    public GameObject result_panel;
    public TextMeshProUGUI result_text;


    private void Start()
    {
        result_panel.SetActive(false);
        player1ScoreText.text = "0";
        player2ScoreText.text = "0";

        player1NicknameText.text = player1Nickname;
        player2NicknameText.text = player2Nickname;


        Events.OnUpdateScore += UpdateScore;
        Events.OnUpdateNextBlock += UpdateNextBlock;
        Events.OnEndGame += EndGame;
    }
    public void EndGame(int looserPlayerID)
    {
        Time.timeScale = 0;
        result_panel.SetActive(true);
        if (looserPlayerID == 1)
        {
            result_text.text = $"{player2Nickname} won!\nScore:{player2ScoreText.text}";
        }
        else if (looserPlayerID == 2)
        {
            result_text.text = $"{player1Nickname} won!\nScore:{player1ScoreText.text}";
        }
        else
        {
            result_text.text = "Error";
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadSceneAsync("1");
    }
    public void UpdateScore(int playerID, int playerScore)
    {
        if(playerID == 1)
        {
            player1ScoreText.text = playerScore.ToString();
        }
        else if(playerID == 2)
        {
            player2ScoreText.text = playerScore.ToString();
        }
    }
    public void UpdateNextBlock(int playerId, GameObject nextBlock)
    {
        var blockPreview = playerId==1?player1BlockPreview : player2BlockPreview;

        foreach (Transform child in blockPreview.transform)
        {
            Destroy(child.gameObject);
        }
        Instantiate(nextBlock.GetComponent<BlockPreview>().blockImage.transform, blockPreview.transform);
    }
}
