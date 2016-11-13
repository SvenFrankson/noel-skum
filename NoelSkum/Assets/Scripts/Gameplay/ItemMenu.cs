using UnityEngine;
using System.Collections;

public class ItemMenu : MonoBehaviour {

    public static ItemMenu instance;
    public static ItemMenu Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ItemMenu>();
            }
            return instance;
        }
    }

    private Item Target;

    public ItemMenuOption[] Options;

    
	void Update () {
        this.transform.LookAt(Player.Instance.Head, Vector3.up);
	}

    public void Rebuild(Item target)
    {
        this.Target = target;
        this.gameObject.SetActive(true);
        this.transform.position = this.Target.Position;
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
        }
    }

    public void HideItemMenu()
    {
        this.gameObject.SetActive(false);
    }
}
