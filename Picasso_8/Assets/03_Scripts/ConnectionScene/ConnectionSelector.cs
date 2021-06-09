using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConnectionSelector : MonoBehaviour
{
    public GameObject Connection;
    public GameObject OffLine;
    public GameObject Resistry;

    private void Start()
    {
        OffLine.SetActive(false);
        Connection.SetActive(true);
    }

    private void Update()
    {
        
    }

    public void As_PlaySandbox()
    {
        //if (Resistry != null) Resistry.SetActive(false);
        Connection.SetActive(false);
        OffLine.SetActive(true);
    }
    public void As_PlayGuest()
    {
        //if (Resistry != null) Resistry.SetActive(false);
        OffLine.SetActive(false);
        Connection.SetActive(true);
    }
    public void As_Resistry()
    {
        OffLine.SetActive(false);
        Connection.SetActive(false);
        //if (Resistry != null) Resistry.SetActive(true);
    }
}
