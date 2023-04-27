using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace GameScene {
    public partial class CSubGameSceneManager : CGameSceneManager
    {
        [Header("★ [Reference] UI Top")]
        public List<RectTransform> starTransform;
        public List<GameObject> starOn;
        public RectTransform scoreGageFrame;
        public Image scoreGage;
        public TMP_Text scoreText;

        private float gageFrameX;
        private float gagePadding = 20f;

        public void InitUITop()
        {
            gageFrameX = scoreGageFrame.sizeDelta.x;
            for(int i=0; i < starTransform.Count; i++)
            {
                starTransform[i].anchoredPosition = new Vector2(Mathf.Clamp(((float)Engine.scoreList[i]/(float)Engine.scoreList[2]) * gageFrameX, gagePadding, gageFrameX - gagePadding), 0);
            }

            ScoreUpdate(false);
        }

        public void ScoreUpdate(bool _gageAni = true)
        {
            scoreText.text = string.Format(GlobalDefine.FORMAT_SCORE, Engine.currentScore);
            
            if (_gageAni && scoreGage.fillAmount < 1f)
                scoreGage.DOFillAmount((float)Engine.currentScore/(float)Engine.scoreList[2], GlobalDefine.SCORE_GAGE_DURATION);
            else
                scoreGage.fillAmount = ((float)Engine.currentScore/(float)Engine.scoreList[2]);

            Engine.starCount = 0;
            
            for(int i=0; i < starOn.Count; i++)
            {
                bool isActiveStar = Engine.currentScore >= Engine.scoreList[i];
                bool isGetStar = !starOn[i].activeInHierarchy && isActiveStar;

                starOn[i].SetActive(isActiveStar);
                Engine.starCount += starOn[i].activeInHierarchy ? 1 : 0;

                if (isGetStar)
                {
                    GlobalDefine.PlaySoundFX(ESoundSet.SOUND_GET_STAR);
                }
            }
        }

        public void OnTouchProfileBtn()
        {
            OpenTab(0);
        }

        public void OnTouchManualBtn()
        {
            Func.ShowTutorialPopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CTutorialPopup).Init();
			});
        }

        public void SetDarkMode(bool isDarkMode)
        {
            GlobalDefine.UserInfo.Settings_DarkMode = isDarkMode;
            GlobalDefine.SaveUserData();

            ApplyDarkMode(isDarkMode);
        }

        private void ApplyDarkMode(bool isDarkMode)
        {
            darkModeButton[0].SetActive(!isDarkMode);
            darkModeButton[1].SetActive(isDarkMode);
            darkModeBackground[0].SetActive(isDarkMode);
            darkModeBackground[1].SetActive(!isDarkMode);
            darkModeTopPanel[0].SetActive(isDarkMode);
            darkModeTopPanel[1].SetActive(!isDarkMode);
        }

        public void OnTouchClearBtn()
        {
            m_oEngine.LevelClear();
        }

		/** 정지 버튼을 눌렀을 경우 */
		public void OnTouchPauseBtn() {
            if (PopupUIs.transform.childCount == 0)
            {
                Func.ShowPausePopup(this.PopupUIs, (a_oSender) => {
                    (a_oSender as CPausePopup).Init(CPausePopup.MakeParams(new Dictionary<CPausePopup.ECallback, System.Action<CPausePopup>>() {
                        [CPausePopup.ECallback.RETRY] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.RETRY),
                        [CPausePopup.ECallback.LEAVE] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.LEAVE)
                    }, this.Engine));
                });
            }
		}

		/** 설정 버튼을 눌렀을 경웅 */
		public void OnTouchSettingsBtn() {
			Func.ShowSettingsPopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CSettingsPopup).Init();
			});
		}

		/** 광고 버튼을 눌렀을 경우 */
		private void OnTouchAdsBtn(ERewardAdsUIs a_eRewardAdsUIs) {
			m_oRewardAdsUIsDict.ExReplaceVal(EKey.SEL_REWARD_ADS_UIS, a_eRewardAdsUIs);

#if ADS_MODULE_ENABLE
			Func.ShowRewardAds(this.OnCloseRewardAds);
#endif // #if ADS_MODULE_ENABLE
		}
    }
}