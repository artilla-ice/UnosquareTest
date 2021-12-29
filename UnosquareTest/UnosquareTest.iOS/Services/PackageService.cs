using UnosquareTest.Services;
using Xamarin.Essentials;

namespace UnosquareTest.iOS.Services
{
    public class PackageService : IPackageService
    {
        public string GetAppPackageName()
        {
            return this.GetBundleiOS();
        }

        private string GetBundleiOS()
        {
            //return Foundation.NSBundle.MainBundle.BundleIdentifier;
            return AppInfo.PackageName;
        }
    }
}