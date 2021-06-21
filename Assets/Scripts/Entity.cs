using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public Action OnExplode;
    public void Explode()
    {
        OnExplode?.Invoke();
        gameObject.SetActive(false);
    }
}