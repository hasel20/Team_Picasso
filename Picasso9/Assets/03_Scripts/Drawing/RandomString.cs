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
        //배열 랜덤
        string[] RandomString = new string[] {
            "천사", "개미", "사과", "백팩", "바나나", "바구니",
            "공", "박쥐", "해변", "곰", "침대", "꿀벌", "벨트", 
            "교회", "자전거", "새", "케이크", "뇌", "빵", "양동이",
            "버스", "카메라", "캠프파이어", "양초", "대포", "차", 
            "당근", "고양이", "의자", "성당", "원", "시계", "구름", 
            "나침반", "컴퓨터", "쿠키", "소", "게", "왕관", "컵", 
            "다이아몬드", "버섯", "못", "목걸이", "코", "문어", "양파",
            "오븐", "올빼미", "붓", "페인트 통", "야자수", "팬더", 
            "바지", "낙하산", "앵무새", "땅콩", "미어캣", "피아노",
            "액자", "돼지", "베개", "파인애플", "피자", "경찰차"};
        System.Random random = new System.Random();
        int deg = random.Next(RandomString.Length);
        string pick = RandomString[deg];
        Que.text = (pick);
    }
   public void Randomize()
    {
        string[] RandomString = new string[] {
            "천사", "개미", "사과", "백팩", "바나나", "바구니",
            "공", "박쥐", "해변", "곰", "침대", "꿀벌", "벨트",
            "교회", "자전거", "새", "케이크", "뇌", "빵", "양동이",
            "버스", "카메라", "캠프파이어", "양초", "대포", "차",
            "당근", "고양이", "의자", "성당", "원", "시계", "구름",
            "나침반", "컴퓨터", "쿠키", "소", "게", "왕관", "컵",
            "다이아몬드", "버섯", "못", "목걸이", "코", "문어", "양파",
            "오븐", "올빼미", "붓", "페인트 통", "야자수", "팬더",
            "바지", "낙하산", "앵무새", "땅콩", "미어캣", "피아노",
            "액자", "돼지", "베개", "파인애플", "피자", "경찰차"};
        System.Random random = new System.Random();
        int deg = random.Next(RandomString.Length);
        string pick = RandomString[deg];
        Que.text = (pick);
    }
}
