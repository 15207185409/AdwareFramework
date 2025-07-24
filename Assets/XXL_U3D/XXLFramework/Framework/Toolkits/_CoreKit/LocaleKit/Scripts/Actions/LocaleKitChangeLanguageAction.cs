using UnityEngine;

namespace XXLFramework
{
    public class LocaleKitChangeLanguageAction : MonoBehaviour
    {
        public Language Language;

        public void Execute()
        {
            LocaleKit.ChangeLanguage(Language);
        }
    }
}
