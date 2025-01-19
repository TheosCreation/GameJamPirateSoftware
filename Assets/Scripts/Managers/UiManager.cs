using UnityEngine;

public class UiManager : Singleton<UiManager>
{
    [Header("UI Screens")]
    public PlayerHud playerHud;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private UpgradeScreen upgradeScreen;

    private void Start()
    {
        OpenPlayerHud();
    }

    public void PauseMenu(bool isPaused)
    {
        pauseMenu.SetActive(isPaused);
        playerHud.SetActive(!isPaused);
    }

    public void OpenPlayerHud()
    {
        playerHud.SetActive(true);
        pauseMenu.SetActive(false);
        upgradeScreen.SetActive(false);
    }

    public void OpenUpgradeScreen()
    {
        upgradeScreen.SetActive(true);
    }
}
