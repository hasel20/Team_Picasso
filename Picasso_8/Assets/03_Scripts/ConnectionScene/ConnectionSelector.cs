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
    public GameObject SingleGame;

    private void Start()
    {
        if (Connection != null)
            Connection.SetActive(true);
        if (OffLine != null)
            OffLine.SetActive(false);
        if (Resistry != null)
            Resistry.SetActive(false);
        if (SingleGame != null)
            SingleGame.SetActive(false);
    }
    public void As_PlaySandbox()
    {
        if (Resistry != null) Resistry.SetActive(false);
        if (Connection != null) Connection.SetActive(false);
        if (SingleGame != null) SingleGame.SetActive(false);
        if (OffLine == null) return;
        else OffLine.SetActive(true); 
    }
    public void As_PlayGuest()
    {
        if (Resistry != null) Resistry.SetActive(false);
        if (OffLine != null) OffLine.SetActive(false);
        if (SingleGame != null) SingleGame.SetActive(false);
        if (Connection == null) return;
        else Connection.SetActive(true); 
    }
    public void As_Resistry()
    {
        if (Connection != null) Connection.SetActive(false);
        if (OffLine != null) OffLine.SetActive(false);
        if (SingleGame != null) SingleGame.SetActive(false);
        if (Resistry == null) return;
        else Resistry.SetActive(true);
    }
    public void As_SingleGame()
    {
        if (Connection != null) Connection.SetActive(false);
        if (OffLine != null) OffLine.SetActive(false);
        if (Resistry != null) Resistry.SetActive(false);
        if (SingleGame == null) return;
        else SingleGame.SetActive(true); 
    }
}
