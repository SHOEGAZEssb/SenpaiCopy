using System.Windows.Controls;

namespace SebiCopyWPF
{
  class PathCheckBox : CheckBox
  {
    private string _fullPath;

    public string FullPath
    {
      get { return _fullPath; }
      set
      {
        _fullPath = value;
      }
    }
  }
}
