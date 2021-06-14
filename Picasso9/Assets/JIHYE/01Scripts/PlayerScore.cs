using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerScore : MonoBehaviourPun
{
    public int score;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AddScore(int add)
    {
        score += add;
    }
}
