#if UNITY_EDITOR
namespace XXLFramework
{
    internal class PackageKitLoginApp : Architecture<PackageKitLoginApp>
    {
        protected override void Init()
        {
            RegisterModel(new PackgeLoginService());
        }
    }
}
#endif