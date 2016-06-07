using UnityEngine;
using System.Collections;

/// <summary>
/// Provide common interface for input device
/// </summary
public class InputController : MonoBehaviour
{
    /// <summary>
    /// Possible user actions on the device
    /// </summary
    public enum CursorAction
    {
        CursorPressed,
        CursorUnpressed
    };

    public delegate void CursorActionCallback(CursorAction userAction);
    protected CursorActionCallback m_cursorActionCallback;

    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Callback register for user action with input device
    /// </summary
    public void SetCursorActionCallback(CursorActionCallback cursorActionCallback)
    {
        m_cursorActionCallback = cursorActionCallback;
    }

    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Difference of X screen position from previouse frame
    /// </summary
    public virtual float GetOffsetXFromPrevFrame() { return 0.0f; }
    /// <summary>
    /// Difference of Y screen position from previouse frame
    /// </summary
    public virtual float GetOffsetYFromPrevFrame() { return 0.0f; }
    /// <summary>
    /// Reset device state
    /// </summary
    public virtual void ResetState() {}
}
