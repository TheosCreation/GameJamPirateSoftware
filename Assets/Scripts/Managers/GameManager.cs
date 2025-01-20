using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonPersistent<GameManager>
{
    
    public void ExitToMainMenu()
    {
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
}