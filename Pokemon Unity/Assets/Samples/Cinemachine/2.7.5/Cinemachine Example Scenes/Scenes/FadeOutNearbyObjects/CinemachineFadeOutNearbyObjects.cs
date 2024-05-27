using System;
using UnityEngine;

namespace Cinemachine.Examples
{
    /// <summary>
    /// An example add-on module for Cinemachine Virtual Camera for controlling
    /// the FadeOut shader included in our example package.
    /// </summary>
    [AddComponentMenu("")] // Hide in menu
    [ExecuteAlways]
    public class CinemachineFadeOutNearbyObjects : CinemachineExtension
    {
        /// <summary>
        /// Radius of the look at target.
        /// </summary>
        [Tooltip("Radius of the look at target.")]
        public float m_LookAtTargetRadius = 1;
    
        /// <summary>
        /// Minimum distance to have fading out effect in front of the camera.
        /// </summary>
        [Tooltip("Minimum distance to have fading out effect in front of the camera.")]
        public float m_MinDistance = 0;

        /// <summary>
        /// Maximum distance to have fading out effect in front of the camera.
        /// </summary>
        [Tooltip("Maximum distance to have fading out effect in front of the camera.")]
        public float m_MaxDistance = 8;

        /// <summary>
        /// If true, m_MaxDistance will be set to
        /// distance between this virtual camera and LookAt target minus m_LookAtTargetRadius.
        /// </summary>
        [Tooltip("If true, MaxDistance will be set to " +
            "distance between this virtual camera and LookAt target minus LookAtTargetRadius.")]
        public bool m_SetToCameraToLookAtDistance = false;
    
        /// <summary>
        /// Material using the FadeOut shader.
        /// </summary>
        [Tooltip("Material using the FadeOut shader.")]
        public Material m_FadeOutMaterial;

        static readonly int k_MaxDistanceID = Shader.PropertyToID("_MaxDistance");
        static readonly int k_MinDistanceID = Shader.PropertyToID("_MinDistance");

        /// <summary>
        /// Updates FadeOut shader on the specified FadeOutMaterial.
        /// </summary>
        /// <param name="vcam">The virtual camera being processed</param>
        /// <param name="stage">The current pipeline stage</param>
        /// <param name="state">The current virtual camera state</param>
        /// <param name="deltaTime">The current applicable deltaTime</param>
        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage == CinemachineCore.Stage.Finalize)
            {
                if (m_FadeOutMaterial == null || !m_FadeOutMaterial.HasProperty(k_MaxDistanceID) || 
                    !m_FadeOutMaterial.HasProperty(k_MinDistanceID)) return;
            
                if (m_SetToCameraToLookAtDistance && vcam.LookAt != null)
                {
                    m_MaxDistance = Vector3.Distance(vcam.transform.position, vcam.LookAt.position) - m_LookAtTargetRadius;
                }

                m_FadeOutMaterial.SetFloat(k_MaxDistanceID, m_MaxDistance);
                m_FadeOutMaterial.SetFloat(k_MinDistanceID, m_MinDistance);
            }
        }
        
        void OnValidate()
        {
            m_LookAtTargetRadius = Math.Max(0, m_LookAtTargetRadius);
            m_MinDistance = Math.Max(0, m_MinDistance);
            m_MaxDistance = Math.Max(0, m_MaxDistance);
        }
    }
}
