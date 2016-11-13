using UnityEngine;
using System.Collections;
using System;

public enum ItemMenuOptionType
{
    PickUp,
    Move
}

public class ItemMenuOption : MonoBehaviour {

    public ItemMenuOptionType OptionType;
    public Action OnActivation;

    public void SetUp(Action onActivation)
    {
        this.OnActivation = onActivation;
    }

    public void Unset()
    {
        Debug.Log("Unset this");
    }

    public void Activate()
    {
        this.OnActivation();
    }
}
