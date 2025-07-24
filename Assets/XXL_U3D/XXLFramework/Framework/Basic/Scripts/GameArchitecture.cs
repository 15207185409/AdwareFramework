namespace XXLFramework
{
	public class GameArchitecture : Architecture<GameArchitecture>
	{
		//主动调用初始化架构，调用此方法时会走到Init方法
		public static void InitArchitecture()
		{
			IArchitecture architecture = Interface;
		}

		// 将项目中用到的System和Model注册到架构中并依次初始化
		protected override void Init()
		{
			// 注册项目用需要用到的system和model
			//RegisterSystem(new GameSystem());
			//RegisterModel(new BindPropertyDemoModel()); 

		}


	}
}