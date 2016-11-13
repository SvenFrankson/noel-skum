using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameMode
{
    Normal,
    ItemMenuMain,
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
        }
    }
    public Panel panelPreview;
    public GameObject panelPreviewInstance;
    public int itemPreviewRot;
    public Item itemPreview;

    public LayerMask itemMenuOptionLayerMask;

    public void Start()
    {
        this.panelPreview = new Panel(-1, -1, -1, 0);
        this.itemPreview = Item.ItemConstructor(0, 0, 0, 0, new byte[] { 0, 0, 0, 0 });
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
        if (GUILayout.Button("GameMode Normal"))
        {
            this.GMode = GameMode.Normal;
        }
        if (GUILayout.Button("GameMode SetPanel"))
        {
            this.GMode = GameMode.SetPanel;
        }
        if (GUILayout.Button("GameMode SetItem"))
        {
            this.GMode = GameMode.SetItem;
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

        if (Input.GetMouseButton(1))
        {
            this.transform.rotation = Quaternion.AngleAxis(-5f * Input.GetAxis("Mouse X"), this.transform.up) * this.transform.rotation;
            this.Head.rotation = Quaternion.AngleAxis(5f * Input.GetAxis("Mouse Y"), this.transform.right) * this.Head.rotation;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (this.GMode == GameMode.Normal)
            {
                SelectItem();
            }
            else if (this.GMode == GameMode.ItemMenuMain)
            {
                SelectItemMenuOption();
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

    public void SelectItem()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 8f))
        {
            if (hit.collider.GetComponent<Item>() != null)
            {
                GMode = GameMode.ItemMenuMain;
                ItemMenuMain.Instance.Rebuild(hit.collider.GetComponent<Item>());
            }
        }
    }

    public void SelectItemMenuOption()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 8f, this.itemMenuOptionLayerMask))
        {
            if (hit.collider.GetComponent<ItemMenuOption>() != null)
            {
                hit.collider.GetComponent<ItemMenuOption>().Activate();
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
            if (hit.collider.GetComponent<ItemMenuOption>() != null)
            {
                hit.collider.GetComponent<ItemMenuOption>().Activate();
                return;
            }
        }
        GMode = GameMode.Normal;
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
        this.HidePreviewItem();
    }

    public void HidePreviewItem()
    {
        this.itemPreview.gameObject.SetActive(false);
    }
}
