using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class UICounter : MonoBehaviour
{
    public static UICounter intance;

    public LayerMask wallMask;

    public int initialSecs;

    public TextMeshProUGUI gameOverScoreText;

    public TextMeshProUGUI timerText;

    public UnityEvent GameOver;

    float miliSecCounter;
    int secondCounter;

    public TextMeshProUGUI scoreText1;
    public TextMeshProUGUI scoreText2;

    int score1;//ammount of orders delivers Player1

    int score2;//ammount of orders delivers Player2

    private void Awake()
    {
        if (intance == null)
            intance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start() {
        secondCounter=initialSecs;
    }
        private void Update() {

            if(secondCounter<=0){
                GameOver.Invoke();
                return;
            }

            if(miliSecCounter<=0){
                miliSecCounter=99;
                secondCounter--;
            }
            miliSecCounter-=Time.deltaTime*99;
            float miliSecRound = Mathf.Round(miliSecCounter);
            timerText.SetText(secondCounter+" : "+
                (miliSecRound<10?"0"+miliSecRound.ToString():miliSecRound.ToString()));
        }
        public void AddSecs(int add){
            secondCounter+=add;
        }
    public void AddScore(Player player = 0)
    {
        switch (player)
        {
            case Player.One:
                score1++;
                scoreText1.SetText(score1.ToString());
                break;
            case Player.Two:
                score2++;
                scoreText2.SetText(score2.ToString());
                break;
            default:
                break;
        }
    }
    public void StealScore(Player thief = 0)
    {
        switch (thief)
        {
            case Player.One:
                if (score2 <= 0) return;
                score1++;
                score2--;
                scoreText1.SetText(score1.ToString());
                scoreText2.SetText(score2.ToString());
                break;
            case Player.Two:
                if (score1 <= 0) return;
                score1--;
                score2++;
                scoreText1.SetText(score1.ToString());
                scoreText2.SetText(score2.ToString());
                break;
            default:
                break;
        }
    }
    public void SubstractSecs(int substract)
    {
        secondCounter -= substract;
    }
    public void SetScore()
    {
        gameOverScoreText.SetText(score1.ToString());
    }
}
public enum Player
{
    One,
    Two
}
