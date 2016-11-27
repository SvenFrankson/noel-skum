using UnityEngine;
using System.Collections;
using System;

public enum ObjectMenuOptionType
{
    PickUp,
    Container,
    Move,
    MoveRight,
    MoveUp,
    MoveForward,
    MoveLeft,
    MoveDown,
    MoveBack,
    RotatePlus,
    RotateMinus
}

public class ObjectMenuOption : MonoBehaviour {

    public ObjectMenuOptionType OptionType;
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
