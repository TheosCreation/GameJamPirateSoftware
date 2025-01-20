using UnityEngine;

public class StartMenu : UiPage
{
    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    public void OpenOptionMenu()
    {
        MainMenuManager.Instance.OpenPage(MainMenuManager.Instance.optionsPage);
    }

    public void Quit()
    {
        GameManager.Instance.Quit();
    }
}
