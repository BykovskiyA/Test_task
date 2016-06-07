using UnityEngine;
using System.Collections;

//----------------------------------------------------------------------------------------
/// <summary>
/// Game object propery
/// </summary
public class CubeProperty : MonoBehaviour
{
    public enum CubeState
    {
        Normal,
        Over,
        Pressed,
        BlinkBad,
        BlinkGood,
        Unknown
    };

    public TextMesh[]   m_textView;     // text on object faces
    public Color[]      m_colorState;   // color associated with the state

    CubeState           m_curCubeState;     // curent state
    float               m_fRotation = 0.0f; // current rotation
    Transform           m_objTM;            // transform cash

    int                 m_id;               // object id(for external use)
    float               m_fBlinkTime;       // time from blink start
    bool                m_bBlinked;         // blink state on/off

    //----------------------------------------------------------------------------------------
    void Start ()
    {
        m_objTM = this.GetComponent<Transform>();
	}

    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Init object base state
    /// </summary
    public void ResetState(int iID, string sText, float fMinRot, float fMaxRot)
    {
        m_curCubeState = CubeState.Unknown;
        m_fBlinkTime = 0.0f;
        m_bBlinked = false;
        m_id = iID;
        SetText(sText);
        SetState(CubeState.Normal);
        ApplyRotation(Random.Range(fMinRot, fMaxRot));
    }

    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Return object id
    /// </summary
    public int GetID()
    {
        return m_id;
    }

    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Udate object state
    /// </summary
    void Update ()
    {
        if(m_bBlinked)
        {
            m_fBlinkTime += Time.deltaTime*2.0f*Mathf.PI;
            float fKoef = (Mathf.Sin(m_fBlinkTime) + 1.5f)*(1.0f / 2.5f);
            Color objColor = m_colorState[(int)m_curCubeState] * fKoef;
            objColor.a = 1.0f;
            SetColor(objColor);
        }

        if (m_fRotation == 0.0f)
            return;

        m_objTM.Rotate(Vector3.up, Time.deltaTime * m_fRotation);
	}

    //----------------------------------------------------------------------------------------
    /// <summary>
    ///Set object rotation speed
    /// </summary
    void ApplyRotation(float fSpeed)
    {
        m_fRotation = fSpeed;
    }

    //----------------------------------------------------------------------------------------
    /// <summary>
    ///Set object new state
    /// </summary
    public void SetState(CubeState newState)
    {
        if (m_curCubeState == newState)
            return;

        m_bBlinked = false;

        if (newState == CubeState.BlinkBad || newState == CubeState.BlinkGood)
        {
            m_bBlinked = true;
            m_fBlinkTime = 0.0f;
        }

        m_curCubeState = newState;
        SetColor(m_colorState[(int)m_curCubeState]);
    }

    //----------------------------------------------------------------------------------------
    public CubeState GetState()
    {
        return m_curCubeState;
    }

    //----------------------------------------------------------------------------------------
    void SetColor(Color newColor)
    {
        this.GetComponent<Renderer>().material.color = newColor;
    }

    //----------------------------------------------------------------------------------------
    void SetText(string sNewText)
    {
        foreach(var textView in m_textView)
            textView.text = sNewText;
    }
}
