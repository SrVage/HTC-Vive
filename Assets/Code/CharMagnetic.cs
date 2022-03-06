using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace Code
{
    public class CharMagnetic:MonoBehaviour
    {
        [SerializeField] private float _spellDistance = 20f;
        [SerializeField] private float _maxMagniteForce = 50;
        [SerializeField] private MagnitePoint _magniteSpell;
        [SerializeField] private Transform _blueHolder, _redHolder;
        [SerializeField] private Material _redMaterial, _blueMaterial, _yellowMaterial;
        [SerializeField] private ParticleSystem _highLightReference;

        public void SetBlue(Transform trans)
        {
            _magniteSpell.BlueObj = trans;
            _magniteSpell.BluePos = trans.position;
            Highlighting(true, trans);
            CheckToJoint();
        }
        
        public void SetRed(Transform trans)
        {
            _magniteSpell.RedObj = trans;
            _magniteSpell.RedPos = trans.position;
            Highlighting(true, trans);
            CheckToJoint();
        }
        public void SetBlue(Vector3 trans)
        {
            _magniteSpell.BlueObj = _blueHolder;
            _magniteSpell.BluePos = trans;
            _blueHolder.position = trans;
            _blueHolder.GetChild(0).gameObject.SetActive(true);
            CheckToJoint();
        }
        
        public void SetRed(Vector3 trans)
        {
            _magniteSpell.RedObj = _redHolder;
            _magniteSpell.RedPos = trans;
            _redHolder.position = trans;
            _redHolder.GetChild(0).gameObject.SetActive(true);
            CheckToJoint();
        }
        
        

        private void CheckToJoint()
        {
            if (_magniteSpell.BlueObj != null && _magniteSpell.RedObj != null)
            {
                if (Vector3.Distance(_magniteSpell.RedPos, _magniteSpell.BluePos) < _spellDistance)
                    CreateJoint();
                else EraseSpell();
            }
        }

        private void EraseSpell()
        {
            _magniteSpell.BlueObj = null;
            _magniteSpell.RedObj = null;
            for (int i = 0; i < _magniteSpell.HighLight.Count; i++)
            {
                _magniteSpell.HighLight[i].GetComponent<Renderer>().material = _yellowMaterial;
            }
        }

        private void CreateJoint()
        {
            SpringJoint springJoint = _magniteSpell.BlueObj.gameObject.AddComponent<SpringJoint>();
            springJoint.autoConfigureConnectedAnchor = false;
            springJoint.anchor = Vector3.zero;
            springJoint.connectedAnchor = Vector3.zero;
            springJoint.enableCollision = true;
            springJoint.enablePreprocessing = false;
            springJoint.connectedBody = _magniteSpell.RedObj.GetComponent<Rigidbody>();
            EraseSpell();
            _magniteSpell.JointList.Add(springJoint);
            Rigidbody rb = springJoint.GetComponent<Rigidbody>();
            _magniteSpell.Rigidbodies.Add(rb);
            AddRigidBody(springJoint.connectedBody);
        }

        private void AddRigidBody(Rigidbody rb)
        {
            if (_magniteSpell.Rigidbodies==null)
                return;
            for (int i = 0; i < _magniteSpell.Rigidbodies.Count; i++)
            {
                if (rb == _magniteSpell.Rigidbodies[i])
                    break;
                if (i == _magniteSpell.Rigidbodies.Count - 1)
                {
                    _magniteSpell.Rigidbodies.Add(rb);
                } 
            }
        }

        private void Highlighting(bool isBlue, Transform trans)
        {
            if (trans.GetComponentInChildren<ParticleSystem>())
            {
                Destroy(trans.GetComponentInChildren<ParticleSystem>());
            }
            ParticleSystem ps = Instantiate(_highLightReference, trans, false);
            if (isBlue)
                ps.GetComponent<Renderer>().material = _blueMaterial;
            else
                ps.GetComponent<Renderer>().material = _redMaterial;
            _magniteSpell.HighLight.Add(ps);
        }

        public void DestroyAllJoints()
        {
            for (int i = 0; i < _magniteSpell.JointList.Count; i++)
            {
                Destroy(_magniteSpell.JointList[i]);
            }

            for (int i = 0; i < _magniteSpell.Rigidbodies.Count; i++)
            {
                _magniteSpell.Rigidbodies[i].angularDrag = 0.05f;
                _magniteSpell.Rigidbodies[i].drag = 0;
                _magniteSpell.Rigidbodies[i].WakeUp();
            }
            _magniteSpell.JointList.Clear();
            _magniteSpell.Rigidbodies.Clear();
            EraseSpell();
            for (int i = 0; i < _magniteSpell.HighLight.Count; i++)
            {
                Destroy(_magniteSpell.HighLight[i]);
            }

            DisableHolders();
        }

        public void ChangeSpringPower(float force)
        {
            if (_magniteSpell.JointList.Count > 0)
            {
                for (int i = 0; i < _magniteSpell.JointList.Count; i++)
                {
                    _magniteSpell.JointList[i].spring += force;
                    _magniteSpell.JointList[i].damper += force;
                    _magniteSpell.JointList[i].spring =
                        Mathf.Clamp(_magniteSpell.JointList[i].spring, 0, _maxMagniteForce);
                    _magniteSpell.JointList[i].damper =
                        Mathf.Clamp(_magniteSpell.JointList[i].damper, 0, _maxMagniteForce);
                }

                for (int i = 0; i < _magniteSpell.Rigidbodies.Count; i++)
                {
                    _magniteSpell.Rigidbodies[i].WakeUp();
                    _magniteSpell.Rigidbodies[i].angularDrag += force;
                    _magniteSpell.Rigidbodies[i].drag += force;
                    _magniteSpell.Rigidbodies[i].angularDrag = Mathf.Clamp(_magniteSpell.Rigidbodies[i].angularDrag, 0,_maxMagniteForce);
                    _magniteSpell.Rigidbodies[i].drag += Mathf.Clamp(_magniteSpell.Rigidbodies[i].drag, 0, _maxMagniteForce);
                }
            }
        }

        private void DisableHolders()
        {
            _blueHolder.GetChild(0).gameObject.SetActive(false);
            _redHolder.GetChild(0).gameObject.SetActive(false);
        }
    }
    
    [Serializable]
    public struct MagnitePoint
    {
        public List<SpringJoint> JointList;
        public List<Rigidbody> Rigidbodies;
        public List<ParticleSystem> HighLight;
        public Transform BlueObj, RedObj;
        public Vector3 BluePos, RedPos;
    }
}