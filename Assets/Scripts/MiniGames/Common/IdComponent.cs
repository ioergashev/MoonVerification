using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Itorum
{
    public class IdComponent : MonoBehaviour
    {
        [Inject]
        private ManagerId managerId;

        public string Id;

        private void OnEnable()
        {
            if(!managerId.Entities.Contains(this))
            {
                managerId.Entities.Add(this);
            }
        }
    }
}