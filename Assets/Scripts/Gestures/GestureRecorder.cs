#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GestureRecorder : GestureHandler
{
    [SerializeField]
    private Gesture m_TargetGesture;

    [SerializeField]
    private AudioSource m_RecordDoneSound;

    protected override void handleGesture()
    {
        if (m_TargetGesture == null)
        {
            string defaultPath = EditorPrefs.GetString("GestureSavePath", Application.dataPath);
            string targetPath = EditorUtility.SaveFilePanelInProject("Save Gesture Data", "NewGesture", "asset", "Choose a path to save your gesture", defaultPath);

            if (!String.IsNullOrWhiteSpace(targetPath))
            {
                EditorPrefs.SetString("GestureSavePath", targetPath);

                AssetDatabase.CreateAsset(RecordedGesture, targetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
        else
        {
            m_TargetGesture.XPosCurve = RecordedGesture.XPosCurve;
            m_TargetGesture.YPosCurve = RecordedGesture.YPosCurve;
            m_TargetGesture.ZPosCurve = RecordedGesture.ZPosCurve;
            m_TargetGesture.XRotCurve = RecordedGesture.XRotCurve;
            m_TargetGesture.YRotCurve = RecordedGesture.YRotCurve;
            m_TargetGesture.ZRotCurve = RecordedGesture.ZRotCurve;
            m_TargetGesture.WRotCurve = RecordedGesture.WRotCurve;
            m_TargetGesture.BasePosition = RecordedGesture.BasePosition;
            m_TargetGesture.BaseRotation = RecordedGesture.BaseRotation;

            m_RecordDoneSound.Play();
        }
    }
}
#endif