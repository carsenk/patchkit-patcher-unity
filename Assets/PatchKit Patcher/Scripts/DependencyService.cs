using Microsoft.Practices.Unity;
using PatchKit.Patcher.AppData.Local;
using PatchKit.Patcher.AppData.Remote.Downloaders;
using PatchKit.Patcher.Unity;

namespace PatchKit.Patcher
{
    public static class DependencyService
    {
        public static readonly IUnityContainer Container;

        static DependencyService()
        {
            Container = new UnityContainer();
            Container.RegisterType<ICache, UnityCache>();
            Container.RegisterType<ITorrentClientProcessStartInfoProvider, UnityTorrentClientProcessStartInfoProvider>();
        }
    }
}