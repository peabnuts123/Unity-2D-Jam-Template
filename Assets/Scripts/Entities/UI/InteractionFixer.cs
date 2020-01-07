using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Change (or "fix") the behaviour of Unity's event system a bit with respect to
/// highlighting and selecting between button/joystick and mouse inputs
///
/// @NOTE that it is expected other objects are in charge of updating `EventSystem.current.firstSelectedGameObject`
/// </summary>
public class InteractionFixer : MonoBehaviour
{
    // Private state
    /// <summary>
    /// Coordinate of mouse cursor last time Update() was called
    /// </summary>
    private Vector3 oldMousePosition;


    public void Update()
    {
        // Calculate mouse distance since last update
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 mouseDelta = currentMousePosition - oldMousePosition;
        this.oldMousePosition = currentMousePosition;

        // If there's nothing selected and you press a keyboard/joystick/button, select something
        // @NOTE that it is assumed that different panels will update `EventSystem.current.firstSelectedGameObject`
        //  as they are enabled / disabled
        if (!IsSelected() && Input.GetButton("Vertical"))
        {
            EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
        }

        // If something is selected and the mouse is moved, deselect stuff
        if (IsSelected() && mouseDelta.sqrMagnitude > 0.1)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    /// <summary>
    /// Whether anything is currently selected in the event system
    /// </summary>
    public bool IsSelected()
    {
        return EventSystem.current.currentSelectedGameObject != null;
    }
}