using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GestureRecognizer : GestureHandler
{
    [SerializeField]
    private TextMesh m_Text;

    [SerializeField]
    private Gesture m_Gesture;
    
    protected override void handleGesture()
    {
        if (RecordedGesture.IsSimilarTo(m_Gesture))
        {
            m_Text.text = "Correct";
        }
        else
        {
            m_Text.text = "Wrong";
        }
    }
}
