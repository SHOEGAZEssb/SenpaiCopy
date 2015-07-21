using Caliburn.Micro;

namespace SenpaiCopy
{
  class AppBootstrapper : BootstrapperBase
  {
    public AppBootstrapper()
    {
      Initialize();
    }

    protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
    {
      DisplayRootViewFor<MainViewModel>();
    }
  }
}