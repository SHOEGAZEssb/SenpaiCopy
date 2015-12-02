using System.Windows.Controls;

namespace SenpaiCopy
{
	/// <summary>
	/// CheckBox that holds a folder path.
	/// </summary>
	internal class PathCheckBox : CheckBox
	{
		#region Properties

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
		private string _fullPath;

		#endregion
	}
}