using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeartCounter : MonoBehaviour
{
    public List<GameObject> hearts;

    public static HeartCounter Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void IncreaseHealth()
    {
        if (hearts.Last().activeSelf) return;
        foreach (var heart in hearts.Where(heart => !heart.activeSelf))
        {
            heart.SetActive(true);
            break;
        }
    }

    public void DecreaseHealth()
    {
        for (var i = hearts.Count - 1; i >= 0; i--)
        {
            if (hearts[i].activeSelf)
            {
                hearts[i].SetActive(false);
                break;
            }
        }
    }

    public void ResetLife()
    {
        hearts[0].SetActive(true);
    }

    public bool IsEmptyLife()
    {
        return !hearts.First().activeSelf;
    }
}