using TapTap.License;
using TapTap.License.Internal;
using TapTap.Common;
using System.Collections.Generic;
using TapTap.Common.Standalone;

namespace TapTap.License.Standalone {
    
#if UNITY_STANDALONE_WIN
    public class TapLicenseStandalone : ITapLicenseBridge
    {

        private ITapDlcCallback currentDLCCallback;

        private ITapLicenseCallback currentLicenseCallback;

        // 是否已在 dll 层注册回调
        private bool HasRegisterNativeDLCCallback = false;

        private bool HasRegisterNativeLicenseCallback = false;

        public void Check(bool force = false)
        {

            var isOwned = TapClientStandalone.HasLicense();
            if (currentLicenseCallback != null)
            {
                if (isOwned)
                {
                    currentLicenseCallback.OnLicenseSuccess();
                }
                else
                {
                    currentLicenseCallback.OnLicenseFailed();
                }
            }
        }


        public void QueryDLC(string[] skus)
        {
            if (skus.Length == 0)
            {
                return;
            }
            Dictionary<string, object> dlcResult = new Dictionary<string, object>();
            foreach (string skuId in skus)
            {
                bool isOwned = TapClientStandalone.QueryDLC(skuId);
                dlcResult.Add(skuId, isOwned ? 1 : 0);
            }
            if (currentDLCCallback != null)
            {
                currentDLCCallback.OnQueryCallBack(TapLicenseQueryCode.QUERY_RESULT_OK, dlcResult);
            }
        }

        public void SetDLCCallback(ITapDlcCallback callback)
        {
            currentDLCCallback = callback;
            if (!HasRegisterNativeDLCCallback)
            {
                HasRegisterNativeDLCCallback = true;
                TapClientStandalone.RegisterDLCOwnedCallback(DLCCallbackDelegate);
            }
        }


        public void SetLicencesCallback(ITapLicenseCallback callback)
        {
            currentLicenseCallback = callback;
            if (!HasRegisterNativeLicenseCallback)
            {
                HasRegisterNativeLicenseCallback = true;
                TapClientStandalone.RegisterLicenseCallback(LicenseCallbackDelegate);
            }
        }


        public void SetTestEnvironment(bool isTest)
        {
            TapLogger.Warn($"{nameof(SetTestEnvironment)} NOT implemented.");
        }

        public void PurchaseDLC(string skuId)
        {
            TapClientStandalone.ShowStore(skuId);
        }

        private void DLCCallbackDelegate(string skuId, bool isOwned)
        {
            if (currentDLCCallback != null)
            {
                currentDLCCallback.OnOrderCallBack(skuId, isOwned ? TapLicensePurchasedCode.DLC_PURCHASED : TapLicensePurchasedCode.DLC_NOT_PURCHASED);
            }
        }


        private void LicenseCallbackDelegate(bool isOwned)
        {
            if (currentLicenseCallback != null)
            {
                if (isOwned)
                {
                    currentLicenseCallback.OnLicenseSuccess();
                }
                else
                {
                    currentLicenseCallback.OnLicenseFailed();
                }
            }
        }

        public void SetDLCCallback(ITapDlcCallback callback, bool checkOnce, string publicKey)
        {
            SetDLCCallback(callback);
        }
    }
#endif
}
