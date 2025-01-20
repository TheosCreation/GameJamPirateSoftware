using UnityEngine;

public class StartMenu : UiPage
{
    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    public void Quit()
    {
        GameManager.Instance.Quit();
    }
}
