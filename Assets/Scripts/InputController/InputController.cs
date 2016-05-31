using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour
{
    public enum CursorAction
    {
        CursorPressed,
        CursorUnpressed
    };

    public delegate void CursorActionCallback(CursorAction userAction);
    protected CursorActionCallback m_cursorActionCallback;

    //----------------------------------------------------------------------------------------
    public void SetCursorActionCallback(CursorActionCallback cursorActionCallback)
    {
        m_cursorActionCallback = cursorActionCallback;
    }

    //----------------------------------------------------------------------------------------
    public virtual float GetOffsetXFromPrevFrame() { return 0.0f; }
    public virtual float GetOffsetYFromPrevFrame() { return 0.0f; }
    public virtual void ResetState() {}
}
