﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameMode
{
    Normal,
    Inventory,
    Container,
    ObjectMenuMain,
    ItemMenuMove,
    SetPanel,
    SetItem
}

public class Player : MonoBehaviour
{
    public static Player instance;
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }
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

    public GameMode gMode;
    public GameMode GMode
    {
        get
        {
            return this.gMode;
        }
        set {
            this.HidePreviewPanel();
            this.HidePreviewItem();
            this.gMode = value;
            if (this.gMode != GameMode.Container)
            {
                Inventory.Instance.TargetContainer = null;
            }
        }
    }
    public Panel panelPreview;
    public int itemPreviewRot;
    public Item itemPreview;

    public LayerMask itemMenuOptionLayerMask;

    public InventoryObject equiped = null;

    public void Start()
    {
        this.panelPreview = Panel.PanelConstructor(Coordinates.Zero, "00000000");
        Destroy(panelPreview.GetComponent<Collider>());
        this.itemPreview = Item.ItemConstructor(Coordinates.Zero, 0, "01000000");
        Destroy(itemPreview.GetComponent<Collider>());
        this.GMode = GameMode.Normal;
    }

    public void OnGUI()
    {
        GUILayout.BeginArea(Rect.MinMaxRect(0.05f * Screen.width, 0.05f * Screen.height, 0.2f * Screen.width, 0.3f * Screen.height));
        if (GUILayout.Button("Save"))
        {
            NoelSkumGame.Instance.Save();
        }
        GUILayout.TextField("Current GameMode : " + GMode.ToString());
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
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (this.GMode == GameMode.Normal)
            {
                this.GMode = GameMode.Inventory;
            }
            else if (this.GMode == GameMode.Inventory)
            {
                this.GMode = GameMode.Normal;
            }
            else if (this.GMode == GameMode.Container)
            {
                this.GMode = GameMode.Normal;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (this.GMode == GameMode.SetPanel)
            {
                this.UnEquip();
            }
            else if (this.GMode == GameMode.SetItem)
            {
                this.UnEquip();
            }
            else if (this.GMode == GameMode.Inventory)
            {
                this.GMode = GameMode.Normal;
            }
            else if (this.GMode == GameMode.Container)
            {
                this.GMode = GameMode.Normal;
            }
        }

        if (Input.GetMouseButton(1))
        {
            this.transform.rotation = Quaternion.AngleAxis(-5f * Input.GetAxis("Mouse X"), this.transform.up) * this.transform.rotation;
            this.Head.rotation = Quaternion.AngleAxis(5f * Input.GetAxis("Mouse Y"), this.transform.right) * this.Head.rotation;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (this.GMode == GameMode.Normal)
            {
                SelectObject();
            }
            else if (this.GMode == GameMode.ObjectMenuMain)
            {
                SelectObjectMenuOption();
            }
            else if (this.GMode == GameMode.ItemMenuMove)
            {
                SelectMoveItem();
            }
            else if (this.GMode == GameMode.SetPanel)
            {
                PutPanelAtMouse();
            }
            else if (this.GMode == GameMode.SetItem)
            {
                PutItemAtMouse();
            }
        }

        if (this.GMode == GameMode.SetPanel)
        {
            UpdatePreviewPanel();
        }
        if (this.GMode == GameMode.SetItem)
        {
            UpdatePreviewItem();
        }
    }

    public void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.C_Rigidbody.AddForce(5f * this.transform.up, ForceMode.Impulse);
        }
    }

    public void Equip(InventoryObject target) {
        this.equiped = target;
        if (this.equiped.GetType() == typeof(InventoryPanel)) {
            this.SwitchToSetPanel(this.equiped.Reference);
        }
        else if (this.equiped.GetType() == typeof(InventoryItem))
        {
            this.SwitchToSetItem(this.equiped.Reference);
        }
    }

    public void UnEquip()
    {
        if (this.equiped != null)
        {
            Inventory.Instance.Add(this.equiped);
        }
        this.equiped = null;
        this.GMode = GameMode.Normal;
    }

    public void UseEquip()
    {
        this.equiped = null;
        this.GMode = GameMode.Normal;
    }

    public void SelectObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 8f))
        {
            if (hit.collider.GetComponent<Object>() != null)
            {
                GMode = GameMode.ObjectMenuMain;
                ObjectMenuMain.Instance.Rebuild(hit.collider.GetComponent<Object>());
            }
        }
    }

    public void SelectObjectMenuOption()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 8f, this.itemMenuOptionLayerMask))
        {
            if (hit.collider.GetComponent<ObjectMenuOption>() != null)
            {
                hit.collider.GetComponent<ObjectMenuOption>().Activate();
                return;
            }
        }
        GMode = GameMode.Normal;
    }

    public void SelectMoveItem()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 8f, this.itemMenuOptionLayerMask))
        {
            if (hit.collider.GetComponent<ObjectMenuOption>() != null)
            {
                hit.collider.GetComponent<ObjectMenuOption>().Activate();
                return;
            }
        }
        GMode = GameMode.Normal;
    }

    public void SwitchToSetPanel(string reference)
    {
        Destroy(this.panelPreview.gameObject);
        this.panelPreview = Panel.PanelConstructor(Coordinates.Zero, reference);
        Destroy(panelPreview.GetComponent<Collider>());
        this.GMode = GameMode.SetPanel;
    }

    public void PutPanelAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 4f))
        {
            if ((hit.collider.GetComponent<Panel>() != null) || (hit.collider.GetComponent<Chunck>() != null))
            {
                Vector3 worldPos = hit.point + hit.normal * 0.5f;
                Coordinates cGlobal = panelPreview.WorldPosToPanelPos(worldPos);
                NoelSkumGame.Instance.AddPanel(cGlobal, this.panelPreview.Reference);
                InventoryPanel next = Inventory.Instance.FindSamePanel(this.equiped);
                this.UseEquip();
                if (next != null)
                {
                    Debug.Log("Another object found. Keep going.");
                    next.EquipObject();
                }
            }
        }
    }

    public void UpdatePreviewPanel()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 4f))
        {
            if ((hit.collider.GetComponent<Panel>() != null) || (hit.collider.GetComponent<Chunck>() != null))
            {
                Vector3 worldPos = hit.point + hit.normal * 0.5f;
                Coordinates cGlobal = panelPreview.WorldPosToPanelPos(worldPos);
                panelPreview.UpdatePos(cGlobal);
                this.panelPreview.gameObject.SetActive(true);
                return;
            }
        }
        HidePreviewPanel();
    }

    public void HidePreviewPanel()
    {
        if (this.panelPreview != null)
        {
            this.panelPreview.gameObject.SetActive(false);
        }
    }

    public void SwitchToSetItem(string reference)
    {
        Debug.Log(reference.ToString() + " : " + reference.Length);
        Destroy(this.itemPreview.gameObject);
        this.itemPreview = Item.ItemConstructor(Coordinates.Zero, 0, reference);
        Destroy(itemPreview.GetComponent<Collider>());
        this.GMode = GameMode.SetItem;
    }

    public void PutItemAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 4f))
        {
            Vector3 worldPos = hit.point + hit.normal * 0.25f;
            Coordinates cGlobal = Item.WorldPosToItemPos(worldPos);
            NoelSkumGame.Instance.AddItem(cGlobal, this.itemPreviewRot, this.itemPreview.Reference);
            this.UseEquip();
        }
    }

    public void UpdatePreviewItem()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 4f))
        {
            Vector3 worldPos = hit.point + hit.normal * 0.25f;
            Coordinates cGlobal = Item.WorldPosToItemPos(worldPos);
            itemPreview.UpdatePos(cGlobal, this.itemPreviewRot);
            this.itemPreview.gameObject.SetActive(true);
            return;
        }
        this.HidePreviewItem();
    }

    public void HidePreviewItem()
    {
        this.itemPreview.gameObject.SetActive(false);
    }
}
