using System.Windows;

namespace SebiCopyWPF
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainView : Window
  {
    /// <summary>
    /// Ctor.
    /// </summary>
    public MainView()
    {
      InitializeComponent();
      DataContext = new MainViewModel();
    }

    private void wrapPanel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
      (DataContext as MainViewModel).RemoveCheckBox();
    }
  }
}