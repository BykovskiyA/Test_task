using UnityEngine;
using System.Collections.Generic;

public class CaptureHitManager
{
    List<GameObject> m_captureFactory = new List<GameObject>();

    public delegate void ActionCallback(int iID);
    public delegate void ActionHoverCallback(int iID, bool bIn);
    protected ActionCallback m_actionCallback;
    protected ActionHoverCallback m_actionHoverCallback;

    int m_iLastCapturedID = -1;

    //----------------------------------------------------------------------------------------
    public void RegisterActionCallbacks(ActionCallback actionCallback, ActionHoverCallback actionHoverCallback)
    {
        m_actionCallback = actionCallback;
        m_actionHoverCallback = actionHoverCallback;
    }

    //----------------------------------------------------------------------------------------
    public void RegisterObject(GameObject go, int iID)
    {
        CaptureProperty objCaptureProperty = go.GetComponent<CaptureProperty>();
        objCaptureProperty.SetObjectID(iID);
        objCaptureProperty.SetActive(true);
        objCaptureProperty.RegisterCallback(OjectActionCallback);
        m_captureFactory.Add(go);
    }

    //----------------------------------------------------------------------------------------
    public void UnregisterObjects()
    {
        m_iLastCapturedID = -1;
        ActivateObjects(false);
        m_captureFactory.Clear();
    }

    //----------------------------------------------------------------------------------------
    public void UpdateCapturedObjects(int iCurCapturedID)
    {
        if (m_iLastCapturedID == iCurCapturedID)
            return;

        if (m_iLastCapturedID >= 0)
        {
            m_captureFactory[m_iLastCapturedID].GetComponent<CaptureProperty>().ObjectWasMissed();
           m_actionHoverCallback(m_iLastCapturedID, false);
        }

        if (iCurCapturedID >= 0)
        {
            m_captureFactory[iCurCapturedID].GetComponent<CaptureProperty>().ObjectWasCaptured();
            m_actionHoverCallback(iCurCapturedID, true);
        }

        m_iLastCapturedID = iCurCapturedID;
    }

    //----------------------------------------------------------------------------------------
    public void ActivateObjects(bool bActive)
    {
        foreach(var obj in m_captureFactory)
        {
            obj.GetComponent<CaptureProperty>().SetActive(bActive);
        }
    }

    //----------------------------------------------------------------------------------------
    void OjectActionCallback(int iID)
    {
        if (m_actionCallback != null)
            m_actionCallback(iID);
    }

    //----------------------------------------------------------------------------------------
    void Update ()
    {
	
	}
}
