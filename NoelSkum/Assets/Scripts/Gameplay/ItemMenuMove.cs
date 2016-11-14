using UnityEngine;
using System.Collections;

public class ItemMenuMove : ObjectMenu {

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

    public override void Rebuild(Object target)
    {
        base.Rebuild(target);
        this.Options[ObjectMenuOptionType.MoveRight].SetUp(((Item)this.Target).MoveRight);
        this.Options[ObjectMenuOptionType.MoveUp].SetUp(((Item)this.Target).MoveUp);
        this.Options[ObjectMenuOptionType.MoveForward].SetUp(((Item)this.Target).MoveForward);
        this.Options[ObjectMenuOptionType.MoveLeft].SetUp(((Item)this.Target).MoveLeft);
        this.Options[ObjectMenuOptionType.MoveDown].SetUp(((Item)this.Target).MoveDown);
        this.Options[ObjectMenuOptionType.MoveBack].SetUp(((Item)this.Target).MoveBack);
        this.Options[ObjectMenuOptionType.RotatePlus].SetUp(((Item)this.Target).RotatePlus);
        this.Options[ObjectMenuOptionType.RotateMinus].SetUp(((Item)this.Target).RotateMinus);
    }
}
