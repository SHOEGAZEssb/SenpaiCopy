using System.Windows.Controls;

namespace SenpaiCopy
{
  /// <summary>
  /// CheckBox that holds a folder path.
  /// </summary>
  class PathCheckBox : CheckBox
  {
    private string _fullPath;

    /// <summary>
    /// Path to the folder.
    /// </summary>
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