using UnityEngine;

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
}