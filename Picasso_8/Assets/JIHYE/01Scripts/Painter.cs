using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Painter : MonoBehaviourPun, IPunObservable
{
    public OVRInput.Button OpenPaletteButton;
    public OVRInput.Button ClosePaletteButton;

    struct SyncData
    {
        public Vector3 pos;
        public Quaternion rot;
    }
    public Transform rayTransform;

    //Wand() �ʿ�
    public GameObject lineFactory;//���� 
    public GameObject finger;//���� 
    LineRenderer liner;
    //���ÿ� �ʿ� 
    Color lineColor;    //Select_Palette() 
    float width;        //Select_Width()
    GameObject figure;  //Select_Figure()
    public GameObject[] Figures;//1
    //Inventory_Active();  �ʿ� 
    public GameObject[] inventory; //���� â�� 
    int count = -1;
    //Draw()�ʿ�
    LineRenderer lr;
    List<Vector3> points = new List<Vector3>();

    Vector3 pos;
    SyncData[] syncData;

    float wandLength = 0.1f;

    void Start()
    {
        if (photonView.IsMine)
        {
            //=======�κ��丮 ��Ȱ��ȭ
            for (int i = 0; i < inventory.Length; i++)
            {
                inventory[i].SetActive(false);
            }
            inventory[3].SetActive(true);
            //--------Line�⺻ ���� ���� ����
            lineColor = lineFactory.GetComponent<LineRenderer>().startColor;
            width = lineFactory.GetComponent<LineRenderer>().startWidth;
            //�⺻ ���� ���� 
            figure = Figures[0];
            //�հ��� ��ġ ���� �� ��Ȱ��ȭ
            finger.transform.position = rayTransform.position + rayTransform.forward * wandLength;
            finger.SetActive(false);
        }
    } 

    void Update()
    {
        //������
        if (photonView.IsMine)
        {
            Wand();
            Inventory_Active();
            Draw();

            //photonView.RPC("Start_Draw",RpcTarget.AllBuffered);
            //photonView.RPC("Curr_Draw", RpcTarget.AllBuffered);
            //photonView.RPC("End_Draw", RpcTarget.AllBuffered);            
        }
    }
    //���ֺ�!(���ֺ����� ��ƾ�ϴ°ž�)
    void Wand()
    {
        //Ray ray = new Ray(rhand.position, rhand.forward);
        //���� �����տ��� ���̸� ���� ���� ����. 
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            liner = Instantiate(lineFactory).GetComponent<LineRenderer>();
            liner.SetWidth(0.005f, 0.005f);
            liner.SetColors(Color.white, Color.white);
            //���ֺ��� ���̴� ����
            liner.positionCount = 2;
            liner.SetPosition(0, rayTransform.position);
            liner.SetPosition(1, rayTransform.position + rayTransform.forward * wandLength);
            //���� �ν��� �������� 
            finger.SetActive(true);
            liner.transform.SetParent(rayTransform);

        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            Destroy(liner.gameObject);
            liner = null;
            finger.SetActive(false);
        }
        //============//�����Ͱ� ȱ��ȭ �Ǿ����� ��� ��ä�� ���� �� ���ְԿ� 
        //if (point&&photonView.IsMine)
        if (finger)
        {
            Collider[] hits = Physics.OverlapSphere(finger.transform.position, 0.1f);
            if (hits.Length > 0)
            {
                Select_Palette(hits[0].gameObject);
                Select_Width(hits[0].gameObject);
                Select_Figure(hits[0].gameObject);
            }
        }
    }
    void Select_Palette(GameObject hit)
    {
        if (hit.CompareTag("Palette"))
        {
            //go.name
            switch (hit.name)
            {
                case "RED":
                    lineColor = Color.red;
                    break;
                case "YELLOW":
                    lineColor = Color.yellow;
                    break;
                case "BLUE":
                    lineColor = Color.blue;
                    break;
                case "GREEN":
                    lineColor = Color.green;
                    break;
                case "BLACK":
                    lineColor = Color.black;
                    break;
                case "WHITE":
                    lineColor = Color.white;
                    break;
                default:
                    break;
            }
        }
    }
    void Select_Width(GameObject hit)
    {
        if (hit.CompareTag("Slider"))
        {
            switch (hit.name)
            {
                case "Large":
                    width = 0.05f;
                    break;
                case "Medium":
                    width = 0.03f;
                    break;
                case "Small":
                    width = 0.01f;
                    break;
                default:
                    break;
            }
        }
    }
    void Select_Figure(GameObject hit)
    {
        if (hit.CompareTag("Figure"))
        {
            switch (hit.name)
            {
                case "Cube":
                    figure = Figures[0];
                    print("������ü");
                    break;
                case "Sphere":
                    figure = Figures[1];
                    print("��");
                    break;
                case "Capsule":
                    figure = Figures[2];
                    print("�˾�");
                    break;
                case "Cylinder":
                    figure = Figures[3];
                    print("�����");
                    break;
                default:
                    break;
            }
        }
    }
    
    //�κ� â Ȱ��ȭ �������!
    void Inventory_Active()
    {

        if (OVRInput.GetDown(OpenPaletteButton, OVRInput.Controller.LTouch))
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                inventory[i].SetActive(false);
            }
            count++;
            inventory[count % inventory.Length].SetActive(true);
        }


        if (OVRInput.GetDown(ClosePaletteButton, OVRInput.Controller.LTouch))
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                inventory[i].SetActive(false);
            }
            inventory[3].SetActive(true);
            count = -1;
        }
    }

    //�׸� �׸���(�׸��� ���ð��־�����)
    void Draw()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            photonView.RPC("Start_Draw",RpcTarget.AllBuffered);
        }
        else if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            photonView.RPC("Curr_Draw", RpcTarget.AllBuffered);
        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            photonView.RPC("End_Draw", RpcTarget.AllBuffered);
        }
    }
    [PunRPC]
    public void Start_Draw()
    {
        GameObject line = Instantiate(lineFactory);
        lr = line.GetComponent<LineRenderer>();
        Set_Line(lr);//�׸� �� ���� �����ؑ��� ������ ����

        points.Add(rayTransform.position);
        lr.positionCount = 1;
        lr.SetPosition(0, points[0]);
    }
    [PunRPC]
    public void Curr_Draw()
    {
        Vector3 pos = rayTransform.position;
        if (Vector3.Distance(points[points.Count - 1], pos) > 0.01f)
        {
            points.Add(pos);
            lr.positionCount++;
            lr.SetPosition(points.Count - 1, pos);
        }
    }
    [PunRPC]
    public void End_Draw()
    {
        MeshCollider ms = lr.GetComponent<MeshCollider>();
        Mesh mesh = new Mesh();
        lr.BakeMesh(mesh, true);
        ms.sharedMesh = mesh;
        points.Clear();
        lr = null;
    }
    void Set_Line(LineRenderer line)
    {
        //if (photonView.IsMine)
        {
            line.SetColors(lineColor, lineColor);
            line.SetWidth(width, width);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}