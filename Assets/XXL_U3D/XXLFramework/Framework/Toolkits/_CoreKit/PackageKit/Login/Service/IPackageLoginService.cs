using System;

namespace XXLFramework
{
    internal interface IPackageLoginService: IModel
    {
        void DoGetToken(string username, string password, Action<string> onTokenGetted);
    }
}