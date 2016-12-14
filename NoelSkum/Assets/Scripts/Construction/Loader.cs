using System.Collections.Generic;
using UnityEngine;

public class Loader
{
    static private Dictionary<string, GameObject> LoadableResources = new Dictionary<string, GameObject>();

    static Loader()
    {
        GameObject[] AllResources = Resources.LoadAll<GameObject>("Prefabs");
        foreach (GameObject g in AllResources)
        {
            Object o = g.GetComponent<Object>();
            if (o != null)
            {
                Debug.Log("Loader : Loading asset \"" + o.Reference + "\".");
                LoadableResources.Add(o.Reference, g);
            }
        }
        Debug.Log("Loader : " + LoadableResources.Count + " assets loaded.");
    }

    static public GameObject Get(string reference)
    {
        return LoadableResources[reference];
    }
}
