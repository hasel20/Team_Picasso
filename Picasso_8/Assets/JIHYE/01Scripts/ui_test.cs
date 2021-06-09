using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_test : MonoBehaviour
{
    public GameObject ink;
    public GameObject slider;

    public List<GameObject> canvas = new List<GameObject>();
    float xx;
    int count;
    Vector3 originScale;
    Color cc;
    void Start()
    {
        originScale = ink.transform.localScale;
    }

    void Update()
    {
        Setting();

        Change_Menu();
    }
    void Setting()
    {
        ink.transform.localScale = originScale * xx;
        ink.GetComponent<MeshRenderer>().material.color = cc;
    }
    void Change_Menu()
    {        
        if (Input.GetKeyDown(KeyCode.A))
        {
            count++;
            for (int i = 0; i < canvas.Count; i++)
            {
                canvas[i].SetActive(false);
            }
            canvas[count % canvas.Count].SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            count = 0;
        }
    }





    public void Change_Value()
    {
        Slider sl =slider.GetComponent<Slider>();
        //ink.transform.localScale = new Vector3( sl.value, sl.value, sl.value);
        xx =  sl.value;
    }
}
