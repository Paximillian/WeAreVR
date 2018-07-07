using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Gesture : ScriptableObject, IEnumerable<GesturePose>, IEnumerator<GesturePose>
{
    private const float k_AcceptablePosDeltaThreshold = 0.1f;
    private const float k_AcceptableRotDeltaThreshold = 10f;

    [SerializeField]
    [ReadOnly]
    private AnimationCurve m_XPosCurve;
    public AnimationCurve XPosCurve { get { return m_XPosCurve; } set { m_XPosCurve = value; } }

    [SerializeField]
    [ReadOnly]
    private AnimationCurve m_YPosCurve;
    public AnimationCurve YPosCurve { get { return m_YPosCurve; } set { m_YPosCurve = value; } }

    [SerializeField]
    [ReadOnly]
    private AnimationCurve m_ZPosCurve;
    public AnimationCurve ZPosCurve { get { return m_ZPosCurve; } set { m_ZPosCurve = value; } }

    [SerializeField]
    [ReadOnly]
    private AnimationCurve m_XRotCurve;
    public AnimationCurve XRotCurve { get { return m_XRotCurve; } set { m_XRotCurve = value; } }

    [SerializeField]
    [ReadOnly]
    private AnimationCurve m_YRotCurve;
    public AnimationCurve YRotCurve { get { return m_YRotCurve; } set { m_YRotCurve = value; } }

    [SerializeField]
    [ReadOnly]
    private AnimationCurve m_ZRotCurve;
    public AnimationCurve ZRotCurve { get { return m_ZRotCurve; } set { m_ZRotCurve = value; } }

    [SerializeField]
    [ReadOnly]
    private AnimationCurve m_WRotCurve;
    public AnimationCurve WRotCurve { get { return m_WRotCurve; } set { m_WRotCurve = value; } }

    public Quaternion BaseRotation { get; set; }
    public Vector3 BasePosition { get; set; }

    public int Length { get { return m_XRotCurve?.length ?? 0; } }

    public float Duration { get { return Length == 0 ? 0 : m_XRotCurve[Length - 1].value; } }
    
    /// <summary>
    /// Compares this gesture to the given one and returns a value between 0-1 indicating the match rate of the 2.
    /// </summary>
    public bool IsSimilarTo(Gesture i_OtherGesture)
    {
        Reset();
        List<GesturePose> poses = this.ToList();
        i_OtherGesture.Reset();
        List<GesturePose> otherPoses = i_OtherGesture.ToList();

        double xPosDist = Algorithms.FrechetDistance(poses.Select(pose => new double[2] { pose.Time, pose.Position.x }).ToList(),
                                                     otherPoses.Select(pose => new double[2] { pose.Time, pose.Position.x }).ToList());

        double yPosDist = Algorithms.FrechetDistance(poses.Select(pose => new double[2] { pose.Time, pose.Position.y }).ToList(),
                                                     otherPoses.Select(pose => new double[2] { pose.Time, pose.Position.y }).ToList());

        double zPosDist = Algorithms.FrechetDistance(poses.Select(pose => new double[2] { pose.Time, pose.Position.z }).ToList(),
                                                     otherPoses.Select(pose => new double[2] { pose.Time, pose.Position.z }).ToList());

        double xRotDist = Algorithms.FrechetDistance(poses.Select(pose => new double[2] { pose.Time, pose.Rotation.x }).ToList(),
                                                     otherPoses.Select(pose => new double[2] { pose.Time, pose.Rotation.x }).ToList());

        double yRotDist = Algorithms.FrechetDistance(poses.Select(pose => new double[2] { pose.Time, pose.Rotation.y }).ToList(),
                                                     otherPoses.Select(pose => new double[2] { pose.Time, pose.Rotation.y }).ToList());

        double zRotDist = Algorithms.FrechetDistance(poses.Select(pose => new double[2] { pose.Time, pose.Rotation.z }).ToList(),
                                                     otherPoses.Select(pose => new double[2] { pose.Time, pose.Rotation.z }).ToList());

        double wRotDist = Algorithms.FrechetDistance(poses.Select(pose => new double[2] { pose.Time, pose.Rotation.w }).ToList(),
                                                     otherPoses.Select(pose => new double[2] { pose.Time, pose.Rotation.w }).ToList());

        return xPosDist < 0.75f &&
               yPosDist < 0.75f &&
               zPosDist < 0.75f &&
               xRotDist < 0.75f &&
               yRotDist < 0.75f &&
               zRotDist < 0.75f &&
               wRotDist < 0.75f;
    }

    public GesturePose this[int i]
    {
        get
        {
            return new GesturePose()
            {
                Position = new Vector3(m_XPosCurve.keys[i].value, m_YPosCurve.keys[i].value, m_ZPosCurve.keys[i].value),
                Rotation = new Quaternion(m_XRotCurve.keys[i].value, m_YRotCurve.keys[i].value, m_ZRotCurve.keys[i].value, m_WRotCurve.keys[i].value),
                Time = m_XPosCurve.keys[i].time
            };
        }
    }

    #region IEnumerable
    private int m_EnumerationIndex = -1;

    public IEnumerator GetEnumerator()
    {
        return this;
    }

    public bool MoveNext()
    {
        m_EnumerationIndex++;
        return (m_EnumerationIndex < Length);
    }

    public object Current
    {
        get
        {
            return this[m_EnumerationIndex];
        }
    }

    GesturePose IEnumerator<GesturePose>.Current
    {
        get
        {
            return this[m_EnumerationIndex];
        }
    }

    public void Dispose()
    {
    }

    IEnumerator<GesturePose> IEnumerable<GesturePose>.GetEnumerator()
    {
        return this;
    }

    public void Reset()
    {
        m_EnumerationIndex = -1;
    }
    #endregion IEnumerable
}
