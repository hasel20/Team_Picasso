using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerScore : MonoBehaviourPun
{
    public int score;
    public Text ScoreTx;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AddScore(int add)
    {
        score += add;

        ScoreTx.text = "현제 점수 : " + score.ToString();
    }
}
