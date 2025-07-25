using System.Collections;
using UnityEngine;

namespace XXLFramework.Example
{
	public class ReplaceableMonoSingletonExample : MonoBehaviour
	{
		// Use this for initialization
		IEnumerator Start()
		{
			// 创建一个单例
			var instance = GameManager.Instance;

			// 强制创建一个实例
			new GameObject().AddComponent<GameManager>();

			// 等一帧，等待第二个 GameManager 把自己删除
			yield return new WaitForEndOfFrame();
			
			// 结果为 1 
			Debug.Log(FindObjectsOfType<GameManager>().Length);
			
			// 最先创建的实例已经被删除
			Debug.Log(instance != FindObjectOfType<GameManager>());
		}


		public class GameManager : ReplaceableMonoSingleton<GameManager>
		{

		}
	}
}