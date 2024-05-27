using UnityEngine;

namespace Cinemachine.Examples
{
    /// <summary>
    /// This is an add-on for Cinemachine virtual cameras containing the ThirdPersonFollow component.
    /// It modifies the camera distance as a function of vertical angle.
    /// </summary>
    [SaveDuringPlay]
    public class ThirdPersonFollowDistanceModifier : MonoBehaviour
    {
        [Tooltip("Camera angle that corresponds to the start of the distance graph")]
        public float MinAngle;

        [Tooltip("Camera angle that corresponds to the end of the distance graph")]
        public float MaxAngle;

        [Tooltip("Defines how the camera distance scales as a function of vertical camera angle.  "
            + "X axis of graph go from 0 to 1, Y axis is the multiplier that will be "
            + "applied to the base distance.")]
        public AnimationCurve DistanceScale;

        Cinemachine3rdPersonFollow TpsFollow;
        Transform FollowTarget;
        float BaseDistance;

        void Reset()
        {
            MinAngle = -90;
            MaxAngle = 90;
            DistanceScale = AnimationCurve.EaseInOut(0, 0.5f, 1, 2);
        }

        void OnEnable()
        {
            var vcam = GetComponentInChildren<CinemachineVirtualCamera>();
            if (vcam != null)
            {
                TpsFollow = vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
                FollowTarget = vcam.Follow;
            }

            // Store the base camera distance, for consistent scaling
            if (TpsFollow != null)
                BaseDistance = TpsFollow.CameraDistance;
        }

        void OnDisable()
        {
            // Restore the TPS base camera distance
            if (TpsFollow != null)
                TpsFollow.CameraDistance = BaseDistance;
        }

        void Update()
        {
            // Scale the TPS camera distance
            if (TpsFollow != null && FollowTarget != null)
            {
                var xRot = FollowTarget.rotation.eulerAngles.x;
                if (xRot > 180)
                    xRot -= 360;
                var t = (xRot - MinAngle) / (MaxAngle - MinAngle);
                TpsFollow.CameraDistance = BaseDistance * DistanceScale.Evaluate(t);
            }
        }
    }
}