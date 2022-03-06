using System;
using HTC.UnityPlugin.Vive;
using HTC.UnityPlugin.Vive.SteamVRExtension;
using HTC.UnityPlugin.VRModuleManagement;
using UnityEngine;

namespace Code
{
    public class TrackPadScroller:MonoBehaviour
    {
        [SerializeField] private float _speed = 10, _deadzone = 0.1f;
        private VIUSteamVRRenderModel _vive;
        private CharMagnetic _charMagnetic;

        private void Start()
        {
            _charMagnetic = GetComponent<CharMagnetic>();
        }

        private void Update()
        {
            if (_vive == null)
                _vive = GetComponentInChildren<VIUSteamVRRenderModel>();
            float dp = ViveInput.GetPadTouchDelta(HandRole.RightHand).y;
            if (Mathf.Abs(dp) > _deadzone)
            {
                _charMagnetic.ChangeSpringPower(dp*_speed); 
            }
        }
    }
}