using UnityEngine;
using UnityEngine.Events;

namespace Entities
{
    public abstract class Entity : MonoBehaviour
    {
        public UnityAction OnExplode;
        // ReSharper disable Unity.PerformanceAnalysis
        public void Explode()
        {
            OnExplode?.Invoke();
            gameObject.SetActive(false);
        }
    }
}