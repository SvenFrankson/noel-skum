using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    private List<byte> panels;
    private List<byte[]> items;

    public void Start()
    {
        this.panels = new List<byte>();
        this.items = new List<byte[]>();
        panels.Add(0);
        panels.Add(1);
        panels.Add(2);
        items.Add(new byte[] { 0, 0, 0, 0 });
        items.Add(new byte[] { 0, 0, 0, 1 });
    }

    public void OnGUI()
    {
        if (Player.Instance.GMode == GameMode.Inventory)
        {
            GUILayout.BeginArea(Rect.MinMaxRect(0.25f * Screen.width, 0.25f * Screen.height, 0.75f * Screen.width, 0.75f * Screen.height));
            GUILayout.TextArea("Panels");
            foreach (byte b in this.panels)
            {
                if (GUILayout.Button("p_" + b))
                {
                    Player.Instance.SwitchToSetPanel(b);
                }
            }
            GUILayout.TextArea("Items");
            foreach (byte[] b in this.items)
            {
                if (GUILayout.Button(b.ToString()))
                {
                    Player.Instance.SwitchToSetItem(b);
                }
            }
            GUILayout.EndArea();
        }
    }
}
