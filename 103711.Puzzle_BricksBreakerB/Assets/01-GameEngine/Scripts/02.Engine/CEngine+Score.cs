using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NSEngine {
    public partial class CEngine : CComponent
    {
        [Header("★ [Live] Score")]
        public List<int> scoreList = new List<int>();
        public int currentCombo;
        public int currentScore;

        private void InitScoreList(int score1, int score2, int score3)
        {
            scoreList.Clear();
            scoreList.Add(score1);
            scoreList.Add(score2);
            scoreList.Add(score3);

            scoreList = scoreList.OrderBy(g => g).ToList();
        }

        public void InitCombo()
        {
            currentCombo = 0;
        }

        public void InitScore()
        {
            currentScore = 0;
        }

        public void GetScore()
        {
            currentCombo = Mathf.Min(currentCombo + 1, int.MaxValue);
            currentScore = Mathf.Min(currentScore + GetComboScore(), int.MaxValue);

            subGameSceneManager.ScoreUpdate();
            
            //Debug.Log(CodeManager.GetMethodName() + GetComboScore());
        }

        private int GetComboScore()
        {
            return GlobalDefine.SCORE_DEFAULT + (GlobalDefine.SCORE_COMBO_BONUS * (currentCombo - 1));
        }
    }
}