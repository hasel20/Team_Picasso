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

    //Wand() 필요
    public GameObject lineFactory;//라인 
    public GameObject finger;//라인 
    LineRenderer liner;
    //세팅에 필요 
    Color lineColor;    //Select_Palette() 
    float width;        //Select_Width()
    GameObject figure;  //Select_Figure()
    public GameObject[] Figures;//1
    //Inventory_Active();  필요 
    public GameObject[] inventory; //설정 창들 
    int count = -1;
    //Draw()필요
    LineRenderer lr;
    List<Vector3> points = new List<Vector3>();

    Vector3 pos;
    SyncData[] syncData;

    float wandLength = 0.1f;

    void Start()
    {
        if (photonView.IsMine)
        {
            //=======인벤토리 비활성화
            for (int i = 0; i < inventory.Length; i++)
            {
                inventory[i].SetActive(false);
            }
            inventory[3].SetActive(true);
            //--------Line기본 색과 굵기 정의
            lineColor = lineFactory.GetComponent<LineRenderer>().startColor;
            width = lineFactory.GetComponent<LineRenderer>().startWidth;
            //기본 도형 설정 
            figure = Figures[0];
            //손가락 위치 지정 후 비활성화
            finger.transform.position = rayTransform.position + rayTransform.forward * wandLength;
            finger.SetActive(false);
        }
    } 

    void Update()
    {
        //움직임
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
    //지휘봉!(지휘봉으로 세틴하는거야)
    void Wand()
    {
        //Ray ray = new Ray(rhand.position, rhand.forward);
        //선택 오른손에서 레이를 쏴서 빛을 본다. 
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            liner = Instantiate(lineFactory).GetComponent<LineRenderer>();
            liner.SetWidth(0.005f, 0.005f);
            liner.SetColors(Color.white, Color.white);
            //지휘봉의 길이는 일정
            liner.positionCount = 2;
            liner.SetPosition(0, rayTransform.position);
            liner.SetPosition(1, rayTransform.position + rayTransform.forward * wandLength);
            //접촉 인식할 포인터의 
            finger.SetActive(true);
            liner.transform.SetParent(rayTransform);

        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            Destroy(liner.gameObject);
            liner = null;
            finger.SetActive(false);
        }
        //============//포인터가 홧성화 되었을떄 닿는 물채애 따라 고를 수있게에 
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
                    print("정육면체");
                    break;
                case "Sphere":
                    figure = Figures[1];
                    print("구");
                    break;
                case "Capsule":
                    figure = Figures[2];
                    print("알약");
                    break;
                case "Cylinder":
                    figure = Figures[3];
                    print("원기둥");
                    break;
                default:
                    break;
            }
        }
    }
    
    //인벤 창 활성화 순서대로!
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

    //그림 그리긔(그릴떄 세팅값넣어줘유)
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
        Set_Line(lr);//그릴 떄 전세 에팅해뫃은 값으로 적용

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