using System;
using UnityEngine;

namespace XXLFramework
{
	public static class GameArchitectureExtension 
    {
		public static void SendCommand<TCommand>(this MonoBehaviour self)where TCommand : AbstractCommand, new()
		{
			GameArchitecture.Interface.SendCommand<TCommand>();
		}

		public static void SendCommand<TCommand>(this MonoBehaviour self, TCommand command) where TCommand : AbstractCommand
		{
			GameArchitecture.Interface.SendCommand(command);
		}

		public static  TSystem GetSystem<TSystem>(this MonoBehaviour self) where TSystem :  AbstractSystem
		{
			return GameArchitecture.Interface.GetSystem<TSystem>();
		}

		public static TModel GetModel<TModel>(this MonoBehaviour self)where TModel : class, IModel 
		{
			return GameArchitecture.Interface.GetModel<TModel>();
		}

		public static void SendEvent<TEvent>(this MonoBehaviour self) where TEvent : new()
		{
			GameArchitecture.Interface.SendEvent<TEvent>();
		}

		public static void SendEvent<TEvent>(this MonoBehaviour self, TEvent e)
		{
			GameArchitecture.Interface.SendEvent<TEvent>(e);
		}

		public static IUnRegister RegisterEvent<TEvent>(this MonoBehaviour self, Action<TEvent> onEvent)
		{
			return GameArchitecture.Interface.RegisterEvent<TEvent>(onEvent);
		}

		public static void UnRegisterEvent<TEvent>(this MonoBehaviour self, Action<TEvent> onEvent)
		{
			GameArchitecture.Interface.UnRegisterEvent<TEvent>(onEvent);
		}

		public static TResult SendQuery<TResult>(this MonoBehaviour self, IQuery<TResult> query)
		{
			return GameArchitecture.Interface.SendQuery<TResult>(query);
		}

	}
}
