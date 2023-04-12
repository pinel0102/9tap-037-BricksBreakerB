using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProjectManager
{
#region Project Settings

    // < 프로젝트 이름 변경 방법 >
    // 1. productName / appIdentifier 변경
    // 2. Firebase 세팅 파일 google-services.json : package_name을 변경한 Identifier로 변경 (실제로 존재하지 않아도 됨)
    // 3. Firebase 세팅 파일 googleService-Info.plist : BUNDLE_ID를 변경한 Identifier로 변경 (실제로 존재하지 않아도 됨)
    // 4. Facebook > Edit Settings > Facebook App Id : 대충 다른 Id로 변경 (실제로 존재하지 않아도 됨)
    // 5. Assets > External Dependency Manager > Android Resolver > Resolve
    // 6. 완료 후 APK를 빌드하면 기존 앱과 별개로 설치된다.
    public const string projectName = "103711.Puzzle_BricksBreakerB";
    public const string appIdentifier = "com.ninetap.brickbreaker";
    public const string appStoreConnectId = "1610049465";
    public const string appStoreAppId = "Ninetap";

#if UNITY_ANDROID
    public const string productName = "Brick Breaker - Shoot the Ball";
#elif UNITY_IOS
    public const string productName = "Brick Breaker - Shoot the Ball";
#elif UNITY_STANDALONE 
    public const string productName = "Brick Breaker Editor";
#else
    public const string productName = "Brick Breaker";
#endif

#endregion Project Settings

#region SDK Keys

    // Assets/IronSource/Resources : 세팅 애셋에 입력 필요.
    // Assets/IronSource/Plugins/Android/AndroidManifest.xml : AdMob 키 입력 필요. (Android)
    public const string ironSource_AppKey_AOS = "19878e3d5";
    public const string ironSource_AppKey_iOS = "198791d5d";
    public const string ironSource_Admob_AppID_AOS = "ca-app-pub-9834260879045639~9072776983"; // need update
    public const string ironSource_Admob_AppID_iOS = "ca-app-pub-9834260879045639~1357938495"; // need update

    // AppsFlyerManager.cs : 추가 작업 필요 없음.
    public const string appsflyer_dev_key = "J7eXAem8sBRuHTr3iX58d5";

    // FlurryManager.cs : 추가 작업 필요 없음.
    public const string flurry_Api_Key_AOS = "P68VWCZ4W8GPPCNKHNXR"; // need update
    public const string flurry_Api_Key_iOS = "WQG94NW32FXJPKG2V4XJ"; // need update

#endregion SDK Keys

#region Company Info

    public const string companyName = "9tap";
    public const string servicesURL_KR = "https://www.ninetap.com/terms_of_service_kr.html";
    public const string servicesURL_EU = "https://www.ninetap.com/terms_of_service.html";
    public const string privacyURL_KR = "https://www.ninetap.com/privacy_policy_kr.html";    
    public const string privacyURL_EU = "https://www.ninetap.com/privacy_policy.html";    
    public const string storeURL_AOS = "https://play.google.com/store/apps/details?id={0}";
    public const string storeURL_iOS = "https://apps.apple.com/app/id{0}";
    public const string moreGamesURL_AOS = "https://play.google.com/store/apps/developer?id=Ninetap";
    public const string moreGamesURL_iOS = "https://apps.apple.com/us/developer/ninetap/id1456103822#see-all/i-phonei-pad-apps";
    public const string supportsMail = "cs@ninetap.com";

#endregion Company Info

}