//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class JH_Drowing : MonoBehaviour
//{
//    public float speed = 10;
//    public GameObject cameraRig;
//    public Transform rHand;
//    public Transform lHand;
//    //public Transform mouse;
//    public GameObject lineFactory;
//    LineRenderer lr;

//    Color lineColor;
//    float width;
//    GameObject figure;
//    GameObject drop;
//    List<Vector3> points = new List<Vector3>();

//    public GameObject finger;
//    //--------------

//    public GameObject[] inventory;

//    public GameObject volum;

//    public List<GameObject> Figures = new List<GameObject>();


//    void Start()
//    {
//        //=======인벤토리 비활성화
//        for (int i = 0; i < inventory.Length; i++)
//        {
//            inventory[i].SetActive(false);
//        }
//        inventory[3].SetActive(true);
//        //--------Line기본 색과 굵기 정의
//        lineColor = lineFactory.GetComponent<LineRenderer>().startColor;
//        width = lineFactory.GetComponent<LineRenderer>().startWidth;

//        figure = Figures[0];
//    }


//    void Update()
//    {
//        Wand();

//        MY_Moving();

//        //Select_Palette();
//        //Shoot_Laser();
//        Inventory_Active();
//        Drow();
//        Drop_Figure();
//        // Grab_OBJ();
//    }

//    void MY_Moving()
//    {
//        //if(photonview.IsMine)
//        {
//            Vector2 pedL = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
//            Vector3 dir = new Vector3(pedL.x, 0, pedL.y);

//            dir = cameraRig.transform.TransformDirection(dir);

//            transform.Translate(dir * speed * Time.deltaTime, Space.Self);
//        }
//    }

//    void Drow()
//    {
//        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) ||
//            Input.GetMouseButtonDown(0))
//        {
//            GameObject line = Instantiate(lineFactory);
//            lr = line.GetComponent<LineRenderer>();
//            Set_Line(lr);

//            points.Add(rHand.position);
//            lr.positionCount = 1;
//            lr.SetPosition(0, points[0]);
//        }
//        else if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) ||
//            Input.GetMouseButton(0))
//        {
//            Vector3 pos = rHand.position;
//            if (Vector3.Distance(points[points.Count - 1], pos) > 0.01f)
//            {
//                points.Add(pos);
//                lr.positionCount++;
//                lr.SetPosition(points.Count - 1, pos);
//            }
//        }
//        else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) ||
//            Input.GetMouseButtonUp(0))
//        {
//            MeshCollider ms = lr.GetComponent<MeshCollider>();
//            Mesh mesh = new Mesh();
//            lr.BakeMesh(mesh, true);
//            ms.sharedMesh = mesh;
//            points.Clear();
//            lr = null;
//        }
//    }

//    int count = -1;

//    void Inventory_Active()
//    {

//        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
//        {
//            for (int i = 0; i < inventory.Length; i++)
//            {
//                inventory[i].SetActive(false);
//            }
//            count++;
//            inventory[count % inventory.Length].SetActive(true);
//        }


//        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
//        {
//            for (int i = 0; i < inventory.Length; i++)
//            {
//                inventory[i].SetActive(false);
//            }
//            inventory[3].SetActive(true);
//            count = -1;
//        }
//    }

//    LineRenderer liner;
//    void Shoot_Laser()
//    {
//        Ray ray = new Ray(rHand.position, rHand.position + rHand.forward);
//        RaycastHit hit;
//        //선택 오른손에서 레이를 쏴서 빛을 본다. 
//        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
//        {
//            liner = Instantiate(lineFactory).GetComponent<LineRenderer>();
//            liner.SetWidth(0.005f, 0.005f);
//            liner.SetColors(Color.red, Color.red);
//        }
//        else if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
//        {
//            liner.positionCount = 2;

//            //Line_Size();

//            if (Physics.Raycast(ray, out hit))
//            {
//                liner.SetPosition(0, rHand.position);
//                liner.SetPosition(1, hit.point);

//                Select_Palette(hit.transform.gameObject);
//                //Control_Slider(hit.transform.gameObject, hit.point);
//                Select_Width(hit.transform.gameObject);
//                Select_Figure(hit.transform.gameObject);

//            }
//            else
//            {
//                liner.SetPosition(0, rHand.position);
//                liner.SetPosition(1, rHand.position + rHand.forward);
//            }
//        }
//        else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
//        {
//            Destroy(liner.gameObject);
//            liner = null;
//        }
//    }


//    void Select_Width(GameObject hit)
//    {
//        if (hit.CompareTag("Slider"))
//        {
//            switch (hit.name)
//            {
//                case "Large":
//                    width = 0.05f;
//                    break;
//                case "Medium":
//                    width = 0.03f;
//                    break;
//                case "Small":
//                    width = 0.01f;
//                    break;
//                default:
//                    break;
//            }
//        }


//    }
//    void Select_Palette(GameObject hit)
//    {
//        if (hit.CompareTag("Palette"))
//        {
//            //go.name
//            switch (hit.name)
//            {
//                case "RED":
//                    lineColor = Color.red;
//                    break;
//                case "YELLOW":
//                    lineColor = Color.yellow;
//                    break;
//                case "BLUE":
//                    lineColor = Color.blue;
//                    break;
//                case "GREEN":
//                    lineColor = Color.green;
//                    break;
//                case "BLACK":
//                    lineColor = Color.black;
//                    break;
//                case "WHITE":
//                    lineColor = Color.white;
//                    break;
//                default:
//                    break;
//            }
//        }
//    }
//    public
//    void Select_Figure(GameObject hit)
//    {
//        if (hit.CompareTag("Figure"))
//        {
//            //figure = hit;
//            print(hit.name);
//            //나중에는 각각의 게임 오브젝트들을 Public으로 받아서 누르면 해당 오브젝트가 되게 만들어 줄꺼 다아 그럴라구 
//            switch (hit.name)
//            {
//                case "Cube":
//                    figure = Figures[0];
//                    print("정육면체");
//                    break;
//                case "Sphere":
//                    figure = Figures[1];
//                    print("구");
//                    break;
//                case "Capsule":
//                    figure = Figures[2];
//                    print("알약");
//                    break;
//                case "Cylinder":
//                    figure = Figures[3];
//                    print("원기둥");
//                    break;
//                default:
//                    break;
//            }
//        }
//    }



//    void Drop_Figure()
//    {
//        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
//        {
//            drop = Instantiate(figure);
//            drop.transform.SetParent(rHand);
//            drop.transform.localPosition = Vector3.forward;
//        }
//        else if (OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.RTouch))
//        {
//            drop.transform.parent = null;
//        }
//    }

//    void Grab_OBJ()
//    {
//        Ray ray = new Ray(lHand.position, lHand.forward);
//        RaycastHit hit;
//        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch) &&
//            !OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
//        {
//            liner = Instantiate(lineFactory).GetComponent<LineRenderer>();
//            liner.SetWidth(0.005f, 0.005f);
//            liner.SetColors(Color.blue, Color.blue);
//        }
//        else if (OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.LTouch) &&
//            !OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
//        {
//            liner.positionCount = 2;
//            liner.SetPosition(0, lHand.position);
//            if (Physics.Raycast(ray, out hit)) { liner.SetPosition(1, hit.point); }
//            else { liner.SetPosition(1, lHand.position + lHand.forward * 10); }
//        }
//        else if (OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.LTouch) &&
//           !OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
//        {
//            Destroy(liner.transform.gameObject);
//        }

//        if (Physics.SphereCast(ray, 0.5f, out hit, 100))
//        {
//            hit.transform.SetParent(lHand);
//            //Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
//            //rb.isKinematic = true;
//        }
//    }


//    void Line_Size()
//    {
//        ////레이저 버튼 누르면서 
//        ////오른쪽 스틱 조정하면 
//        //if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch))
//        //{
//        //    Vector2 stickR = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
//        //    if (stickR.x > 0.1f || stickR.x < -0.1f)
//        //    {
//        //        //x 값에 따라 크기가 변하는 캔버스 이미지. 크기 조정 됨
//        //        dot.SetActive(true);
//        //        dot.transform.localScale *= stickR.x;
//        //    }
//        //    else if (stickR.x < 0.1f && stickR.x > -0.1f)
//        //    {
//        //        dot.SetActive(false);
//        //        width *= stickR.x;
//        //    }
//        //    else
//        //    {
//        //        dot.SetActive(false);
//        //    }
//        //}
//    }

//    void Control_Slider(GameObject hit, Vector3 pos)
//    {
//        if (hit.CompareTag("Slider"))
//        {
//            hit.transform.position = pos;
//        }
//    }


//    void Set_Line(LineRenderer line)
//    {
//        line.SetColors(lineColor, lineColor);
//        line.SetWidth(width, width);
//    }

//    public void Change_LineSize()
//    {
//        Slider sl = volum.GetComponent<Slider>();
//        width *= sl.value;
//    }

//    //=====================================
//    void Wand()
//    {
//        //Ray ray = new Ray(rhand.position, rhand.forward);
//        //선택 오른손에서 레이를 쏴서 빛을 본다. 
//        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
//        {
//            liner = Instantiate(lineFactory).GetComponent<LineRenderer>();
//            liner.SetWidth(0.005f, 0.005f);
//            liner.SetColors(Color.white, Color.white);
//            //지휘봉의 길이는 일정
//            liner.positionCount = 2;
//            liner.SetPosition(0, rHand.position);
//            liner.SetPosition(1, rHand.position + rHand.forward * 0.1f);
//            //접촉 인식할 포인터의 
//            finger.SetActive(true);
//            finger.transform.position = rHand.position + rHand.forward * 0.1f;

//            liner.transform.SetParent(rHand);
//            finger.transform.SetParent(rHand);

//        }
//        else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
//        {
//            Destroy(liner.gameObject);
//            liner = null;
//            finger.SetActive(false);
//        }
//        //============//포인터가 홧성화 되었을떄 닿는 물채애 따라 고를 수있게에 
//        //if (point&&photonView.IsMine)
//        if (finger)
//        {
//            Collider[] hits = Physics.OverlapSphere(finger.transform.position, 0.01f);
//            if (hits.Length > 0)
//            {
//                Select_Palette(hits[0].gameObject);
//                Select_Width(hits[0].gameObject);
//                Select_Figure(hits[0].gameObject);
//            }
//        }

//    }
//}
