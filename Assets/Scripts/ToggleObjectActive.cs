using UdonSharp;
using UnityEngine;

public class ToggleObjectActive : UdonSharpBehaviour
{
    [SerializeField] private GameObject[] targetObjects;

    public void ToggleActive()
    {
        foreach (var obj in targetObjects)
        {
            if (obj == null) continue;
            obj.SetActive(!obj.activeSelf);
        }
    }
}