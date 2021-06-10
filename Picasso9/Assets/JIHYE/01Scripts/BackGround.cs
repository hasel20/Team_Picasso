using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    // �ӷ�
    public float speed = 0.5f;
    void Start()
    {

    }

    void Update()
    {
        //������ �ӵ��� ����� ��ũ�� �ϰ� �ʹ�.
        // 1. MeshRenderer ������Ʈ�� ��������
        MeshRenderer mr = GetComponent<MeshRenderer>();
        // 2. Material ��������
        Material mat = mr.material;
        // 3. offset y ���� �����ؼ� ��ũ�� �ǰ� ����.
        mat.mainTextureOffset -= Vector2.up * speed * Time.deltaTime;
    }
}
