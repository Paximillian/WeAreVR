﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureDemonstrator : MonoBehaviour
{
    [SerializeField]
    private Gesture m_Gesture;

    private void Awake()
    {
        StartCoroutine(playbackGesture());
    }

    private IEnumerator playbackGesture()
    {
        for (int i = 0; i < m_Gesture.XPosCurve.length; ++i)
        {
            transform.position = new Vector3(m_Gesture.XPosCurve[i].value, m_Gesture.YPosCurve[i].value, m_Gesture.ZPosCurve[i].value);
            transform.rotation = new Quaternion(m_Gesture.XRotCurve[i].value + m_Gesture.BaseRotation.x,
                                                m_Gesture.YRotCurve[i].value + m_Gesture.BaseRotation.y,
                                                m_Gesture.ZRotCurve[i].value + m_Gesture.BaseRotation.z,
                                                m_Gesture.WRotCurve[i].value + m_Gesture.BaseRotation.w);

            if (m_Gesture.XPosCurve.length > i + 1)
            {
                yield return new WaitForSeconds(m_Gesture.XPosCurve[i + 1].time - m_Gesture.XPosCurve[i].time);
            }
        }

        StartCoroutine(playbackGesture());
    }
}