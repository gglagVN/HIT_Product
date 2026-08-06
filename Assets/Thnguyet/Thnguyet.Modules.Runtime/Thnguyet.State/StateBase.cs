namespace Thnguyet.State
{
	public abstract class StateBase
	{
		public void Enter()
		{
			OnEnter();
		}

		public void Exit()
		{
			OnExit();
		}

		public void Suspend()
		{
			OnSuspend();
		}

		public void Resume()
		{
			OnResume();
		}

		public void Update(float dt)
		{
			OnUpdate(dt);
		}

		protected abstract void OnEnter();

		protected abstract void OnExit();

		protected abstract void OnSuspend();

		protected abstract void OnResume();

		protected abstract void OnUpdate(float dt);

		protected StateBase()
		{
		}
	}
}
