using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Text : MonoBehaviour
{
    public GameObject canvas;
    Text how_to;
    AudioSource audios;
    public GameObject butten;
    public GameObject mainbtn;

    int count;

    public List<AudioClip> narration = new List<AudioClip>();

    void Start()
    {
        how_to = canvas.GetComponentInChildren<Text>();
        audios = canvas.GetComponent<AudioSource>();
        butten.GetComponentInChildren<Text>().text = "Start!";
        mainbtn.SetActive(false);
    }

    IEnumerator How_to_Drawing()
    {
        audios.PlayOneShot(narration[0]);
        how_to.text = "Do you want Drawing?";
        yield return new WaitForSeconds(1.5f);
        how_to.text = "Press on the index Butten," + "\r\n" + "on the right!";
        yield return new WaitForSeconds(4);
        butten.SetActive(true);
    }
    IEnumerator How_to_Moving()
    {
        audios.PlayOneShot(narration[1]);
        how_to.text = "Do you want to move?";
        yield return new WaitForSeconds(1.5f);
        how_to.text = "Use the Left Joystick!";
        yield return new WaitForSeconds(3);
        butten.SetActive(true);

    }
    IEnumerator Change_Pallet()
    {
        audios.PlayOneShot(narration[2]);
        how_to.text = "How to change the palette.";
        yield return new WaitForSeconds(1.8f);
        how_to.text = "Press the [X] button" + "\r\n" + "on the left and it changes!";
        yield return new WaitForSeconds(4);
        how_to.text = "and if you want to" + "\r\n" + "turn off pallet";
        yield return new WaitForSeconds(3);
        how_to.text = "Press the [Y] button" + "\r\n" + "on the left";
        yield return new WaitForSeconds(4);
        butten.SetActive(true);

    }
    IEnumerator How_to_Change_Color()
    {
        audios.PlayOneShot(narration[3]);
        how_to.text = "If you want to" + "\r\n" + "Change the color";
        yield return new WaitForSeconds(2);
        how_to.text = "Shoot Ray on the Pallet";
        yield return new WaitForSeconds(3);
        butten.SetActive(true);

    }
    IEnumerator How_to_Change_Thick()
    {
        audios.PlayOneShot(narration[4]);
        how_to.text = "If you want to" + "\r\n" + "change the thickness,";
        yield return new WaitForSeconds(2.5f);
        how_to.text = "Shoot Ray on the" + "\r\n" + "thickness board";
        yield return new WaitForSeconds(1.5f);
        how_to.text = "Shoot Ray on the thickness";
        yield return new WaitForSeconds(3);
        butten.SetActive(true);

    }
    IEnumerator How_to_Instante_OBJ()
    {
        audios.PlayOneShot(narration[5]);
        how_to.text = "Do you want to" + "\r\n" + "make shapes?";
        yield return new WaitForSeconds(2.5f);
        how_to.text = "Press on the [A]button" + "\r\n" + "on the right";
        yield return new WaitForSeconds(3);
        how_to.text = "And, If you want to" + "\r\n" + "change the shape";
        yield return new WaitForSeconds(3);
        how_to.text = "Shoot Ray on the shapes";
        yield return new WaitForSeconds(4);;
        butten.SetActive(true);
    }
    IEnumerator How_to_Delet_OBJ()
    {
        audios.PlayOneShot(narration[6]);
        how_to.text = "Somethings you want to erase?";
        yield return new WaitForSeconds(1.9f);
        how_to.text = "Touch what you want to erase.";
        yield return new WaitForSeconds(2);
        how_to.text = "And, Press on the [B]button" + "\r\n" + "on the right";
        yield return new WaitForSeconds(4);

        butten.SetActive(true);
    }
  
    void Change_Tutorial()
    {
        switch (count%8)
        {
            case 0:
                StartCoroutine(How_to_Drawing());
                break;
            case 1:
                StartCoroutine(How_to_Moving());
                break;
            case 2:
                StartCoroutine(Change_Pallet());
                break;
            case 3:
                StartCoroutine(How_to_Change_Color());
                break;
            case 4:
                StartCoroutine(How_to_Change_Thick());
                break;
            case 5:
                StartCoroutine(How_to_Instante_OBJ());
                break;
            case 6:
                StartCoroutine(How_to_Delet_OBJ());
                break;
            case 7:
                how_to.gameObject.SetActive(false);
                mainbtn.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void TU_ONCLICK()
    {
        Change_Tutorial();
        butten.GetComponentInChildren<Text>().text = "next";
        count++;
        butten.SetActive(false);
    }
    public void TU_ON_Skip()
    {
        count = 7;
        Change_Tutorial();
    }
    public void TU_ON_Main_Scene()
    {
        mainbtn.GetComponentInChildren<Text>().text = "메인으로...";
    }

}
