using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomString : MonoBehaviour
{
    public Text Que;
    Text Random;

    private void Start()
    {
        //�迭 ����
        string[] RandomString = new string[] {
            "õ��", "����", "���", "����", "�ٳ���", "�ٱ���",
            "��", "����", "�غ�", "��", "ħ��", "�ܹ�", "��Ʈ", 
            "��ȸ", "������", "��", "����ũ", "��", "��", "�絿��",
            "����", "ī�޶�", "ķ�����̾�", "����", "����", "��", 
            "���", "�����", "����", "����", "��", "�ð�", "����", 
            "��ħ��", "��ǻ��", "��Ű", "��", "��", "�հ�", "��", 
            "���̾Ƹ��", "����", "��", "�����", "��", "����", "����",
            "����", "�û���", "��", "����Ʈ ��", "���ڼ�", "�Ҵ�", 
            "����", "���ϻ�", "�޹���", "����", "�̾�Ĺ", "�ǾƳ�",
            "����", "����", "����", "���ξ���", "����", "������"};
        System.Random random = new System.Random();
        int deg = random.Next(RandomString.Length);
        string pick = RandomString[deg];
        Que.text = (pick);
    }
   public void Randomize()
    {
        string[] RandomString = new string[] {
            "õ��", "����", "���", "����", "�ٳ���", "�ٱ���",
            "��", "����", "�غ�", "��", "ħ��", "�ܹ�", "��Ʈ",
            "��ȸ", "������", "��", "����ũ", "��", "��", "�絿��",
            "����", "ī�޶�", "ķ�����̾�", "����", "����", "��",
            "���", "�����", "����", "����", "��", "�ð�", "����",
            "��ħ��", "��ǻ��", "��Ű", "��", "��", "�հ�", "��",
            "���̾Ƹ��", "����", "��", "�����", "��", "����", "����",
            "����", "�û���", "��", "����Ʈ ��", "���ڼ�", "�Ҵ�",
            "����", "���ϻ�", "�޹���", "����", "�̾�Ĺ", "�ǾƳ�",
            "����", "����", "����", "���ξ���", "����", "������"};
        System.Random random = new System.Random();
        int deg = random.Next(RandomString.Length);
        string pick = RandomString[deg];
        Que.text = (pick);
    }
}
