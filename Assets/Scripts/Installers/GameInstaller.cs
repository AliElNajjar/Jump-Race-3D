using Input;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        public InputManager InputManager;
        public override void InstallBindings()
        {
            Container.Bind<InputManager>().AsSingle();
        }
    }
}
