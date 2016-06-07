using UnityEngine;
using System.Collections;

/// <summary>
/// Provide mouse device functionality
/// </summary
public class MouseController : InputController
{
    Vector2 m_lastMousePos;
    Vector2 m_mousePosDiff;
    Vector2 m_invScrSize;
    float   m_mouseSensitivity = 1.2f;

    //----------------------------------------------------------------------------------------
    void Start ()
    {
        ResetState();
    }

    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Update mause state each frame
    /// </summary
    void Update ()
    {
        Vector2 curPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        m_mousePosDiff = m_lastMousePos - curPos;
        m_mousePosDiff.x *= m_invScrSize.x;
        m_mousePosDiff.y *= m_invScrSize.y;

        m_lastMousePos = curPos;

        if(m_cursorActionCallback != null)
        {
            if (Input.GetMouseButtonDown(0))
                m_cursorActionCallback(CursorAction.CursorPressed);
        }
    }

    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Reset device state to default
    /// </summary
    public override void ResetState()
    {
        m_lastMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        m_mousePosDiff = new Vector2(0.0f, 0.0f);

        m_invScrSize = new Vector2(m_mouseSensitivity / (float)Screen.width, m_mouseSensitivity / (float)Screen.height);
    }

    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Difference of X screen position from previouse frame
    /// </summary
    public override float GetOffsetXFromPrevFrame()
    {
        return m_mousePosDiff.x;
    }

    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Difference of Y screen position from previouse frame
    /// </summary
    public override float GetOffsetYFromPrevFrame()
    {
        return m_mousePosDiff.y;
    }


}
