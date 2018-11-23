using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Windows.Kinect;
using Joint = Windows.Kinect.Joint;

public class KinectBodyController : MonoBehaviour
{
    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;

    public Transform[] skeleton = new Transform[25];
    private Skeleton skeletonOffset;


    private Dictionary<JointType, JointType> _BoneMap = new Dictionary<JointType, JointType>()
    {
        { JointType.FootLeft, JointType.AnkleLeft },
        { JointType.AnkleLeft, JointType.KneeLeft },
        { JointType.KneeLeft, JointType.HipLeft },
        { JointType.HipLeft, JointType.SpineBase },

        { JointType.FootRight, JointType.AnkleRight },
        { JointType.AnkleRight, JointType.KneeRight },
        { JointType.KneeRight, JointType.HipRight },
        { JointType.HipRight, JointType.SpineBase },

        { JointType.HandTipLeft, JointType.HandLeft },
        { JointType.ThumbLeft, JointType.HandLeft },
        { JointType.HandLeft, JointType.WristLeft },
        { JointType.WristLeft, JointType.ElbowLeft },
        { JointType.ElbowLeft, JointType.ShoulderLeft },
        { JointType.ShoulderLeft, JointType.SpineShoulder },

        { JointType.HandTipRight, JointType.HandRight },
        { JointType.ThumbRight, JointType.HandRight },
        { JointType.HandRight, JointType.WristRight },
        { JointType.WristRight, JointType.ElbowRight },
        { JointType.ElbowRight, JointType.ShoulderRight },
        { JointType.ShoulderRight, JointType.SpineShoulder },

        { JointType.SpineBase, JointType.SpineMid },
        { JointType.SpineMid, JointType.SpineShoulder },
        { JointType.SpineShoulder, JointType.Neck },
        { JointType.Neck, JointType.Head },
    };

    private void Start()
    {
        _BodyManager = this.GetComponent<BodySourceManager>();
        skeletonOffset = this.GetComponent<Skeleton>();
        //if (skeletonOffset.IsNotNull())
        //{
        //    skeletonOffset.SetSkeleton(skeleton, this.gameObject.name);
        //}
    }

    void Update()
    {
        Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            return;
        }
        
        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                trackedIds.Add(body.TrackingId);
            }
        }

        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

        // First delete untracked bodies
        foreach (ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }
        
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                if (!_Bodies.ContainsKey(body.TrackingId))
                {
                    MoveBody(body);
                }
            }
        }
    }

    private void MoveBody(Body body)
    {
        for (JointType jt = JointType.SpineBase; jt <= JointType.ThumbRight; jt++)
        {
            if (skeleton[(int)jt] == null) continue;
            else
            {
                SetSkeleton(body, jt, ref skeleton);
            }
        }
    }
    private static Vector3 GetVector3FromJoint(Joint joint)
    {
        return new Vector3(joint.Position.X, joint.Position.Y, joint.Position.Z);
    }

    private void SetSkeleton(Body body, JointType index, ref Transform[] skeleton)
    {
        switch (index)
        {
            case JointType.SpineBase:
                skeletonOffset.skeleton[0].position = GetVector3FromJoint(body.Joints[index]);
                //skeletonOffset.skeleton[0].LookAt(GetVector3FromJoint(body.Joints[index + 1]));
                break;
            case JointType.SpineMid:
                skeletonOffset.LookAt2(GetVector3FromJoint(body.Joints[index + 1]), 1);
                break;
            case JointType.Neck:
                skeletonOffset.LookAt2(GetVector3FromJoint(body.Joints[index + 1]), 2);
                break;
            case JointType.Head:
                //skeletonOffset.LookAt2(GetVector3FromJoint(body.Joints[index + 1]), 3);
                break;
            case JointType.ShoulderLeft:
                //skeletonOffset.skeleton[8].position = GetVector3FromJoint(body.Joints[index]);
                skeletonOffset.LookAt2(GetVector3FromJoint(body.Joints[index + 1]), 8);
                break;
            case JointType.ElbowLeft:
                skeletonOffset.LookAt2(GetVector3FromJoint(body.Joints[index + 1]), 9);
                break;
            case JointType.WristLeft:
                skeletonOffset.LookAt2(GetVector3FromJoint(body.Joints[index + 1]), 10);
                break;
            case JointType.HandLeft:
                //skeletonOffset.LookAt2(GetVector3FromJoint(body.Joints[index + 1]), 7);
                break;
            case JointType.ShoulderRight:
                //skeletonOffset.skeleton[4].position = GetVector3FromJoint(body.Joints[index]);
                skeletonOffset.LookAt2(GetVector3FromJoint(body.Joints[index + 1]), 4);
                break;
            case JointType.ElbowRight:
                skeletonOffset.LookAt2(GetVector3FromJoint(body.Joints[index + 1]), 5);
                break;
            case JointType.WristRight:
                skeletonOffset.LookAt2(GetVector3FromJoint(body.Joints[index + 1]), 6);
                break;
            case JointType.HandRight:
                //skeletonOffset.LookAt2(GetVector3FromJoint(body.Joints[index + 1]), 11);
                break;
            case JointType.HipLeft:
                //skeletonOffset.skeleton[16].position = GetVector3FromJoint(body.Joints[index]);
                skeletonOffset.LookAt2(GetVector3FromJoint(body.Joints[index + 1]), 16);
                break;
            case JointType.KneeLeft:
                skeletonOffset.LookAt2(GetVector3FromJoint(body.Joints[index + 1]), 17);
                break;
            case JointType.AnkleLeft:
                skeletonOffset.LookAt2(GetVector3FromJoint(body.Joints[index + 1]), 18);
                break;
            case JointType.FootLeft:
                //skeletonOffset.LookAt2(GetVector3FromJoint(body.Joints[index + 1]), 19);
                break;
            case JointType.HipRight:
                //skeletonOffset.skeleton[12].position = GetVector3FromJoint(body.Joints[index]);
                skeletonOffset.LookAt2(GetVector3FromJoint(body.Joints[index + 1]), 12);
                break;
            case JointType.KneeRight:
                skeletonOffset.LookAt2(GetVector3FromJoint(body.Joints[index + 1]), 13);
                break;
            case JointType.AnkleRight:
                skeletonOffset.LookAt2(GetVector3FromJoint(body.Joints[index + 1]), 14);
                break;
            case JointType.FootRight:
                //skeletonOffset.LookAt2(GetVector3FromJoint(body.Joints[index + 1]), 15);
                break;
            case JointType.SpineShoulder:
                //
                break;
            case JointType.HandTipLeft:
                //
                break;
            case JointType.ThumbLeft:
                //
                break;
            case JointType.HandTipRight:
                //
                break;
            case JointType.ThumbRight:
                //
                break;
        }
    }
}
