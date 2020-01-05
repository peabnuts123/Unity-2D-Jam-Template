using UnityEngine;
using UnityEngine.EventSystems;

public class PanelSelector : MonoBehaviour
{
    // Public references
    public GameObject[] panels;


    /// <summary>
    /// Deactivate all panels except for the target selected panel
    /// </summary>
    /// <param name="selectedPanel">Panel to activate</param>
    public void SelectPanel(GameObject selectedPanel)
    {
        // Disable all panels
        foreach (GameObject panel in this.panels)
        {
            panel.SetActive(false);
        }

        // Enable selected panel
        selectedPanel.SetActive(true);

        // Deselect any selected controls
        EventSystem.current.SetSelectedGameObject(null);
    }
}
