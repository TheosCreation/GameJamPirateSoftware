using UnityEngine;

public class PauseManager : Singleton<PauseManager>
{
    [SerializeField] private bool isPaused = false;
    public bool canUnpause = true;

    private void Start()
    {
        CheckPaused();
    }

    private void CheckPaused()
    {
        if (!canUnpause) return;

        if (isPaused)
        {
            Pause();
        }
        else
        {
            UnPause();
        }
    }

    public void SetPaused(bool pausedStatus)
    {
        isPaused = pausedStatus;
        CheckPaused();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        CheckPaused();
    }

    private void Pause()
    {
        InputManager.Instance.DisableInGameInput();
        if (UiManager.Instance != null)
        {
            UiManager.Instance.PauseMenu(true);
        }
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PauseNoScreen()
    {
        InputManager.Instance.DisableInGameInput();
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;
    }

    private void UnPause()
    {
        Time.timeScale = 1;
        InputManager.Instance.EnableInGameInput();
        UiManager.Instance.PauseMenu(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
}