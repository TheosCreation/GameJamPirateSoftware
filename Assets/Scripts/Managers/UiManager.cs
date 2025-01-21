using UnityEngine;

public class UiManager : Singleton<UiManager>
{
    [Header("UI Screens")]
    public PlayerHud playerHud;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private UpgradeScreen upgradeScreen;
    [Header("Camera")]
    [SerializeField] private Camera uiCamera;

    private void Start()
    {
        OpenLoadingScreen();
    }

    public void PauseMenu(bool isPaused)
    {
        pauseMenu.SetActive(isPaused);
        playerHud.SetActive(!isPaused);
    }

    public void OpenLoadingScreen()
    {
        loadingScreen.SetActive(true);
        playerHud.SetActive(false);
        pauseMenu.SetActive(false);
        upgradeScreen.SetActive(false);
    }

    public void OpenPlayerHud()
    {
        playerHud.SetActive(true);
        pauseMenu.SetActive(false);
        upgradeScreen.SetActive(false);
        loadingScreen.SetActive(false);
        gameOverScreen.SetActive(false);
    }

    public void OpenUpgradeScreen()
    {
        upgradeScreen.SetActive(true);
        pauseMenu.SetActive(false);
        playerHud.SetActive(false);
        loadingScreen.SetActive(false);
    }

    public void OpenGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        upgradeScreen.SetActive(false);
        pauseMenu.SetActive(false);
        playerHud.SetActive(false);
        loadingScreen.SetActive(false);

        uiCamera.gameObject.SetActive(true);
        uiCamera.enabled = true;
    }
}
