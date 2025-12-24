
namespace TapTap.License.Internal {
    public interface ITapLicenseBridge
    {
        void SetDLCCallback(ITapDlcCallback callback);

        void QueryDLC(string[] skus);

        void SetDLCCallback(ITapDlcCallback callback, bool checkOnce, string publicKey);

        void Check(bool force = false);

        void SetLicencesCallback(ITapLicenseCallback callback);

        void PurchaseDLC(string sku);

        void SetTestEnvironment(bool isTest);



    }
}