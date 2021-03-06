﻿using UnityEngine;
using System.Collections;

public class ObjectMenuMain : ObjectMenu
{
    public static ObjectMenuMain instance;
    public static ObjectMenuMain Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ObjectMenuMain>();
            }
            return instance;
        }
    }

    public override void InheritedUpdate()
    {
        if (Player.Instance.GMode == GameMode.ObjectMenuMain)
        {
            if (!this.Show)
            {
                this.ShowItemMenu();
            }
        }
        else
        {
            if (this.Show)
            {
                this.HideItemMenu();
            }
        }
        this.transform.LookAt(Player.Instance.Head, Vector3.up);
    }

    public override void Rebuild(Object target)
    {
        base.Rebuild(target);

        foreach (ObjectMenuOption option in Options.Values)
        {
            option.Unset();
        }
        if (Target.MenuOptions.Contains(ObjectMenuOptionType.PickUp))
        {
            this.Options[ObjectMenuOptionType.PickUp].SetUp(this.Target.PickUp);
        }
        if (Target.MenuOptions.Contains(ObjectMenuOptionType.Move))
        {
            this.Options[ObjectMenuOptionType.Move].SetUp(this.SwitchToItemMenuMove);
        }
        if (Target.MenuOptions.Contains(ObjectMenuOptionType.Container))
        {
            this.Options[ObjectMenuOptionType.Container].SetUp((((Container)this.Target).SwitchToContainer));
        }
        if (Target.MenuOptions.Contains(ObjectMenuOptionType.OpenClose))
        {
            this.Options[ObjectMenuOptionType.OpenClose].SetUp((((Door)this.Target).SwitchOpenDoor));
        }
    }

    public void SwitchToItemMenuMove()
    {
        Player.Instance.GMode = GameMode.ItemMenuMove;
        ItemMenuMove.Instance.Rebuild(this.Target);
    }
}
