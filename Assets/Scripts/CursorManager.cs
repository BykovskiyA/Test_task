using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Abstarct cursor class. It support mouse and can be extended with other input devices
/// </summary
public class CursorManager : MonoBehaviour
{
    InputController m_inputController = null;
    Vector2         m_cursorPos; // (0.0; 0.0) - screen center
    Vector2 m_screenSize;

    public RawImage m_cursorTex;

    #if USE_STEREO
        public RawImage m_cursorTex2;
    #endif

    //----------------------------------------------------------------------------------------
    void Start ()
    {
        // all standalong platform use mouse, all mobile devices use touch screen
        // InputController class provide common interface
        #if USE_STEREO
            //gameObject.AddComponent<TouchController>();
            //m_inputController = gameObject.GetComponent<TouchController>() as InputController;
            //m_inputController = gameObject.GetComponent<GiroController>() as InputController;
            gameObject.AddComponent<MouseController>();
            m_inputController = gameObject.GetComponent<MouseController>() as InputController;
            m_cursorTex2.gameObject.SetActive(true);
        #else
            gameObject.AddComponent<MouseController>();
            m_inputController = gameObject.GetComponent<MouseController>() as InputController;
        #endif

        ResetState();
    }

    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Return cursor virtual position
    /// </summary
    public Vector2 GetCursorPos()
    {
        return m_cursorPos;
    }

    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Reset cursor state to default state
    /// </summary
    public void ResetState()
    {
        m_cursorPos.x = 0.0f;
        m_cursorPos.y = 0.0f;
        m_cursorTex.rectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);

        #if USE_STEREO
            m_cursorTex2.rectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
            m_screenSize = new Vector2(Screen.width*0.5f, Screen.height);
        #else
            m_screenSize = new Vector2(Screen.width, Screen.height);
        #endif

        m_inputController.ResetState();
    }

    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Call hepen when abstract input device send notification (if registered callback action)
    /// </summary
    public void SetCursorActionCallback(InputController.CursorActionCallback cursorActionCallback)
    {
        m_inputController.SetCursorActionCallback(cursorActionCallback);
    }

    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Update cursor view
    /// </summary
    void UpdateCursorScreenView()
    {
        #if USE_STEREO
            m_cursorTex.rectTransform.anchoredPosition = new Vector2((m_cursorPos.x - 0.5f) * m_screenSize.x, m_cursorPos.y * m_screenSize.y);
            m_cursorTex2.rectTransform.anchoredPosition = new Vector2((m_cursorPos.x + 0.5f) * m_screenSize.x, m_cursorPos.y * m_screenSize.y);
        #else
            m_cursorTex.rectTransform.anchoredPosition = new Vector2(m_cursorPos.x* m_screenSize.x, m_cursorPos.y* m_screenSize.y);
        #endif
    }

    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Update cursor state each frame
    /// </summary
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
