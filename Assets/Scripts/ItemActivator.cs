using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemActivator : MonoBehaviour
{
    [SerializeField] List<GameObject> items;

    public bool IsActivated => isActivated;
    public Action<bool> onActivated;

    bool isActivated = false;

    public void Activate(bool activate)
    {
        isActivated = activate;
        foreach (GameObject item in items)
        {
            item.SetActive(activate);
        }
        onActivated?.Invoke(activate);
    }
}
