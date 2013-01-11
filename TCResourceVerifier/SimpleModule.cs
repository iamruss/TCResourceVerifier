using Ninject.Modules;
using TCResourceVerifier.Interfaces;
using TCResourceVerifier.Services;

namespace TCResourceVerifier
{
    public class SimpleModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IFileSystemService>().To<FileSystemServiceImpl>();
            Bind<IWidgetReader>().To<WidgetReader>();
        }
    }
}