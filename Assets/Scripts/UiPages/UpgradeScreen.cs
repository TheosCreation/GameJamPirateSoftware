public class UpgradeScreen : UiPage
{
    public void SelectUpgrade()
    {
        gameObject.SetActive(false); 
        PauseManager.Instance.canUnpause = true;
        PauseManager.Instance.SetPaused(false); 
    }
}