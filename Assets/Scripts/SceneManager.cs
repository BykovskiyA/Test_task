using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Scane manager. Provade base functionality with application posible state. Can be extended with callback actuions for flexibility.
/// </summary
public class SceneManager : MonoBehaviour
{
    /// <summary>
    /// Application base states. Any state change can be provided with virtual callback.
    /// </summary
    enum SceneState
    {
        Unknown,
        BeginGame,
        StartLevel,
        StartNextLevel,
        RestartLevel,
        LooseGame,
        WinGame,
        SceneDelay,
        LevelRun,
        ScaneStateNumer
    }

    SceneState m_prevGameState; // not used, but for additional purpose in change states
    SceneState m_curGameState;  // current application state
    SceneState m_newGameState;  // next application state

    public Transform    m_sceneRoot;        // parent for all dynamic 3d objects
    public Text         m_quastionField;    // text view for user quastion, can be removed in childs for better virtualizaion

    public GameObject   m_answerObjPrefab;  // prefab for dynamicly created answer object
    List<GameObject>    m_answerSceneObjects = new List<GameObject>(); // interactive answer objects

    int m_levelID; // level number/difficulty

    public float m_fAnswerObjectsRadius = 3.0f; // object rotation radius
    int m_iCorrectAnswerID = -1;                // id of correct answer

    CursorManager       m_cursorManager;        // cursor manager
    CaptureHitManager   m_captureManager = new CaptureHitManager(); // object action 

    public Camera   m_baseCamera;       // camera used in usual visuality mode
    public Camera[] m_stereoCamera;     // cameras used in stereo mode

    //----------------------------------------------------------------------------------------
    void Start ()
    {
        #if USE_STEREO
            m_baseCamera.enabled = false;
            foreach(var camIter in m_stereoCamera)
            camIter.enabled = true;
        #else
            m_baseCamera.enabled = true;
            foreach (var camIter in m_stereoCamera)
            camIter.enabled = false;
        #endif

        m_levelID = 0;
        m_prevGameState = SceneState.Unknown;
        m_curGameState = SceneState.Unknown;
    }

    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Init scene state, register callbacks
    /// </summary
    void InitScene()
    {
        m_cursorManager = GetComponent<CursorManager>();
        m_cursorManager.ResetState();
        //m_cursorManager.SetCursorActionCallback(); // if need action from mouse button - register this callback
        m_captureManager.RegisterActionCallbacks(PressedActionCallback, HoverActionCallback);
    }

    //----------------------------------------------------------------------------------------
    void SetState(SceneState newState)
    {
        m_newGameState = newState;
    }

    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Update state of scene state machine
    /// </summary
    void Update ()
    {
        if (m_newGameState == m_curGameState)
        {
            if(m_curGameState == SceneState.LevelRun)
            {
                UpdateSceneAnimation();
                UpdateCapture();
            }

            if (m_curGameState == SceneState.Unknown)
            {
                InitScene();
                SetState(SceneState.BeginGame);
            }

            return;
        }

        if (m_newGameState == SceneState.BeginGame)
        {
            InitScene();
            UpdateLevelState(SceneState.StartLevel);
            return;
        }

        if (m_newGameState == SceneState.StartLevel)
        {
            m_prevGameState = m_curGameState;
            ClearSceneData();
            FillSceneWithObjects();
            m_curGameState = SceneState.LevelRun;
            m_newGameState = SceneState.LevelRun;
            return;
        }

        if (m_newGameState == SceneState.LooseGame)
        {
            m_levelID = 0;
            UpdateLevelState(SceneState.StartLevel);
        }

        if (m_newGameState == SceneState.WinGame)
        {
            ++m_levelID;
            UpdateLevelState(SceneState.StartLevel);
        }
    }

    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Set scene in new state
    /// </summary
    void UpdateLevelState(SceneState newState)
    {
        m_prevGameState = m_curGameState;
        m_curGameState = m_newGameState;
        m_newGameState = newState;
    }

    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Update cursor with scene objets interaction
    /// </summary
    void UpdateCapture()
    {
        int iHitedID = -1;
        RaycastHit hitResult = new RaycastHit();
        Vector2 vieportPoint = m_cursorManager.GetCursorPos();

        #if USE_STEREO
            Ray hitRay = m_stereoCamera[0].ViewportPointToRay(new Vector3(vieportPoint.x + 0.5f, vieportPoint.y + 0.5f, 0.0f));
        #else
            Ray hitRay = m_baseCamera.ViewportPointToRay(new Vector3(vieportPoint.x + 0.5f, vieportPoint.y + 0.5f, 0.0f));
        #endif

        if (Physics.Raycast(hitRay, out hitResult))
        {
            CubeProperty cubeProperty = hitResult.transform.gameObject.GetComponent<CubeProperty>();

            if(cubeProperty != null)
                iHitedID = cubeProperty.GetID();
        }

        m_captureManager.UpdateCapturedObjects(iHitedID);
    }

    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Callback run when the object is pressed
    /// </summary
    void PressedActionCallback(int iID)
    {
        m_newGameState = SceneState.SceneDelay;
        m_answerSceneObjects[iID].GetComponent<CubeProperty>().SetState(CubeProperty.CubeState.Pressed);
        StartCoroutine(DelayedChangeState(iID));
    }

    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Coroutin for merge animation and few scene states in same time
    /// </summary
    IEnumerator DelayedChangeState(int iID)
    {
        yield return new WaitForSeconds(2);
        m_answerSceneObjects[iID].GetComponent<CubeProperty>().SetState(iID == m_iCorrectAnswerID ? CubeProperty.CubeState.BlinkGood : CubeProperty.CubeState.BlinkBad);
        yield return new WaitForSeconds(3);
        m_newGameState = iID == m_iCorrectAnswerID ? SceneState.WinGame : SceneState.LooseGame;
        
    }
    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Callback run when cursor on the object hover in/out(object selection)
    /// </summary
    void HoverActionCallback(int iID, bool isIn)
    {
        m_answerSceneObjects[iID].GetComponent<CubeProperty>().SetState(isIn ? CubeProperty.CubeState.Over : CubeProperty.CubeState.Normal);
    }

    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Scene animation update
    /// </summary
    void UpdateSceneAnimation()
    {
        m_sceneRoot.Rotate(Vector3.up, Time.deltaTime*(4.0f + m_levelID));
    }

    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Clearing scene data
    /// </summary
    void ClearSceneData()
    {
        m_captureManager.UnregisterObjects();

        foreach (var answerObj in m_answerSceneObjects)
            GameObject.DestroyImmediate(answerObj);

        m_answerSceneObjects.Clear();
        m_sceneRoot.rotation = Quaternion.identity;
    }

    //----------------------------------------------------------------------------------------
    /// <summary>
    /// Create scene objects
    /// </summary
    void FillSceneWithObjects()
    {
        ExpresionGenerator opsGenerator = new ExpresionGenerator(m_levelID);

        int iSceneAnswerNumber = opsGenerator.GetAnswerNumber();
        m_iCorrectAnswerID = opsGenerator.GetCorrectAnswerID();

        float fSectorAngle = 2.0f*Mathf.PI / (float)iSceneAnswerNumber;

        m_quastionField.text = opsGenerator.GetQuestion();


        for (int iNewObjCount = 0; iNewObjCount < iSceneAnswerNumber; iNewObjCount++)
        {
            GameObject newAnswer = GameObject.Instantiate(m_answerObjPrefab) as GameObject;

            CubeProperty prperty = newAnswer.GetComponent<CubeProperty>();
            prperty.ResetState(iNewObjCount, opsGenerator.GetAnswer(iNewObjCount).ToString(), 10.0f + m_levelID*2.0f, 15.0f + m_levelID*2.0f);

            newAnswer.transform.SetParent(m_sceneRoot);
            newAnswer.transform.position = new Vector3(m_fAnswerObjectsRadius * Mathf.Sin(fSectorAngle * (float)iNewObjCount),
                0.0f, m_fAnswerObjectsRadius * Mathf.Cos(fSectorAngle * (float)iNewObjCount));

            m_captureManager.RegisterObject(newAnswer, iNewObjCount);
            m_answerSceneObjects.Add(newAnswer);
        }
    }
}
