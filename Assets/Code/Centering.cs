using System;
using UnityEngine;

namespace Code
{
    public class Centering:MonoBehaviour
    {
        [SerializeField] private Transform _pivot;
        [SerializeField] private CapsuleCollider _collider;
        private Vector3 _newPosition;

        private void OnValidate()
        {
            _collider = GetComponent<CapsuleCollider>();
        }

        private void Start()
        {
            FindTeleportPivotAndTarget();
            _newPosition.y = _collider.center.y;
        }

        private void Update()
        {
            _newPosition.x = _pivot.localPosition.x;
            _newPosition.z = _pivot.localPosition.z;
            _collider.center = _newPosition;
        }

        private void FindTeleportPivotAndTarget()
        {
            foreach (var camera in Camera.allCameras)
            {
                if (!camera.enabled)
                    continue;
                if (camera.stereoTargetEye!=StereoTargetEyeMask.Both)
                    continue;
                _pivot = camera.transform;
            }
        }
    }
}