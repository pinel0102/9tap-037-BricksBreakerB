//
//  GFunc.h
//  Unity-iPhone
//
//  Created by 이상동 on 2020/01/11.
//

#import "../Define/KGDefine.h"

/** 전역 함수 */
namespace GFunc {
	/** 문자열 유효 여부를 검사한다 */
	BOOL IsValid(NSString *a_pStr);
	
	/** 진동 타입 유효 여부를 검사한다 */
	BOOL IsValid(EVibrateType a_eType);
	
	/** 문자열 => 논리로 변화한다 */
	BOOL ConvertStrToBool(NSString *a_pStr);
	
	/** 논리 => 문자열로 변환한다 */
	NSString * ConvertBoolToStr(BOOL a_bIsTrue);
	
	/** 객체 => JSON 문자열로 변환한다 */
	NSString * ConvertObjToJSONStr(NSObject *a_pObj, NSError **a_pError);
	
	/** JSON 문자열 => 객체로 변환한다 */
	NSObject * ConvertJSONStrToObj(NSString *a_pStr, NSError **a_pError);
	
	/** URL 요청을 전송한다 */
	void SendURLRequest(NSURLRequest *a_pRequest, void (^a_pfnCallback)(NSData *, NSURLResponse *, NSError *));
	
	/** URL 요청을 생성한다 */
	NSMutableURLRequest * MakeURLRequest(NSString *a_pURL, NSString *a_pMethod, double a_dblTimeout);
};
