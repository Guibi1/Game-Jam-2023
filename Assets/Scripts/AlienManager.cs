using System.Collections.Generic;
using UnityEngine;

public class AlienManager : MonoBehaviour
{
    public static AlienManager instance;

    public List<Alien> aliens = new List<Alien>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}