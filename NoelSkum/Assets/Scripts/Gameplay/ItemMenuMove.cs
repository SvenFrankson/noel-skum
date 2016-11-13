using UnityEngine;
using System.Collections;

public class ItemMenuMove : ItemMenu {

    public static ItemMenuMove instance;
    public static ItemMenuMove Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ItemMenuMove>();
            }
            return instance;
        }
    }

    public override void InheritedUpdate()
    {
        if (Player.Instance.GMode == GameMode.ItemMenuMove)
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
        if (Target != null)
        {
            this.transform.position = Target.transform.position;
        }
    }

    public override void Rebuild(Item target)
    {
        base.Rebuild(target);
        foreach (ItemMenuOption option in Options)
        {
            if (option.OptionType == ItemMenuOptionType.MoveRight)
            {
                option.SetUp(this.Target.MoveRight);
            }
            else if (option.OptionType == ItemMenuOptionType.MoveUp)
            {
                option.SetUp(this.Target.MoveUp);
            }
            else if (option.OptionType == ItemMenuOptionType.MoveForward)
            {
                option.SetUp(this.Target.MoveForward);
            }
            else if (option.OptionType == ItemMenuOptionType.MoveLeft)
            {
                option.SetUp(this.Target.MoveLeft);
            }
            else if (option.OptionType == ItemMenuOptionType.MoveDown)
            {
                option.SetUp(this.Target.MoveDown);
            }
            else if (option.OptionType == ItemMenuOptionType.MoveBack)
            {
                option.SetUp(this.Target.MoveBack);
            }
        }
    }
}
