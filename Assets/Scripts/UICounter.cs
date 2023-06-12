using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class UICounter : MonoBehaviour
{

    public int initialSecs;

    public TextMeshProUGUI gameOverScoreText;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;

    public UnityEvent GameOver;

    float miliSecCounter;
    int secondCounter;
    int score;//ammount of orders delivers

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
        public void AddScore(){
            score++;
            scoreText.SetText(score.ToString());
        }
        public void SubstractSecs(int substract){
            secondCounter-=substract;
        }
        public void SetScore(){
            gameOverScoreText.SetText(score.ToString());
        }            
}
