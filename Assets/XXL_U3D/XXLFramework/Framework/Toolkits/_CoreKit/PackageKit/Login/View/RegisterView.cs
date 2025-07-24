/****************************************************************************
 * Copyright (c) 2020.10 
 * 
 * https://xxlframework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 ****************************************************************************/

#if UNITY_EDITOR
using System;
using UnityEngine;

namespace XXLFramework
{
    internal class RegisterView : VerticalLayout, IController
    {
        //string UserName;
        //string Password;
        //string Email;

        public RegisterView()
        {
            var usernameLine = EasyIMGUI.Horizontal().Parent(this);
            EasyIMGUI.Label().Text("username:").Parent(usernameLine);
            var userName = EasyIMGUI.TextField().Parent(usernameLine);

            var passwordLine = new HorizontalLayout().Parent(this);
            EasyIMGUI.Label().Text("password:").Parent(passwordLine);
            var password = EasyIMGUI.TextField().PasswordMode().Parent(passwordLine);

            var emailLine = new HorizontalLayout().Parent(this);
            EasyIMGUI.Label().Text("email:").Parent(emailLine);
            var email = EasyIMGUI.TextField().Parent(emailLine);


            EasyIMGUI.Button()
                .Text("注册")
                .OnClick(()=> { Register(userName.Content.Value, password.Content.Value, email.Content.Value); })
                .Parent(this);

            EasyIMGUI.Button()
                .Text("返回")
                .OnClick(() => { PackageKitLoginState.RegisterViewVisible.Value = false; })
                .Parent(this);
        }

		private void Register(string userName, string password, string email)
		{
            if (string.IsNullOrEmpty(userName))
            {
                Debug.LogError("用户名不能为空");
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                Debug.LogError("密码不能为空");
                return;
            }
            if (string.IsNullOrEmpty(email))
            {
                Debug.LogError("邮箱不能为空");
                return;
            }
            var form = new WWWForm();
            form.AddField("Username", userName);
            form.AddField("Password", password);
            form.AddField("Email", email);
            Debug.Log($"Username：{userName},Password:{password},Email:{email}");


            EditorHttp.Post($"{UrlSetting.BaseUrl}/User/register", form, response =>
            {
                if (response.Type == ResponseType.SUCCEED)
                {
                    Debug.Log(response.Text);

                    var responseJson =
                        JsonUtility.FromJson<QFrameworkServerResultFormat<string>>(response.Text);
                    Debug.Log(responseJson.msg);
                    int code = responseJson.code;
                    if (responseJson.code == 200)
                    {
                        PackageKitLoginState.RegisterViewVisible.Value = false;
                    }
                }
                else if (response.Type == ResponseType.EXCEPTION)
                {
                    Debug.LogError(response.Error);
                }
            });
        }

		protected override void OnDisposed()
        {
        }

        public IArchitecture GetArchitecture()
        {
            return PackageKitLoginApp.Interface;
        }
    }
}
#endif