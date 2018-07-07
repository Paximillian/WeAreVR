using UnityEngine;

public struct GesturePose
{
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public float Time { get; set; }
}