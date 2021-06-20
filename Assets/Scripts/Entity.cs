using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public Action OnExplode;
    protected void Explode()
    {
        OnExplode?.Invoke();
        gameObject.SetActive(false);
    }
}