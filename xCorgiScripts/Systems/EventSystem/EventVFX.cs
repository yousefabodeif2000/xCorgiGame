using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dimensional.Events
{
    public class EventVFX : ScriptableObject
    {
        public string effectName;
        public GameObject vfXObject;

        public void SpawnVFX(Vector3 pos)
        {
            Instantiate(vfXObject, pos, Quaternion.identity);
        }
    }
}