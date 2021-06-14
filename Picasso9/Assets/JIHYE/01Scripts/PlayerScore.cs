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

    public int round_end_score = 15;
    void Start()
    {
        
    }

    void Update()
    {
        if (score >= round_end_score)
        {
            GameManager.instance.RoundEnd(GetComponent<PhotonView>().ViewID);
        }
    }

    public void AddScore(int add)
    {
        score += add;

        ScoreTx.text = "현제 점수 : " + score.ToString();
    }
}
