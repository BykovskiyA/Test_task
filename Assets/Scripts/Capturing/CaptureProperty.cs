using UnityEngine;

public class CaptureProperty : MonoBehaviour
{
    int             m_iCaptureID = -1;
    float           m_fCaptureTime = 0.0f;
    public float    m_fCaptureLen = 1.0f; // by default 1sec. for action run
    bool            m_bCaptured = false;
    bool            m_bActive = true;

    public delegate void ActionCallback(int iID);
    protected ActionCallback m_ActionCallback;

    //----------------------------------------------------------------------------------------
    public void SetActive(bool isActive)
    {
        m_bActive = isActive;
    }
    //----------------------------------------------------------------------------------------
    public void SetObjectID(int iID)
    {
        m_iCaptureID = iID;
    }

    //----------------------------------------------------------------------------------------
    public void RegisterCallback(ActionCallback actionReciver)
    {
        m_ActionCallback = actionReciver;
    }

    //----------------------------------------------------------------------------------------
    public void ObjectWasCaptured()
    {
        if (m_bCaptured)
            return;

        m_bCaptured = true;
        m_fCaptureTime = 0.0f;
    }

    //----------------------------------------------------------------------------------------
    public void ObjectWasMissed()
    {
        m_bCaptured = false;
    }
    
    //----------------------------------------------------------------------------------------
    void Update ()
    {
        if (!m_bCaptured || !m_bActive)
            return;

        m_fCaptureTime += Time.deltaTime;

        if (m_fCaptureTime >= m_fCaptureLen)
        {
            if(m_ActionCallback != null)
                m_ActionCallback(m_iCaptureID);
            m_bCaptured = false;
        }

    }
}
