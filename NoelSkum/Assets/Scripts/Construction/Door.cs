using UnityEngine;
using System.Collections;

public class Door : Panel
{
    private Animator c_animator;
    private Animator C_Animator
    {
        get
        {
            if (c_animator == null)
            {
                c_animator = this.GetComponent<Animator>();
            }
            return c_animator;
        }
    }

    private bool openedDoor;
    public bool OpenedDoor
    {
        get
        {
            return this.openedDoor;
        }
        set
        {
            if (this.openedDoor != value)
            {
                if (value == true)
                {
                    this.Open();
                }
                else
                {
                    this.Close();
                }
            }
            this.openedDoor = value;
        }
    }

    public void Open()
    {
        this.C_Animator.SetTrigger("Open");
    }

    public void Close()
    {
        this.C_Animator.SetTrigger("Close");
    }

    public void SwitchOpenDoor()
    {
        this.OpenedDoor = !this.OpenedDoor;
    }
}
