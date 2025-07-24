/****************************************************************************
 * Copyright (c) 2016 ~ 2022  UNDER MIT LICENSE
 * 
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

namespace XXLFramework
{

	[Serializable]
	public class QFrameworkServerResultFormat<T>
	{
		public int code;

		public string msg;

		public T data;
	}

	internal class PackageManagerServer : AbstractModel, IPackageManagerServer,ICanGetModel
    {
        public void DeletePackage(string packageId, System.Action onResponse)
        {
            var form = new WWWForm();

            form.AddField("UserName", User.Username.Value);
            form.AddField("Password", User.Password.Value);
            form.AddField("PackageId", packageId);

            EditorHttp.Post($"{UrlSetting.BaseUrl}/Package/Delete", form, (response) =>
            {
                var result = JsonUtility.FromJson<QFrameworkServerResultFormat<object>>(response.Text);

                if (result.code == 200)
                {
                    Debug.Log("删除成功");

                    onResponse();
                }
            });
        }
        
        public void GetAllRemotePackageInfoV5(Action<List<PackageRepository>, List<string>> onResponse)
        {
            EditorHttp.Get($"{UrlSetting.BaseUrl}/Package/list",
                   (response) => OnResponseV5(response, onResponse));
            //if (User.Logined)
            //{
            //    var form = new WWWForm();

            //    form.AddField("UserName", User.Username.Value);
            //    form.AddField("Password", User.Password.Value);

            //    EditorHttp.Get($"{PackageKit.Url}/list", form,
            //        (response) => OnResponseV5(response, onResponse));
            //}
            //else
            //{
               
            //}
        }



        [Serializable]
        public class ListPackageResponseResult
        {
            public List<string> categories;
            public List<PackageRepository> repositories;
        }
        
        void OnResponseV5(EditorHttpResponse response, Action<List<PackageRepository>, List<string>> onResponse)
        {
            if (response.Type == ResponseType.SUCCEED)
            {
                var responseJson =
                    JsonUtility.FromJson<QFrameworkServerResultFormat<ListPackageResponseResult>>(response.Text);


                if (responseJson == null)
                {
                    onResponse(null,null);
                    return;
                }
                
                if (responseJson.code == 200)
                {
                    var listPackageResponseResult = responseJson.data;


                    var packageTypeConfigModel = this.GetModel<PackageTypeConfigModel>();
					foreach (var packageRepository in listPackageResponseResult.repositories)
					{
						packageRepository.type = packageTypeConfigModel.GetFullTypeName(packageRepository.type);
					}

					new PackageInfosRequestCache()
                    {
                        PackageRepositories = listPackageResponseResult.repositories
                    }.Save();
                    
                    onResponse(listPackageResponseResult.repositories, listPackageResponseResult.categories);
                }
            }
            else
            {
                onResponse(null,null);
            }
        }

        protected override void OnInit()
        {
            
        }
    }
}
#endif