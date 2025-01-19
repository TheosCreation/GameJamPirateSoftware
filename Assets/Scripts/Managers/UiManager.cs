using UnityEngine;

public class UiManager : Singleton<UiManager>
{
    [Header("UI Screens")]
    public PlayerHud playerHud;
    [SerializeField] private PauseMenu pauseMenu;

    public void PauseMenu(bool isPaused)
    {
        pauseMenu.SetActive(isPaused);
        playerHud.SetActive(!isPaused);
    }

    public void OpenPlayerHud()
    {
        playerHud.SetActive(true);
        pauseMenu.SetActive(false);
    }
}
