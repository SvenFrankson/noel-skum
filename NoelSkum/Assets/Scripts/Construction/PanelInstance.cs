using UnityEngine;
using System.Collections;

public class PanelInstance : MonoBehaviour {

    private Panel panel;

    public static PanelInstance PanelConstructor(Panel panel)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + panel.Reference.ToString());
        GameObject g = Instantiate<GameObject>(prefab);
        PanelInstance p = g.AddComponent<PanelInstance>();
        p.panel = panel;
        g.transform.position = p.panel.Position;
        g.transform.rotation = p.panel.Rotation;

        return p;
    }
}
