using UnityEngine;
using System.Collections;

public class ItemMenuMain : ItemMenu
{
    public static ItemMenuMain instance;
    public static ItemMenuMain Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ItemMenuMain>();
            }
            return instance;
        }
    }

    public override void InheritedUpdate()
    {
        if (Player.Instance.GMode == GameMode.ItemMenuMain)
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

    public override void Rebuild(Item target)
    {
        base.Rebuild(target);

        foreach (ItemMenuOption option in Options)
        {
            option.Unset();
        }
        foreach (ItemMenuOption option in Options)
        {
            if (option.OptionType == ItemMenuOptionType.PickUp)
            {
                option.SetUp(this.Target.PickUp);
            }
            else if (option.OptionType == ItemMenuOptionType.Move)
            {
                option.SetUp(this.SwitchToItemMenuMove);
            }
        }
    }

    public void SwitchToItemMenuMove()
    {
        Player.Instance.GMode = GameMode.ItemMenuMove;
        ItemMenuMove.Instance.Rebuild(this.Target);
    }
}
