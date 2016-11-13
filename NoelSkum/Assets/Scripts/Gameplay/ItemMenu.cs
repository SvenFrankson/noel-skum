using UnityEngine;
using System.Collections;

public class ItemMenu : MonoBehaviour {

    protected Item Target;
    public ItemMenuOption[] Options;
    protected bool Show;

    public void Start()
    {
        this.HideItemMenu();
    }

    public void Update()
    {
        this.InheritedUpdate();
    }

    public virtual void InheritedUpdate()
    {

    }

    public virtual void Rebuild(Item target)
    {
        this.Target = target;
        this.transform.position = this.Target.Position;
    }

    public void ShowItemMenu()
    {
        Debug.Log("ShowItemMenu");
        foreach (ItemMenuOption option in this.Options)
        {
            option.gameObject.SetActive(true);
        }
        this.Show = true;
    }

    public void HideItemMenu()
    {
        foreach (ItemMenuOption option in this.Options)
        {
            option.gameObject.SetActive(false);
        }
        this.Show = false;
    }
}
