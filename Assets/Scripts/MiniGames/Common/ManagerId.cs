using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Itorum
{
    public class ManagerId : MonoBehaviour
    {
        [HideInInspector]
        public List<IdComponent> Entities = new List<IdComponent>();

        private void Awake()
        {
            Entities = FindObjectsOfType<IdComponent>().ToList();
        }

        public T GetComponentInObject<T>(string objectId) where T : Component
        {
            var entity = Entities.Find(i => i.Id == objectId);

            // Если сущность отсутствует в памяти
            if (entity == null)
            {
                entity = FindObjectsOfType<IdComponent>().ToList().Find(i => i.Id == objectId);  
            }

            T component = null;

            if (entity != null)
            {
                component = entity.GetComponent<T>();
            }

            return component;
        }
    }
}