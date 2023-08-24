using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using System;

public partial class CUserInfoStorage
{
    [Header("★ [Parameter] Subscription")]
    public bool subscriptionActivated;
    public SubscriptionInfo subscriptionInfo;

    public void InitSubscriptionInfo()
    {
        subscriptionActivated = false;
        subscriptionInfo = null;

#if UNITY_EDITOR
        //SubscriptionTest();
#endif
    }

    public void SetSubscriptionInfo(SubscriptionInfo info)
    {
        subscriptionActivated = true;
        subscriptionInfo = info;
        
        GlobalDefine.HideBannerAD();

        /*Debug.Log("product id is: " + info.getProductId());
        Debug.Log("purchase date is: " + info.getPurchaseDate());
        Debug.Log("subscription next billing date is: " + info.getExpireDate());
        Debug.Log("is subscribed? " + info.isSubscribed().ToString());
        Debug.Log("is expired? " + info.isExpired().ToString());
        Debug.Log("is cancelled? " + info.isCancelled());
        Debug.Log("product is in free trial peroid? " + info.isFreeTrial());
        Debug.Log("product is auto renewing? " + info.isAutoRenewing());
        Debug.Log("subscription remaining valid time until next billing date is: " + info.getRemainingTime());
        Debug.Log("is this product in introductory price period? " + info.isIntroductoryPricePeriod());
        Debug.Log("the product introductory localized price is: " + info.getIntroductoryPrice());
        Debug.Log("the product introductory price period is: " + info.getIntroductoryPricePeriod());
        Debug.Log("the number of product introductory price period cycles is: " + info.getIntroductoryPricePeriodCycles());*/
    }

    private void SubscriptionTest()
    {
        subscriptionActivated = true;
    }
}