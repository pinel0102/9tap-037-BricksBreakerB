using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
    public partial class CEngine : CComponent
    {
        [Header("★ [Parameter] Score")]
        public List<int> scoreList = new List<int>();
        public int currentCombo;
        public int currentScore;
        public int starCount;

        private void InitScoreList(CLevelInfo levelInfo)
        {
            scoreList = GlobalDefine.GetLevelScoreList(levelInfo);
        }

        public void InitCombo()
        {
            currentCombo = 0;
        }

        public void InitScore()
        {
            currentScore = 0;
            starCount = 0;
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