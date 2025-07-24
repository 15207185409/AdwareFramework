/****************************************************************************
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

using UnityEngine;
using System.Collections.Generic;
using System;

namespace XXLFramework
{
	public class UIManager : MonoBehaviour
	{
		private static UIManager mInstance;
		public static UIManager Instance
		{
			get
			{
				if (!mInstance)
				{
					mInstance = FindObjectOfType<UIManager>();
				}

				if (!mInstance)
				{
					mInstance = Instantiate(Resources.Load<GameObject>("UIManager")).GetComponent<UIManager>();
					mInstance.name = "UIManager";
					DontDestroyOnLoad(mInstance);
				}
				return mInstance;
			}
		}

		private PanelContainer currentContainer; //当前容器		
		public PanelContainer CurrentContainer
		{
			get
			{
				if (currentContainer != null)
				{
					return currentContainer;
				}
				else
				{
					currentContainer = FindObjectOfType<PanelContainer>();
					if (currentContainer==null)
					{
						currentContainer = transform.Find("DefaultContainer").GetComponent<PanelContainer>();
					}
					currentContainer.Show();
					currentContainer.Init();
				}
				return currentContainer;
			}
		}

		private ContainerFSM<Type> containerTable = new ContainerFSM<Type>();

		public UIPanelAssets UIPanelAssets;


		public void Init()
		{
			PanelContainer defaultContainer = Instance.CurrentContainer;
			AddContainer(defaultContainer);
			containerTable.StartState(currentContainer.GetType());
		}

		public void AddContainer(PanelContainer container)
		{
			containerTable.AddState(container.GetType(), container);
		}

		public void RemoveContainer<T>(T t = null) where T : PanelContainer
		{
			if (t == null)
			{
				containerTable.RemoveState(typeof(T));
			}
			else
			{
				containerTable.RemoveState(t.GetType());
			}
		}

		public void ChangeContainer<T>() where T : PanelContainer
		{
			containerTable.ChangeState((typeof(T)));
			currentContainer = containerTable.GetState(typeof(T));
		}

		public bool IncludeContainer<T>() where T : PanelContainer
		{
			return containerTable.IncludeContainer(typeof(T));
		}

		public PanelContainer GetContainer<T>() where T : PanelContainer
		{
			return containerTable.GetState(typeof(T));
		}

		public BasePanel CreateUIPanel<T>(string gameObjName = null)
		{
			BasePanel panel;
			if (gameObjName.IsNotNullAndEmpty())
			{
				panel = UIPanelAssets.GetUIPanel(gameObjName);
			}
			else
			{
				panel = UIPanelAssets.GetUIPanel(typeof(T));
			}
			if (panel != null)
			{
				var obj = Instantiate(panel.gameObject);
				var result = obj.GetComponent<BasePanel>();
				panel.gameObject.name = gameObjName ?? typeof(T).Name;
				return result;
			}
			else
			{
				return null;
			}
		}


	}
	public interface IContainerState
	{
		void Enter();
		void Exit();
	}

	public class ContainerFSM<T>
	{
		protected Dictionary<T, PanelContainer> mStates = new Dictionary<T, PanelContainer>();

		public void AddState(T id, PanelContainer state)
		{
			if (IncludeContainer(id) == false)
			{
				mStates.Add(id, state);
			}
		}

		public void RemoveState(T id)
		{
			mStates.Remove(id);
		}

		public PanelContainer GetState(T id)
		{
			if (IncludeContainer(id))
			{
				return mStates[id];
			}
			else
			{
				Debug.Log($"未找到{id.ToString()}");
				return null;
			}
		}

		public bool IncludeContainer(T id)
		{
			if (mStates.ContainsKey(id))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private PanelContainer mCurrentState;
		private T mCurrentStateId;

		public PanelContainer CurrentState => mCurrentState;
		public T CurrentStateId => mCurrentStateId;
		public T PreviousStateId { get; private set; }


		public void ChangeState(T t)
		{
			if (t.Equals(CurrentStateId)) return;

			if (mStates.TryGetValue(t, out var state))
			{
				if (mCurrentState != null)
				{
					mCurrentState.Exit();
					PreviousStateId = mCurrentStateId;
					mCurrentState = state;
					mCurrentStateId = t;
					mCurrentState.Enter();
				}
			}
		}

		public void StartState(T t)
		{
			if (mStates.TryGetValue(t, out var state))
			{
				PreviousStateId = t;
				mCurrentState = state;
				mCurrentStateId = t;
				state.Enter();
			}
		}

		public void Clear()
		{
			mCurrentState = null;
			mCurrentStateId = default;
			mStates.Clear();
		}


	}
}