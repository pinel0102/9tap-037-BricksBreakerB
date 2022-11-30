#if MSG_PACK_ENABLE && EXTRA_SCRIPT_MODULE_ENABLE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using MessagePack;
using MessagePack.Resolvers;

#if UNITY_EDITOR
using UnityEditor;
#endif // #if UNITY_EDITOR

/** 메세지 팩 등록자 */
public static partial class CMsgPackRegister {
#region 클래스 변수
	private static bool m_bIsRegister = false;
#endregion // 클래스 변수

#region 클래스 함수
	/** 메세지 팩을 등록한다 */
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void RegisterMsgPack() {
		// 등록 가능 할 경우
		if(!CMsgPackRegister.m_bIsRegister) {
			CMsgPackRegister.m_bIsRegister = true;

			var oResolverList = new List<IFormatterResolver>() {
				MessagePack.Resolvers.StandardResolver.Instance, MessagePack.Resolvers.GeneratedResolver.Instance
			};

			StaticCompositeResolver.Instance.Register(oResolverList.ToArray());
			MessagePackSerializer.DefaultOptions = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);
		}
	}
#endregion // 클래스 함수

#region 조건부 클래스 함수
#if UNITY_EDITOR
	/** 초기화 */
	[InitializeOnLoadMethod]
	public static void EditorInitialize() {
		CMsgPackRegister.RegisterMsgPack();
	}
#endif // #if UNITY_EDITOR
#endregion // 조건부 클래스 함수
}
#endif // #if MSG_PACK_ENABLE && EXTRA_SCRIPT_MODULE_ENABLE
