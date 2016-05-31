using UnityEngine;
using System.Collections;

public class ExpresionGenerator
{
    enum MathOps
    {
        Plus,
        Minus,
        Multiply,
        Devide,
        MathOpNumber
    }

    int[]    m_expressions = new int[0];
    int      m_iCorrectAnswerID = -1;
    int      m_iCorrectAnswer;
    string   m_sQuestion;

    //----------------------------------------------------------------------------------------
    private ExpresionGenerator() { }
    //----------------------------------------------------------------------------------------
    public ExpresionGenerator(int iLevelID)
    {
        int iSceneAnswerNumber = 4 + iLevelID / 4;

        if (iSceneAnswerNumber > 8)
            iSceneAnswerNumber = 8;

        m_expressions = new int[iSceneAnswerNumber];
        m_iCorrectAnswerID = Random.Range(0, iSceneAnswerNumber);

        GenerateQuestion(iLevelID);

        int iMinPoint = m_iCorrectAnswer - (iLevelID + 4);
        int iMaxPoint = m_iCorrectAnswer + (iLevelID + 4);

        for (int iValCount = 0; iValCount < m_expressions.Length; iValCount++)
        {
            if(iValCount == m_iCorrectAnswerID)
            {
                m_expressions[iValCount] = m_iCorrectAnswer;
                continue;
            }

            int iAnswer = 0;

            do {
                iAnswer = Random.Range(iMinPoint, iMaxPoint);
            }
            while (iAnswer == m_iCorrectAnswer);


            m_expressions[iValCount] = iAnswer;
        }
    }

    //----------------------------------------------------------------------------------------
    void GenerateQuestion(int iLevelID)
    {
        string[] sMathOp = { "+", "-", "*", "/" };
        m_sQuestion = "Select the correct answer for expression: ";

        int iUsedMathOps = iLevelID / 5 + 1;

        if (iUsedMathOps > sMathOp.Length)
            iUsedMathOps = sMathOp.Length;

        m_iCorrectAnswer = Random.Range(3 + iLevelID*iLevelID, 10 + (iLevelID+1)*iLevelID);

        int iMathOpType = Random.Range(0, iUsedMathOps);
        int iLVal = 0;
        int iMinPoint = 1 + iLevelID;
        int iMaxPoint = m_iCorrectAnswer - 1 - iLevelID;

        if (iMathOpType >= 2)
            iMaxPoint = (m_iCorrectAnswer - 1) / 2;

        int iRVal = Random.Range(iMinPoint, iMaxPoint);

        switch (iMathOpType)
        {
            case (int)MathOps.Plus:
                iLVal = m_iCorrectAnswer - iRVal;
                break;

            case (int)MathOps.Minus:
                iLVal = m_iCorrectAnswer + iRVal;
                break;

            case (int)MathOps.Multiply:
                iLVal = m_iCorrectAnswer / iRVal;
                m_iCorrectAnswer += (m_iCorrectAnswer % iRVal);
                break;

            case (int)MathOps.Devide:
                iLVal = m_iCorrectAnswer * iRVal;
                break;
        }

        m_sQuestion += iLVal.ToString() + " " + sMathOp[iMathOpType] + " " + iRVal.ToString() + " = ?";
    }

    //----------------------------------------------------------------------------------------
    public int GetAnswerNumber()
    {
        return m_expressions.Length;
    }

    public string GetQuestion()
    {
        return m_sQuestion;
    }

    public int GetAnswer(int iID)
    {
        return m_expressions[iID];
    }

    public int GetCorrectAnswerID()
    {
        return m_iCorrectAnswerID;
    }
    
    //----------------------------------------------------------------------------------------

}
