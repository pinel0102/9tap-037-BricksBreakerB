//
//  GFunc.mm
//  Unity-iPhone
//
//  Created by 이상동 on 2020/01/11.
//

#import "GFunc.h"

namespace GFunc {
	/** 유효 여부를 검사한다 */
	BOOL IsValid(NSString *a_pStr) {
		return a_pStr != nil && a_pStr.length > G_VAL_0_INT;
	}
	
	/** 유효 여부를 검사한다 */
	BOOL IsValid(EVibrateType a_eType) {
		return a_eType > EVibrateType::NONE && a_eType < EVibrateType::MAX_VAL;
	}

	/** 문자열 => 논리로 변화한다 */
	BOOL ConvertStrToBool(NSString *a_pStr) {
		return [a_pStr isEqualToString:@(G_RESULT_TRUE)];
	}

	/** 논리 => 문자열로 변환한다 */
	NSString * ConvertBoolToStr(BOOL a_bIsTrue) {
		return a_bIsTrue ? @(G_RESULT_TRUE) : @(G_RESULT_FALSE);
	}

	/** 객체 => JSON 문자열로 변환한다 */
	NSString * ConvertObjToJSONStr(NSObject *a_pObj, NSError **a_pError) {
		NSData *pData = [NSJSONSerialization dataWithJSONObject:a_pObj options:NSJSONWritingPrettyPrinted error:a_pError];
		return [[NSString alloc] initWithData:pData encoding:NSUTF8StringEncoding];
	}

	/** JSON 문자열 => 객체로 변환한다 */
	NSObject * ConvertJSONStrToObj(NSString *a_pStr, NSError **a_pError) {
		NSData *pData = [a_pStr dataUsingEncoding:NSUTF8StringEncoding];
		return [NSJSONSerialization JSONObjectWithData:pData options:NSJSONReadingMutableContainers error:a_pError];
	}
	
	/** URL 요청을 생성한다 */
	NSMutableURLRequest * MakeURLRequest(NSString *a_pURL, NSString *a_pMethod, double a_dblTimeout) {
		NSMutableURLRequest *pRequest = [NSMutableURLRequest requestWithURL:[NSURL URLWithString:a_pURL] cachePolicy:NSURLRequestReloadIgnoringLocalAndRemoteCacheData timeoutInterval:a_dblTimeout];
		pRequest.HTTPMethod = a_pMethod;
		
		return pRequest;
	}
}
