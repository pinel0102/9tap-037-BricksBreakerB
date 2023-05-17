using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 크기 보정자 */
public partial class CSizeCorrector : CComponent {
	#region 변수
	[SerializeField] private Vector3 m_stSizeRate = Vector3.one;
    private Vector3 canvasArea = Vector3.zero;
    private float screenHeightDevice;
    private float safeHeightDevice;
    private float notchSizeTopDevice;
    private float notchSizeBottomDevice;
    private float screenHeightCanvas;
    private float safeHeightCanvas;
    private float notchSizeTopCanvas;
    private float notchSizeBottomCanvas;
    private float positionOffset;
	#endregion // 변수

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		CScheduleManager.Inst.AddComponent(this);
	}

	/** 초기화 */
	public override void Start() {
		base.Start();

        GetNotchSize_Device();
        GetNotchSize_Canvas((float)KCDefine.B_PORTRAIT_SCREEN_HEIGHT);
        SetCanvasArea();

        this.SetSizeRate(m_stSizeRate);
	}

    private void GetNotchSize_Device()
    {
        var safeRect = Screen.safeArea;
        
        // [iPhone X] (1125 x 2436)
        // Screen.safeArea = Rect (0, 102, 1125, 2202)
        
		screenHeightDevice = Screen.height; // [iPhone X] 2436
        safeHeightDevice = safeRect.height; // [iPhone X] 2202
        notchSizeTopDevice = screenHeightDevice - (safeRect.y + safeRect.height); // [iPhone X] 132
        notchSizeBottomDevice = safeRect.y; // [iPhone X] 102
    }

    private void GetNotchSize_Canvas(float canvasHeight)
    {
        float ratio = (canvasHeight / screenHeightDevice);

        screenHeightCanvas = screenHeightDevice * ratio;
        safeHeightCanvas = safeHeightDevice * ratio;
        notchSizeTopCanvas = notchSizeTopDevice * ratio;
        notchSizeBottomCanvas = notchSizeBottomDevice * ratio;
    }

    private void SetCanvasArea()
    {
        canvasArea = new Vector3(CSceneManager.CanvasSize.x, CSceneManager.CanvasSize.y - (notchSizeTopCanvas + notchSizeBottomCanvas), 0);
        
        //positionOffset = (notchSizeTopCanvas - notchSizeBottomCanvas) * 0.5f;
        //this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y - positionOffset, this.transform.localPosition.z);

        //CFunc.ShowLog(string.Format("canvasArea : {0} / positionOffset : {1}", canvasArea, positionOffset));
    }
    
#if !UNITY_STANDALONE && !UNITY_STANDALONE_OSX
	/** 상태를 갱신한다 */
	public override void OnLateUpdate(float a_fDeltaTime) {
		base.OnLateUpdate(a_fDeltaTime);

		// 앱이 실행 중 일 경우
		if(CSceneManager.IsAppRunning) {
			var stSize = canvasArea.ExGetScaleVec(m_stSizeRate);
			(this.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, stSize.x);
			(this.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, stSize.y);
		}
	}
#endif

	/** 크기 비율을 변경한다 */
	public void SetSizeRate(Vector3 a_stRate) {
		m_stSizeRate.x = Mathf.Clamp01(a_stRate.x);
		m_stSizeRate.y = Mathf.Clamp01(a_stRate.y);
		m_stSizeRate.z = Mathf.Clamp01(a_stRate.z);
	}
	#endregion // 함수
}
