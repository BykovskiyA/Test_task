using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CursorManager : MonoBehaviour
{
    InputController m_inputController = null;
    Vector2         m_cursorPos; // (0.0; 0.0) - screen center
    public RawImage m_cursorTex;
    Vector2         m_screenSize;

    //----------------------------------------------------------------------------------------
    void Start ()
    {
        // all standalong platform use mouse, all mobile devices use touch screen
        // InputController class provide common interface
        #if UNITY_EDITOR || UNITY_STANDALONE
            gameObject.AddComponent<MouseController>();
            m_inputController = gameObject.GetComponent<MouseController>() as InputController;
        #else
            gameObject.AddComponent<TouchController>();
            m_inputController = gameObject.GetComponent<TouchController>() as InputController;
        #endif

        ResetState();
    }

    //----------------------------------------------------------------------------------------
    public Vector2 GetCursorPos()
    {
        return m_cursorPos;
    }

    //----------------------------------------------------------------------------------------
    public void ResetState()
    {
        m_cursorPos.x = 0.0f;
        m_cursorPos.y = 0.0f;
        m_cursorTex.rectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
        m_screenSize = new Vector2(Screen.width, Screen.height);

        m_inputController.ResetState();
    }

    //----------------------------------------------------------------------------------------
    public void SetCursorActionCallback(InputController.CursorActionCallback cursorActionCallback)
    {
        m_inputController.SetCursorActionCallback(cursorActionCallback);
    }

    //----------------------------------------------------------------------------------------
    void UpdateCursorScreenView()
    {
        m_cursorTex.rectTransform.anchoredPosition = new Vector2(m_cursorPos.x* m_screenSize.x, m_cursorPos.y* m_screenSize.y);
    }

    //----------------------------------------------------------------------------------------
    void Update ()
    {
        float controllerOffsetX = m_inputController.GetOffsetXFromPrevFrame();
        float controllerOffsetY = m_inputController.GetOffsetYFromPrevFrame();

        if (controllerOffsetX == 0.0f && controllerOffsetY == 0.0f)
            return;

        m_cursorPos.x -= controllerOffsetX;
        if (m_cursorPos.x < -0.5f)
           m_cursorPos.x = -0.5f;

        if (m_cursorPos.x > 0.5f)
            m_cursorPos.x = 0.5f;

        m_cursorPos.y -= controllerOffsetY;
        if (m_cursorPos.y < -0.5f)
            m_cursorPos.y = -0.5f;

        if (m_cursorPos.y > 0.5f)
            m_cursorPos.y = 0.5f;

        UpdateCursorScreenView();
    }
}
