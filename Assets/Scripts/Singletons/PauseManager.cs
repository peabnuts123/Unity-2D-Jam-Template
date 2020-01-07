using UnityEngine;

public class PauseManager : MonoBehaviour
{
    // Public references
    [NotNull(IgnorePrefab = true)]
    public GameObject pauseMenu;

    // Private state
    private bool isGamePaused = false;

    public void SetGamePaused(bool isPaused)
    {
        this.isGamePaused = isPaused;
        this.pauseMenu.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            SetGamePaused(!this.isGamePaused);
        }
    }

    void OnDestroy()
    {
        // If the PauseManager is destroyed (e.g. changing scenes),
        //  unpause the game
        this.SetGamePaused(false);
    }

    public bool IsPaused
    {
        get { return this.isGamePaused; }
    }
}