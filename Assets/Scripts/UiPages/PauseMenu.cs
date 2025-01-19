public class PauseMenu : UiPage
{
    public void Resume()
    {
        PauseManager.Instance.SetPaused(false);
    }
}