
namespace Thnguyet.UI
{
	public interface IUICreator
	{
		T GetUIInstance<T>(string assetPath) where T : UIBase;

		void ReleaseUIInstance(UIBase ui);
	}
}
