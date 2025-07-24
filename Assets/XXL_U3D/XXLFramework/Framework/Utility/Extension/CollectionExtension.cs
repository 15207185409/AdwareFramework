using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XXLFramework
{
    public static class CollectionExtension 
    {
		/// <summary>
		/// 根据所占百分比来显示对应物体的数量
		/// </summary>
		/// <param name="gameObjects">物体列表</param>
		/// <param name="percentage">范围0-1</param>
		public static void ShowGameobjectByPercentage(this List<GameObject> gameObjects, float percentage)
		{
			if (gameObjects.Count > 0)
			{
				int showCount = (int)(percentage * gameObjects.Count);
				for (int i = 0; i < gameObjects.Count; i++)
				{
					if (i < showCount)
					{
						gameObjects[i].Show();
					}
					else
					{
						gameObjects[i].Hide();
					}
				}
			}
		}

		/// <summary>
		/// 根据所占百分比来显示对应物体的数量
		/// </summary>
		/// <param name="gameObjects">物体列表</param>
		/// <param name="percentage">范围0-1</param>
		public static void ShowGameobjectByPercentage<T>(this List<T> gameObjects, float percentage) where T : Component
		{
			if (gameObjects.Count > 0)
			{
				int showCount = (int)(percentage * gameObjects.Count);
				for (int i = 0; i < gameObjects.Count; i++)
				{
					if (i < showCount)
					{
						gameObjects[i].Show();
					}
					else
					{
						gameObjects[i].Hide();
					}
				}
			}

		}

		/// <summary>
		/// 根据索引显示物体
		/// </summary>
		/// <param name="objList"></param>
		/// <param name="indexList"></param>
		public static void ShowObjsByIndexs(this List<GameObject> objList, List<int> indexList)
		{
			for (int i = 0; i < objList.Count; i++)
			{
				if (indexList.Contains(i))
				{
					objList[i].Show();
				}
				else
				{
					objList[i].Hide();
				}
			}
		}

		/// <summary>
		/// 根据索引显示物体
		/// </summary>
		/// <param name="objList"></param>
		/// <param name="indexList"></param>
		public static void ShowObjsByIndexs<T>(this List<T> objList, List<int> indexList) where T : Component
		{
			for (int i = 0; i < objList.Count; i++)
			{
				if (indexList.Contains(i))
				{
					objList[i].Show();
				}
				else
				{
					objList[i].Hide();
				}
			}
		}

		// 打乱泛型列表
		public static List<t> DisorderItems<t>(this List<t> TList)  
		{
			List<t> NewList = new List<t>();
			for (int i = TList.Count - 1; i >= 0; i--)
			{
				int randomIndex = Random.Range(0, TList.Count);
				NewList.Add(TList[randomIndex]);
				TList.RemoveAt(randomIndex);
			}
			return NewList;
		}

		public static List<int> CreateIntList( int min, int max) 
		{
			List<int> result = new List<int>();
			for (int i = min; i <= max; i++)
			{
				result.Add(i);
			}
			return result;
		}


		/// <summary>
		/// 指定区间获取对应数量道题目
		/// </summary>
		/// <param name="minValue">题目最小ID</param>
		/// <param name="maxValue">题目最大ID</param>
		/// <param name="count">要获取的题目数量</param>
		/// <returns></returns>
		public static List<int> GenerateRandomList(int minValue, int maxValue, int count)
		{
			List<int> randomList = new List<int>();

			if (count > (maxValue - minValue + 1) || count < 0)
			{
				// Handle invalid input
				return randomList;
			}

			HashSet<int> uniqueNumbers = new HashSet<int>();

			while (uniqueNumbers.Count < count)
			{
				int randomNumber = Random.Range(minValue, maxValue + 1);

				if (uniqueNumbers.Add(randomNumber))
				{
					randomList.Add(randomNumber);
				}
			}

			randomList.Sort();

			return randomList;
		}

	}
}
