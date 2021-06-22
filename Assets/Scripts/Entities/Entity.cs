using System;
using UnityEngine;

namespace Entities
{
    public abstract class Entity : MonoBehaviour
    {
        public Action OnExplode;
        public void Explode()
        {
            OnExplode?.Invoke();
            gameObject.SetActive(false);
        }
    }
}