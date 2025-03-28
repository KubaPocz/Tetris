using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Player 1 UI")]
    private string _player1Nickname = "Player1";
    public TextMeshProUGUI player1ScoreText;
    public GameObject player1BlockPreview;
    public TextMeshProUGUI player1NicknameText;

    [Header("Player 2 UI")]
    private string _player2Nickname = "Player2";
    public TextMeshProUGUI player2ScoreText;
    public GameObject player2BlockPreview;
    public TextMeshProUGUI player2NicknameText;

    [Header("Result Panel")]
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;

    private void Start()
    {
        // Hide result panel
        resultPanel.SetActive(false);

        // Set starting scores
        player1ScoreText.text = "0";
        player2ScoreText.text = "0";

        // Set nicknames
        player1NicknameText.text = _player1Nickname;
        player2NicknameText.text = _player2Nickname;

        // Connect to game events
        Events.OnUpdateScore += UpdateScore;
        Events.OnUpdateNextBlock += UpdateNextBlock;
        Events.OnEndGame += EndGame;
    }

    public void EndGame(int losingPlayerId)
    {
        // Pause game and show winner info
        Time.timeScale = 0;
        resultPanel.SetActive(true);

        if (losingPlayerId == 1)
        {
            resultText.text = $"{_player2Nickname} won!\nScore: {player2ScoreText.text}";
        }
        else if (losingPlayerId == 2)
        {
            resultText.text = $"{_player1Nickname} won!\nScore: {player1ScoreText.text}";
        }
        else
        {
            resultText.text = "Error";
        }
    }

    public void RestartGame()
    {
        // Reload the scene
        SceneManager.LoadSceneAsync("1");
    }

    public void UpdateScore(int playerId, int playerScore)
    {
        // Update player's score text
        if (playerId == 1)
        {
            player1ScoreText.text = playerScore.ToString();
        }
        else if (playerId == 2)
        {
            player2ScoreText.text = playerScore.ToString();
        }
    }

    public void UpdateNextBlock(int playerId, GameObject nextBlock)
    {
        // Get correct preview container
        GameObject blockPreview = playerId == 1 ? player1BlockPreview : player2BlockPreview;

        // Remove previous block preview
        foreach (Transform child in blockPreview.transform)
        {
            Destroy(child.gameObject);
        }

        // Add new block preview
        Instantiate(nextBlock.GetComponent<BlockPreview>().blockImage.transform, blockPreview.transform);
    }
}
