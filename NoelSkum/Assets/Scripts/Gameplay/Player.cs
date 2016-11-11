using UnityEngine;
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

    public void Start()
    {
        this.panelPreview = new Panel(-1, -1, -1, 0);
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

        if (Input.GetMouseButton(1))
        {
            this.transform.rotation = Quaternion.AngleAxis(-5f * Input.GetAxis("Mouse X"), this.transform.up) * this.transform.rotation;
            this.Head.rotation = Quaternion.AngleAxis(5f * Input.GetAxis("Mouse Y"), this.transform.right) * this.Head.rotation;
        }
        if (Input.GetMouseButtonDown(0))
        {
            this.PutPanelAtMouse();
        }
        UpdatePreviewPanel();
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
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponent<PanelInstance>() != null)
            {
                Vector3 worldPos = hit.point + hit.normal * 0.5f;
                int iPos, jPos, kPos;
                Panel.WorldPosToPanelPos(out iPos, out jPos, out kPos, worldPos);
                Debug.Log("WorldPosToPanelPos");
                Debug.Log("WorldPos = " + worldPos);
                Debug.Log("IPos = " + iPos + ". JPos = " + jPos + ". KPos = " + kPos);
                Grid.Instance.AddPanel(iPos, jPos, kPos, 1);
            }
        }
    }

    public void UpdatePreviewPanel()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
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
}
