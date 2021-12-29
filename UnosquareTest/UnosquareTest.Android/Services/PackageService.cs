using UnosquareTest.Services;
using Xamarin.Essentials;

namespace UnosquareTest.Droid.Services
{
    public class PackageService : IPackageService
    {
        public string GetAppPackageName()
        {
            return this.GetDroidPackageName();
        }

        private string GetDroidPackageName()
        {
            return AppInfo.PackageName;
        }
    }
}