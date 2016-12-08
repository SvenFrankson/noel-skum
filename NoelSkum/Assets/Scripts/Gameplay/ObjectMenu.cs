using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectMenu : MonoBehaviour {

    protected Object Target;
    protected Dictionary<ObjectMenuOptionType, ObjectMenuOption> Options;
    protected bool Show;

    public void Start()
    {this.Options = new Dictionary<ObjectMenuOptionType, ObjectMenuOption>();
        ObjectMenuOption[] options = this.GetComponentsInChildren<ObjectMenuOption>(true);
        foreach (ObjectMenuOption option in options)
        {
            this.Options.Add(option.OptionType, option);
        }
        this.HideItemMenu();
        Debug.Log("Options = " + Options.Count);
    }

    public void Update()
    {
        this.InheritedUpdate();
    }

    public virtual void InheritedUpdate()
    {

    }

    public virtual void Rebuild(Object target)
    {
        this.Target = target;
        this.transform.position = this.Target.Position();
    }

    public void ShowItemMenu()
    {
        if (this.Target != null)
        {
            this.Target.HighLight(new Color(150f / 255f, 200f / 255f, 80f / 255f), 0.01f);
        }
        foreach (ObjectMenuOption option in this.Options.Values)
        {
            option.gameObject.SetActive(true);
        }
        this.Show = true;
    }

    public void HideItemMenu()
    {
        if (this.Target != null)
        {
            this.Target.HighLight(Color.black, 0.005f);
        }
        foreach (ObjectMenuOption option in this.Options.Values)
        {
            option.gameObject.SetActive(false);
        }
        this.Show = false;
    }
}
