﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{

    public Transform head;
    public Transform Head
    {
        get
        {
            if (head == null)
            {
                head = FindObjectOfType<Camera>().transform;
            }
            return head;
        }
    }
    public Rigidbody c_Rigidbody;
    public Rigidbody C_Rigidbody
    {
        get
        {
            if (c_Rigidbody == null)
            {
                c_Rigidbody = this.GetComponent<Rigidbody>();
            }
            return c_Rigidbody;
        }
    }
    public Panel panelPreview;
    public GameObject panelPreviewInstance;
    public int itemPreviewRot;
    public Item itemPreview;

    public void Start()
    {
        this.panelPreview = new Panel(-1, -1, -1, 0);
        this.itemPreview = Item.ItemConstructor(0, 0, 0, 0, new byte[] { 0, 0, 0, 0 });
    }

    public void OnGUI()
    {
        GUILayout.BeginArea(Rect.MinMaxRect(0.05f * Screen.width, 0.05f * Screen.height, 0.2f * Screen.width, 0.3f * Screen.height));
        if (GUILayout.Button("Save"))
        {
            NoelSkumGame.Instance.Save();
        }
        GUILayout.TextField("ItemPreviewRotation = " + this.itemPreviewRot);
        GUILayout.EndArea();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            this.C_Rigidbody.MovePosition(this.transform.position + 5f * Time.deltaTime * this.transform.forward);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.C_Rigidbody.MovePosition(this.transform.position - 5f * Time.deltaTime * this.transform.forward);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            this.C_Rigidbody.MovePosition(this.transform.position - 5f * Time.deltaTime * this.transform.right);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.C_Rigidbody.MovePosition(this.transform.position + 5f * Time.deltaTime * this.transform.right);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            this.itemPreviewRot = (this.itemPreviewRot + 1) % 4;
        }

        if (Input.GetMouseButton(1))
        {
            this.transform.rotation = Quaternion.AngleAxis(-5f * Input.GetAxis("Mouse X"), this.transform.up) * this.transform.rotation;
            this.Head.rotation = Quaternion.AngleAxis(5f * Input.GetAxis("Mouse Y"), this.transform.right) * this.Head.rotation;
        }
        if (Input.GetMouseButtonDown(0))
        {
            this.PutItemAtMouse();
        }
        //UpdatePreviewPanel();
        UpdatePreviewItem();
    }

    public void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.C_Rigidbody.AddForce(5f * this.transform.up, ForceMode.Impulse);
        }
    }

    public void PutPanelAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 4f))
        {
            if (hit.collider.GetComponent<PanelInstance>() != null)
            {
                Vector3 worldPos = hit.point + hit.normal * 0.5f;
                int iPos, jPos, kPos;
                Panel.WorldPosToPanelPos(out iPos, out jPos, out kPos, worldPos);
                Debug.Log("WorldPosToPanelPos");
                Debug.Log("WorldPos = " + worldPos);
                Debug.Log("IPos = " + iPos + ". JPos = " + jPos + ". KPos = " + kPos);
                NoelSkumGame.Instance.AddPanel(iPos, jPos, kPos, 1);
            }
        }
    }

    public void UpdatePreviewPanel()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 4f))
        {
            if (hit.collider.GetComponent<PanelInstance>() != null)
            {
                Vector3 worldPos = hit.point + hit.normal * 0.5f;
                int iPos, jPos, kPos;
                Panel.WorldPosToPanelPos(out iPos, out jPos, out kPos, worldPos);
                int changed = panelPreview.Update(iPos, jPos, kPos, 1);
                if (changed == 1)
                {
                    Destroy(panelPreviewInstance);
                    GameObject prefab = Resources.Load<GameObject>("Prefabs/" + panelPreview.Reference.ToString());
                    panelPreviewInstance = Instantiate<GameObject>(prefab);
                    Destroy(panelPreviewInstance.GetComponent<Collider>());
                    panelPreviewInstance.transform.position = panelPreview.Position;
                    panelPreviewInstance.transform.rotation = panelPreview.Rotation;
                }
                if (this.panelPreviewInstance != null)
                {
                    this.panelPreviewInstance.gameObject.SetActive(true);
                    return;
                }
            }
        }
        HidePreviewPanel();
    }

    public void HidePreviewPanel()
    {
        if (this.panelPreviewInstance != null)
        {
            this.panelPreviewInstance.gameObject.SetActive(false);
        }
    }

    public void PutItemAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 4f))
        {
            Vector3 worldPos = hit.point + hit.normal * 0.5f;
            int iPos, jPos, kPos;
            Item.WorldPosToItemPos(out iPos, out jPos, out kPos, worldPos);
            NoelSkumGame.Instance.AddItem(iPos, jPos, kPos, this.itemPreviewRot, new byte[] { 0, 0, 0, 0 });
        }
    }

    public void UpdatePreviewItem()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 4f))
        {
            Vector3 worldPos = hit.point + hit.normal * 0.5f;
            int iPos, jPos, kPos;
            Item.WorldPosToItemPos(out iPos, out jPos, out kPos, worldPos);
            itemPreview.UpdateItem(iPos, jPos, kPos, this.itemPreviewRot);
            this.itemPreview.gameObject.SetActive(true);
            return;
        }
        this.itemPreview.gameObject.SetActive(false);
    }
}
