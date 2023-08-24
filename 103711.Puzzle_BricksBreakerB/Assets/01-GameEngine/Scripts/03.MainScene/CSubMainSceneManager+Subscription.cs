using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using System;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Purchasing;
#endif // #if UNITY_EDITOR

namespace MainScene {
    public partial class CSubMainSceneManager
    {
        public List<STProductTradeInfo> subscriptionList;

        private CPurchaseManager cPurchaseManager { get { return CPurchaseManager.Inst; } }
        private CUserInfoStorage cUserInfoStorage { get { return CUserInfoStorage.Inst; } }
        private CGameInfoStorage cGameInfoStorage { get { return CGameInfoStorage.Inst; } }
        private Coroutine cCheckSubscriptions;

        public void InitSubscriptions(bool checkAlertPopup = false)
        {
            Debug.Log(CodeManager.GetMethodName());

            subscriptionList = Factory.MakeProductTradeInfos(KDefine.G_PRODUCT_KINDS_SUBSCRIPTION_LIST).Values.ToList();

            if (cCheckSubscriptions != null) StopCoroutine(cCheckSubscriptions);
            cCheckSubscriptions = StartCoroutine(CO_CheckSubscriptions(checkAlertPopup));
        }

        private IEnumerator CO_CheckSubscriptions(bool checkAlertPopup)
        {
            cUserInfoStorage.InitSubscriptionInfo();

            yield return null;

#if !UNITY_EDITOR && !UNITY_STANDALONE && PURCHASE_MODULE_ENABLE
            while(!cPurchaseManager.IsInit)
            {
                yield return null;
            }

            CheckSubscriptions();
#endif

            if (checkAlertPopup)
                CheckAlertPopup();
        }

        private void CheckSubscriptions()
        {
            //Debug.Log(CodeManager.GetMethodName());

            for(int i=0; i < subscriptionList.Count; i++)
            {
                var stProductInfo = CProductInfoTable.Inst.GetProductInfo(subscriptionList[i].m_nProductIdx);
                if (stProductInfo.m_eProductType == ProductType.Subscription)
                {
                    Product _product = cPurchaseManager.GetProduct(stProductInfo.m_oID);
                    if (_product != null && _product.receipt != null)
                    {
                        SubscriptionInfo info = GetSubscriptionInfo(_product);
                        
                        if (info.isSubscribed() == Result.True && info.isExpired() == Result.False)
                            cUserInfoStorage.SetSubscriptionInfo(info);
                    }
                }
            }
        }

        private void CheckAlertPopup()
        {
            if (!GlobalDefine.isLevelEditor)
            {
                //Debug.Log(CodeManager.GetMethodName());
            
                if (Access.IsEnableSubscriptionAlert(cGameInfoStorage.PlayCharacterID) || (cUserInfoStorage.subscriptionActivated && Access.IsEnableGetSubscriptionReward(cGameInfoStorage.PlayCharacterID)))
                {
                    OnClick_OpenPopup_Subscription();
                }
            }
        }

        private SubscriptionInfo GetSubscriptionInfo(string _productID)
        {
            return GetSubscriptionInfo(cPurchaseManager.GetProduct(_productID));
        }

        private SubscriptionInfo GetSubscriptionInfo(Product _product)
        {
            if (_product.receipt == null)
                return null;
            
            if (_product.definition.type == ProductType.Subscription)
            {
                SubscriptionManager p = new SubscriptionManager(_product, null);
                return p.getSubscriptionInfo();
            }
            else
            {
                Debug.LogWarning(CodeManager.GetMethodName() + string.Format("{0} : {1}", _product.definition.id, _product.definition.type));
                return null;
            }
        }
    }
}

#endif