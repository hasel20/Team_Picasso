using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDrawing : MonoBehaviourPun
{
    //�⺻ ��ü ���� 
    public GameObject rhand_R;
    public GameObject lhand_L;

    //Hold_Brush();�ʿ�.
    public GameObject brush; // �귯���� ó�� ���� �����տ� �־ ���� ����
    LineRenderer b_stick;
    public float brushLength = 0.1f;

    //Inventory_Active();  �ʿ� 
    public GameObject[] inventory; //���� â�� 
    int count;

    //���� �� ����. 
    Color lineColor = Color.gray;
    float lineWidth = 0.01f;
    //GameObject figure;  //Select_Figure()
    //public GameObject[] Figures;//1

    //DrawLine();�ʿ�
    public GameObject lineFactory;
    LineRenderer renderer;
    List<Vector3> points = new List<Vector3>();
    public int bezierCount = 10;

    //GrapLine();�ʿ�
    GameObject obj;
    //���� ������ ǥ�� �ϱ� ���� �������Ʈ ����Ʈ�� ��� ������Ʈ ����Ʈ
    List<GameObject> e_obj = new List<GameObject>();
    List<GameObject> u_obj = new List<GameObject>();
    public int p_counts = 1000;
    public GameObject empty_obj;



    void Start()
    {
        ///�����տ� ����ִ� ���� ���� �׸��� ���鲨��. 
        ///b_stick = rhand_R.GetComponent<LineRenderer>();
        ///b_stick.positionCount = 0; // ���ۿ��� 0���� ����� ! 
        //---------------------
        //���ڸ��� �־��ְ� �������Ʈ���� �޼տ� �־����
        for (int i = 0; i < p_counts; i++)
        {
            GameObject a = Instantiate(empty_obj);
            a.transform.SetParent(lhand_L.transform);
            e_obj.Add(a);
        }
        //------------------
        //=======�κ��丮 ��Ȱ��ȭ
        for (int i = 0; i < inventory.Length; i++)
        {
            inventory[i].SetActive(false);
        }
        inventory[0].SetActive(true);
    }

    void Update()
    {
    //    if (photonView.IsMine)
        {
            //�귯���� ���.
            Hold_Brush();
            //�ȷ�Ʈ���� �����ӿ��� �����ϱ� 
            Inventory_Active();
            //�׸��� �׸���. 
            DrawLine();
            //�׸��� �����.
            DeletLine();
            //�׸��� ��� �����̴�.
            GrapLine();
        }
    }

    void Hold_Brush()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            ///b_stick.SetWidth(0.005f, 0.005f);
            ///b_stick.SetColors(Color.white, Color.white);
                ///���ֺ��� ���̴� ����
            ///b_stick.positionCount = 2;
            ///b_stick.SetPosition(0, rhand_R.transform.position);
            ///b_stick.SetPosition(1, rhand_R.transform.position + rhand_R.transform.forward * brushLength);
            //���� �ν��� �������� 
            brush.SetActive(true);
            brush.transform.position = rhand_R.transform.position + rhand_R.transform.forward * brushLength;
            //brush.transform.SetParent(rhand_R.transform);
        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            b_stick.positionCount = 0;
            brush.SetActive(false);
        }

        //�귯���� �������� �� 
        if (brush)
        {
            Collider[] hits = Physics.OverlapSphere(brush.transform.position, 0.1f);
            if (hits.Length > 0)
            {
                Select_Palette(hits[0].gameObject);
                Select_Width(hits[0].gameObject);
                //Select_Figure(hits[0].gameObject); //��� ���� 
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

    /*    
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
    }*/

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
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)&& brush)
        {
            photonView.RPC("Start_Draw", RpcTarget.AllBuffered, brush.transform.position);
        }
        else if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) && brush)
        {
            photonView.RPC("Curr_Draw", RpcTarget.AllBuffered, brush.transform.position);
        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            photonView.RPC("End_Draw", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void Start_Draw(Vector3 popo)
    {
        GameObject line = Instantiate(lineFactory);
        renderer = line.GetComponent<LineRenderer>();
        Set_Line(renderer);//�׸� �� ���� �����س��� ������ ����

        points.Add(popo);
        renderer.positionCount = 1;
        renderer.SetPosition(0, points[0]);
    }
    [PunRPC]
    public void Curr_Draw(Vector3 popo)
    {
        //Vector3 pos = ;
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
        if (photonView.IsMine)
        {
            line.SetColors(lineColor, lineColor);
            line.SetWidth(lineWidth, lineWidth);
        }
    }

    void DeletLine()
    {
        if (!OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            Collider[] coll = Physics.OverlapSphere(rhand_R.transform.position, 0.1f);
            if (coll.Length > 0)
            {
                if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch) &&
                    coll[0].gameObject.layer != LayerMask.NameToLayer("Player"))
                {
                    //Destroy(coll[0].gameObject);
                    photonView.RPC("Erase",RpcTarget.AllBuffered,coll[0].gameObject);
                }
            }
        }
    }
    [PunRPC]
    void Erase(GameObject lineobj)
    {
        Destroy(lineobj);
    }


    void GrapLine()
    {
        if (obj == null)//������ ������ --���
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
            {
                Collider[] coll = Physics.OverlapSphere(lhand_L.transform.position, 0.1f);
                if (coll.Length > 0 && coll[0].gameObject.layer != LayerMask.NameToLayer("Player"))
                {
                    print("��ҵ�!");
                    obj = coll[0].gameObject;
                    //���� ���̰� �����̶�� ���� ������ �غ�������
                    if (coll[0].gameObject.CompareTag("Line"))
                    {                        
                        Empower();// ������ �������Ʈ�� �ο��ϴ� �Լ� 
                    }
                    else
                    {
                         coll[0].transform.SetParent(lhand_L.transform);
                    }
                }
            }
        }
        else if (obj != null)//������ ������ --����
        {
            //�ڽĵ� ���̰� Line�̶�� 
            if (obj.gameObject.CompareTag("Line"))
            {                
                SendSpot();//�ο��� �������Ʈ�� ������Ʈ�� �ǽð� �����ǰ� ������ֱ�! 
            }

            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
            {                
                SetOff();//���� �����̿����� u_obj�� e_obj�� �ٲ��ְ� �ٽ� ����ũ ���ֱ� 
                obj.transform.parent = null;
                obj = null;
            }
        }
    }

    void Empower()
    {
        //�ش� ��ġ�� ��� ������Ʈ���� �־��ش�. 
        LineRenderer ren = obj.GetComponent<LineRenderer>();
        for (int i = 0; i < ren.positionCount; i++)
        {
            if (e_obj.Count > 0)
            {
                //�������Ʈ 0���� �ش� ���η������� ��ġ �����ְ�,
                e_obj[0].transform.position = ren.GetPosition(i);
                u_obj.Add(e_obj[0]);//e0���� u�� �־��ְ� 
                e_obj.RemoveAt(0);// e������ �����Ѵ�,
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
        //�������Ʈ���� ��ġ�� ��� ����Ʈ
        List<Vector3> currline = new List<Vector3>();
        for (int i = 0; i < u_obj.Count; i++)
        {
            currline.Add(u_obj[i].transform.position);
        }
        //���ο� ��� �־��ش�. 
        LineRenderer ren = obj.GetComponent<LineRenderer>();
        ren.positionCount = 0;
        ren.positionCount = currline.Count;
        for (int i = 0; i < currline.Count; i++)
        {
            ren.SetPosition(i, currline[i]);
        }
    }

    void SetOff()
    {
        if (obj.gameObject.name.Contains("Line"))
        {
            e_obj.AddRange(u_obj);
            u_obj.Clear();

            //������ ��ġ�� ���ؼ� �ٸ� ������Ѥ��׵� �˷��� RPC
            //photonView.RPC("TellDrop",RpcTarget.AllBuffered,obj, obj.GetComponent<LineRenderer>());

            LineRenderer ll = obj.GetComponent<LineRenderer>();
            MeshCollider ms = obj.GetComponent<MeshCollider>();
            Mesh mesh = new Mesh();
            ll.BakeMesh(mesh, true);
            ms.sharedMesh = mesh;
        }
    }

    [PunRPC]
    void TellDrop(GameObject dropline,LineRenderer liren)
    {
        MeshCollider ms = dropline.GetComponent<MeshCollider>();
        Mesh mesh = new Mesh();
        liren.BakeMesh(mesh, true);
        ms.sharedMesh = mesh;
    }
}