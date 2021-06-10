using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDrawing : MonoBehaviourPun
{
    //기본 신체 세팅 
    GameObject rhand_R;
    GameObject lhand_L;
    public GameObject myHandL;
    public GameObject myHandR;
    public GameObject otherHandL;
    public GameObject otherHandR;

    //Hold_Brush();필요.
    public GameObject brush; // 브러쉬는 처음 부터 오른손에 넣어서 놓고 시작
    LineRenderer b_stick;
    public float brushLength = 0.1f;

    //Inventory_Active();  필요 
    public GameObject[] inventory; //설정 창들 
    int count;

    //세팅 될 값들. 
    Color lineColor = Color.gray;
    float lineWidth = 0.01f;

    //DrawLine();필요
    public GameObject lineFactory;
    LineRenderer renderer;
    List<Vector3> points = new List<Vector3>();
    public int bezierCount = 10;

    //GrapLine();필요
    GameObject obj;
    //잡은 라인을 표시 하기 위한 빈오브젝트 리스트와 사용 오브젝트 리스트
    List<GameObject> e_obj = new List<GameObject>();
    List<GameObject> u_obj = new List<GameObject>();
    public int p_counts = 1000;
    public GameObject empty_obj;
    //===========
    //개인 키보드. 
    public GameObject keybord;
    public bool IsPainter = false;


    void Start()
    {
        //--나의 손인지 상대의 손인지에따라 위치 다시 지정해주긔
        if (photonView.IsMine)
        {
            lhand_L = myHandL;
            rhand_R = myHandR;
        }
        else
        {
            lhand_L = otherHandL;
            rhand_R = otherHandR;
        }
        //---------------------
        //빈자리에 넣어주고 빈오브젝트들은 왼손에 넣어놓기
        for (int i = 0; i < p_counts; i++)
        {
            GameObject a = Instantiate(empty_obj);
            a.transform.SetParent(lhand_L.transform);
            e_obj.Add(a);
        }
        //=======인벤토리 비활성화
        for (int i = 0; i < inventory.Length; i++)
        {
            inventory[i].SetActive(false);
        }
        inventory[0].SetActive(true);
        //브러쉬 위치 지정해주기
        brush.SetActive(true);
        brush.transform.position = rhand_R.transform.position + rhand_R.transform.forward * brushLength;
        brush.transform.SetParent(rhand_R.transform);//처음부터 자식으루 뒀엉,
        brush.SetActive(false);
        //시작 때 키보드는 모두 꺼주기.
        keybord.SetActive(false);
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (IsPainter)
            {
                //브러쉬를 쥐다.
                Hold_Brush();
                //팔레트등장 움직임에서 제어하긔 
                Inventory_Active();
                //그림을 그리다. 
                DrawLine();
                //그림을 지우다.
                DeletLine();
                //그림을 잡고 움직이다.
                GrapLine();
            }
            else
            {
                //키보드 꺼내기
                SetKeybord();
                //타자 치는 기능. 
                Typing();
            }
        }
        Graping();
    }

    void Hold_Brush()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            //접촉 인식할 포인터의 
            brush.SetActive(true);
            brush.transform.position = rhand_R.transform.position + rhand_R.transform.forward * brushLength;
        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            brush.SetActive(false);
        }

        //브러쉬가 켜져있을 때 
        if (brush)
        {
            Collider[] hits = Physics.OverlapSphere(brush.transform.position, 0.1f);
            if (hits.Length > 0)
            {
                Select_Palette(hits[0].gameObject);
                Select_Width(hits[0].gameObject);
            }
        }
    }
    private void Select_Palette(GameObject hit)
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
    private void Select_Width(GameObject hit)
    {
        if (hit.CompareTag("Slider"))
        {
            switch (hit.name)
            {
                case "Large":
                    lineWidth = 0.05f;
                    break;
                case "Medium":
                    lineWidth = 0.03f;
                    break;
                case "Small":
                    lineWidth = 0.01f;
                    break;
                default:
                    break;
            }
        }
    }

    void Inventory_Active()
    {

        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
                for (int i = 0; i < inventory.Length; i++)
                {
                    inventory[i].SetActive(false);
                }
                inventory[count % inventory.Length].SetActive(true);
                count++;
        }
        
    }

    void DrawLine()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            Vector3 color = new Vector3(lineColor.r, lineColor.g, lineColor.b);
            photonView.RPC("Start_Draw", RpcTarget.All, brush.transform.position, color, lineWidth);
        }
        else if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            photonView.RPC("Curr_Draw", RpcTarget.All, brush.transform.position);
        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            photonView.RPC("End_Draw", RpcTarget.All);
        }
    }

    [PunRPC]
    public void Start_Draw(Vector3 popo, Vector3 color, float width)
    {
        GameObject line = Instantiate(lineFactory);
        renderer = line.GetComponent<LineRenderer>();
        
        lineColor = new Color(color.x, color.y, color.z);
        lineWidth = width;
        
        Set_Line(renderer);//그릴 떄 전세 에팅해놓은 값으로 적용

        points.Add(popo);
        renderer.positionCount = 1;
        renderer.SetPosition(0, points[0]);
    }
    [PunRPC]
    public void Curr_Draw(Vector3 popo)
    {
        if (Vector3.Distance(points[points.Count - 1], popo) > 0.01f)
        {
            points.Add(popo);
            //renderer.positionCount++;
            //renderer.SetPosition(renderer.positionCount - 1, popo);
            if (points.Count >= 4)
            {
                BezierCurve(points[0], points[1], points[2], points[3]);
                Vector3 xx = points[3];
                points.Clear();
                points.Add(xx);
            }
        }
    }
    [PunRPC]
    public void End_Draw()
    {
        MeshCollider ms = renderer.GetComponent<MeshCollider>();
        Mesh mesh = new Mesh();
        renderer.BakeMesh(mesh, true);
        ms.sharedMesh = mesh;
        points.Clear();
        renderer = null;
    }

    private void BezierCurve(Vector3 pos1, Vector3 pos2, Vector3 pos3, Vector3 pos4)
    {
        List<Vector3> bezi = new List<Vector3>();
        for (float i = 0; i <= 1; i += 1.0f / bezierCount)
        {
            Vector3 p1 = Vector3.Lerp(pos1, pos2, i);
            Vector3 p2 = Vector3.Lerp(pos2, pos3, i);
            Vector3 p3 = Vector3.Lerp(pos3, pos4, i);

            Vector3 pp1 = Vector3.Lerp(p1, p2, i);
            Vector3 pp2 = Vector3.Lerp(p2, p3, i);

            Vector3 ppp = Vector3.Lerp(pp1, pp2, i);

            bezi.Add(ppp);
            renderer.positionCount++;
            renderer.SetPosition(renderer.positionCount - 1, ppp);
        }
    }
    private void Set_Line(LineRenderer line)
    {
        line.startColor = lineColor;
        line.endColor = lineColor;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
    }

    void DeletLine()
    {
        //그림 그리기 버튼 (오른쪽 인덱스)을 안 누르고 있을때
        if (!OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            //오른손의 구형태로 충돌되는 콜리더가 있으면 
            Collider[] coll = Physics.OverlapSphere(rhand_R.transform.position, 0.1f);
            if (coll.Length > 0 && coll[0].gameObject.CompareTag("Line"))
            {
                print("지울수 있는 선있음");
                if(OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
                {
                    //라인 지워줌 Rpc로 전달~
                    photonView.RPC("Erase", RpcTarget.All,coll[0].GetComponent<LineInfo>().number);
                }
            }
        }
    }
    [PunRPC]
    void Erase(int lineindex)
    {
        GameObject lineobj = GameManager.instance.GetLine(lineindex);
        if (lineobj != null)  Destroy(lineobj);
    }


    void GrapLine()
    {
        if (obj == null)//잡은게 없음면 --잡기
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
            {
                Collider[] coll = Physics.OverlapSphere(lhand_L.transform.position, 0.1f);
                if(coll.Length>0&&coll[0].gameObject.CompareTag("Line"))
                {
                    print("선잡았따!");
                    photonView.RPC("Empower",RpcTarget.All, coll[0].gameObject.GetComponent<LineInfo>().number);
                }
            }
        }
    }

    void Graping()
    {
        // 해당 캐릭터가 잡은 아이가 있다면, 
        if (obj != null)//잡은게 있으면 --놓기
        {
            //자식된 아이가 Line이라면 
            if (obj.gameObject.CompareTag("Line"))
            {
                SendSpot();//부여된 빈오브젝트의 점리스트가 실시간 연동되게 만들어주기! 
            }

            if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
            {
                photonView.RPC("SetOff", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void Empower(int lineindex)
    {
        obj = GameManager.instance.GetLine(lineindex);
        //해당 위치에 모든 오브젝트들을 넣어준다. 
        LineRenderer ren = obj.GetComponent<LineRenderer>();
        for (int i = 0; i < ren.positionCount; i++)
        {
            if (e_obj.Count > 0)
            {
                //빈오브젝트 0번을 해달 라인렌더러에 위치 시켜주고,
                e_obj[0].transform.position = ren.GetPosition(i);
                u_obj.Add(e_obj[0]);//e0번을 u에 넣어주고 
                e_obj.RemoveAt(0);// e에서는 제거한다,
            }
            else
            {
                GameObject b = Instantiate(empty_obj);
                b.transform.SetParent(lhand_L.transform);
                b.transform.position = ren.GetPosition(i);
                u_obj.Add(b);
            }
        }
    }
    void SendSpot()
    {
        //빈오브젝트들의 위치를 담는 리스트
        List<Vector3> currline = new List<Vector3>();
        for (int i = 0; i < u_obj.Count; i++)
        {
            currline.Add(u_obj[i].transform.position);
        }
        //라인에 계속 넣어준다. 
        LineRenderer ren = obj.GetComponent<LineRenderer>();
        ren.positionCount = 0;
        ren.positionCount = currline.Count;
        for (int i = 0; i < currline.Count; i++)
        {
            ren.SetPosition(i, currline[i]);
        }
    }
    [PunRPC]
    void SetOff()
    {
        if (obj.gameObject.name.Contains("Line"))
        {
            e_obj.AddRange(u_obj);
            u_obj.Clear();

            //움직인 위치에 대해서 다른 사람들한ㄴ테도 알려줄 RPC
            //photonView.RPC("TellDrop",RpcTarget.All,obj, obj.GetComponent<LineRenderer>());

            LineRenderer ll = obj.GetComponent<LineRenderer>();
            MeshCollider ms = obj.GetComponent<MeshCollider>();
            Mesh mesh = new Mesh();
            ll.BakeMesh(mesh, true);
            ms.sharedMesh = mesh;
        }

        obj.transform.SetParent(null);
        obj = null;
    }
    //============================

    void SetKeybord()
    {
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            if (!keybord.activeSelf) keybord.SetActive(true);
            if (keybord.activeSelf) keybord.SetActive(false);
        }
    }

    void Typing()
    {
        if (keybord.activeSelf)
        {
            ////   
            if(Input.GetKeyDown(KeyCode.Alpha1))print(" 키누른다,");
        }        
    }
}
