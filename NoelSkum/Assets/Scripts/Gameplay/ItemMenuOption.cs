using UnityEngine;
using System.Collections;
using System;

public enum ItemMenuOptionType
{
    PickUp,
    Move,
    MoveRight,
    MoveUp,
    MoveForward,
    MoveLeft,
    MoveDown,
    MoveBack
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
        
    }

    public void Activate()
    {
        this.OnActivation();
    }
}
