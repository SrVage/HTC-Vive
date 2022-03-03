using System.Collections;
using HTC.UnityPlugin.Vive;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code
{
    public class MyTeleport:Teleportable
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _coolDown;

        public override IEnumerator StartTeleport(RaycastResult hitResult, Vector3 position, Quaternion rotation, float delay)
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                target.position = Vector3.MoveTowards(target.position, position, _speed * Time.deltaTime);
                Vector3 v = position;
                v.y = target.position.y;
                if (Vector3.Distance(target.position, v) < 0.1f)
                {
                    yield return new WaitForSeconds(_coolDown);
                    teleportCoroutine = null;
                    yield break;
                }
            }
        }
    }
}