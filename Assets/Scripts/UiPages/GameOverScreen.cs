public class GameOverScreen : UiPage
{
    public void ExitToMainMenu()
    {
        GameManager.Instance.ExitToMainMenu();
    }

    public void PlayAgain()
    {
        GameManager.Instance.StartGame();
    }
}