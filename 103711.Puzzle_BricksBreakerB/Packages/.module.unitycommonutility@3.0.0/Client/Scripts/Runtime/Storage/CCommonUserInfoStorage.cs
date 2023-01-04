using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using MessagePack;

/** 공용 유저 정보 */
[MessagePackObject]
[System.Serializable]
public partial class CCommonUserInfo : CCommonBaseInfo {
	#region 변수
	[Key(81)] public List<string> m_oRestoreProductIDList = new List<string>();
	#endregion // 변수

	#region 상수
	private const string KEY_USER_TYPE = "UserType";
	#endregion // 상수

	#region 프로퍼티
	[IgnoreMember]
	public EUserType UserType {
		get { return (EUserType)int.Parse(m_oStrDict.GetValueOrDefault(KEY_USER_TYPE, $"{(int)EUserType.NONE}")); }
		set { m_oStrDict.ExReplaceVal(KEY_USER_TYPE, $"{(int)value}"); }
	}
	#endregion // 프로퍼티

	#region IMessagePackSerializationCallbackReceiver
	/** 직렬화 될 경우 */
	public override void OnBeforeSerialize() {
		base.OnBeforeSerialize();
	}

	/** 역직렬화 되었을 경우 */
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();
		m_oRestoreProductIDList = m_oRestoreProductIDList ?? new List<string>();

		// 버전이 다를 경우
		if(this.Ver.CompareTo(KCDefine.U_VER_COMMON_USER_INFO) < KCDefine.B_COMPARE_EQUALS) {
			// Do Something
		}
	}
	#endregion // IMessagePackSerializationCallbackReceiver

	#region 함수
	/** 생성자 */
	public CCommonUserInfo() : base(KCDefine.U_VER_COMMON_USER_INFO) {
		// Do Something
	}
	#endregion // 함수
}

/** 공용 유저 정보 저장소 */
public partial class CCommonUserInfoStorage : CSingleton<CCommonUserInfoStorage> {
	#region 프로퍼티
	public CCommonUserInfo UserInfo { get; private set; } = new CCommonUserInfo();
	#endregion // 프로퍼티

	#region 함수
	/** 유저 정보를 리셋한다 */
	public virtual void ResetUserInfo(string a_oJSONStr) {
		CFunc.ShowLog($"CCommonUserInfoStorage.ResetUserInfo: {a_oJSONStr}");
		CAccess.Assert(a_oJSONStr.ExIsValid());

		this.UserInfo = a_oJSONStr.ExMsgPackBase64StrToObj<CCommonUserInfo>();
		CAccess.Assert(this.UserInfo != null);
	}

	/** 비소모 상품 복원 여부를 검사한다 */
	public bool IsRestoreProduct(string a_oProductID) {
		CAccess.Assert(a_oProductID.ExIsValid());
		return this.UserInfo.m_oRestoreProductIDList.Contains(a_oProductID);
	}

	/** 비소모 상품 식별자를 추가한다 */
	public void AddRestoreProductID(string a_oProductID) {
		CAccess.Assert(a_oProductID.ExIsValid());
		this.UserInfo.m_oRestoreProductIDList.ExAddVal(a_oProductID);
	}

	/** 유저 정보를 로드한다 */
	public CCommonUserInfo LoadUserInfo() {
		return this.LoadUserInfo(KCDefine.U_DATA_P_COMMON_USER_INFO);
	}

	/** 유저 정보를 로드한다 */
	public CCommonUserInfo LoadUserInfo(string a_oFilePath) {
		// 파일이 존재 할 경우
		if(File.Exists(a_oFilePath)) {
			this.UserInfo = CFunc.ReadMsgPackObj<CCommonUserInfo>(a_oFilePath, true);
			CAccess.Assert(this.UserInfo != null);
		}

		return this.UserInfo;
	}

	/** 유저 정보를 저장한다 */
	public void SaveUserInfo() {
		this.SaveUserInfo(KCDefine.U_DATA_P_COMMON_USER_INFO);
	}

	/** 유저 정보를 저장한다 */
	public void SaveUserInfo(string a_oFilePath) {
		CFunc.WriteMsgPackObj(a_oFilePath, this.UserInfo, true);
	}
	#endregion // 함수
}
