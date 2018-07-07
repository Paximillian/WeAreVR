using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GestureHandler : MonoBehaviour
{
    private const float k_RecordRateInSeconds = 0.01f;
    protected float RecordRateInSeconds { get { return k_RecordRateInSeconds; } }

    private AnimationCurve CurrentXPosRecording;
    private AnimationCurve CurrentYPosRecording;
    private AnimationCurve CurrentZPosRecording;
    private AnimationCurve CurrentXRotRecording;
    private AnimationCurve CurrentYRotRecording;
    private AnimationCurve CurrentZRotRecording;
    private AnimationCurve CurrentWRotRecording;
    private Quaternion BaseRotation;
    private Vector3 BasePosition;

    protected Gesture RecordedGesture { get; private set; }

    private Coroutine m_RecordingCoroutine;

    [SerializeField]
    private string m_ButtonName;

    private void Update()
    {
        if (Input.GetButtonDown(m_ButtonName))
        {
            m_RecordingCoroutine = StartCoroutine(recordTracking());
        }
        else if (Input.GetButtonUp(m_ButtonName))
        {
            StopCoroutine(m_RecordingCoroutine);
            
            RecordedGesture = Gesture.CreateInstance<Gesture>();
            RecordedGesture.BasePosition = BasePosition;
            RecordedGesture.BaseRotation = BaseRotation;
            RecordedGesture.XPosCurve = CurrentXPosRecording;
            RecordedGesture.YPosCurve = CurrentYPosRecording;
            RecordedGesture.ZPosCurve = CurrentZPosRecording;
            RecordedGesture.XRotCurve = CurrentXRotRecording;
            RecordedGesture.YRotCurve = CurrentYRotRecording;
            RecordedGesture.ZRotCurve = CurrentZRotRecording;
            RecordedGesture.WRotCurve = CurrentWRotRecording;

            handleGesture();
        }
    }

    private IEnumerator recordTracking()
    {
        BasePosition = transform.position;
        BaseRotation = transform.rotation;

        CurrentXPosRecording = new AnimationCurve(new Keyframe(0, 0));
        CurrentYPosRecording = new AnimationCurve(new Keyframe(0, 0));
        CurrentZPosRecording = new AnimationCurve(new Keyframe(0, 0));
        CurrentXRotRecording = new AnimationCurve(new Keyframe(0, 0));
        CurrentYRotRecording = new AnimationCurve(new Keyframe(0, 0));
        CurrentZRotRecording = new AnimationCurve(new Keyframe(0, 0));
        CurrentWRotRecording = new AnimationCurve(new Keyframe(0, 0));

        while (true)
        {
            yield return new WaitForSeconds(k_RecordRateInSeconds);

            CurrentXPosRecording.AddKey(CurrentXPosRecording[CurrentXPosRecording.length - 1].time + k_RecordRateInSeconds, transform.position.x - BasePosition.x);
            CurrentYPosRecording.AddKey(CurrentYPosRecording[CurrentYPosRecording.length - 1].time + k_RecordRateInSeconds, transform.position.y - BasePosition.y);
            CurrentZPosRecording.AddKey(CurrentZPosRecording[CurrentZPosRecording.length - 1].time + k_RecordRateInSeconds, transform.position.z - BasePosition.z);
            CurrentXRotRecording.AddKey(CurrentXRotRecording[CurrentXRotRecording.length - 1].time + k_RecordRateInSeconds, transform.rotation.x - BaseRotation.x);
            CurrentYRotRecording.AddKey(CurrentYRotRecording[CurrentYRotRecording.length - 1].time + k_RecordRateInSeconds, transform.rotation.y - BaseRotation.y);
            CurrentZRotRecording.AddKey(CurrentZRotRecording[CurrentZRotRecording.length - 1].time + k_RecordRateInSeconds, transform.rotation.z - BaseRotation.z);
            CurrentWRotRecording.AddKey(CurrentWRotRecording[CurrentWRotRecording.length - 1].time + k_RecordRateInSeconds, transform.rotation.w - BaseRotation.w);
        }
    }

    protected abstract void handleGesture();
}
