using System;
using HTC.UnityPlugin.Pointer3D;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code
{
    public class MagniteRaycaster:MonoBehaviour
    {
        public enum TypeOfMagnite
        {
            Blue=0,
            Red=1
        }

        [SerializeField] private TypeOfMagnite _colorOfMagnite;
        [SerializeField] private CharMagnetic _refToChar;
        private RaycastResult _curObj;
        private Pointer3DRaycaster _raycaster;

        private void OnValidate()
        {
            _raycaster = GetComponent<Pointer3DRaycaster>();
        }

        private void LateUpdate()
        {
            Raycasting();
        }

        private void Raycasting()
        {
            _curObj = _raycaster.FirstRaycastResult();
        }

        public void StartMagnite()
        {
            if (_curObj.isValid)
            {
                Rigidbody rb = _curObj.gameObject.GetComponent<Rigidbody>();
                switch (_colorOfMagnite)
                {
                    case TypeOfMagnite.Blue:
                        if (rb!=null) 
                            _refToChar.SetBlue(_curObj.gameObject.transform);
                        else
                            _refToChar.SetBlue(_curObj.worldPosition);
                        break;
                    case TypeOfMagnite.Red:
                        if (rb!=null) 
                            _refToChar.SetRed(_curObj.gameObject.transform);
                        else 
                            _refToChar.SetRed(_curObj.worldPosition);
                        break;
                }
            }
            else
                return;
        }
    }
}