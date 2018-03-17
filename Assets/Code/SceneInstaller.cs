using _2048EventBased;
using Zenject;

namespace Assets.Code
{
	public class SceneInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.Bind<IChooseNewNumber>().To<NumberChooser>().AsSingle();
			Container.Bind<Game>().FromResolveGetter((IChooseNewNumber numberChooser) => new Game(4, numberChooser)).AsSingle();
		}
	}
}
