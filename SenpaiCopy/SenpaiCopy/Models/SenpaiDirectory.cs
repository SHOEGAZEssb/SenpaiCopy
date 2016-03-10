using Caliburn.Micro;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace SenpaiCopy
{
	/// <summary>
	/// Represents a directory.
	/// </summary>
	/// <remarks>
	/// Most of the properties are for visualization.
	/// </remarks>
	public class SenpaiDirectory : PropertyChangedBase
	{
		#region Properties

		/// <summary>
		/// Gets/sets the full directory path.
		/// </summary>
		public string FullPath
		{
			get { return _fullPath; }
			private set
			{
				_fullPath = value;
				NotifyOfPropertyChange(() => FullPath);
			}
		}
		private string _fullPath;

		/// <summary>
		/// Gets/sets the directory name.
		/// </summary>
		public string Path
		{
			get { return _path; }
			private set
			{
				_path = value;
				NotifyOfPropertyChange(() => Path);
			}
		}
		private string _path;

		/// <summary>
		/// Gets/sets if this directory is selected.
		/// </summary>
		public bool Checked
		{
			get { return _checked; }
			set
			{
				_checked = value;
				NotifyOfPropertyChange(() => Checked);
			}
		}
		private bool _checked;

		/// <summary>
		/// Gets/sets the opacity.
		/// </summary>
		/// <remarks>
		/// Used for visualization.
		/// 0.5 when <see cref="FullPath"/> is ignored.
		/// 1.0 otherwise.
		/// </remarks>
		public double Opacity
		{
			get { return _opacity; }
			set
			{
				_opacity = value;
				NotifyOfPropertyChange(() => Opacity);
			}
		}
		private double _opacity;

		/// <summary>
		/// Gets/sets the visibility.
		/// </summary>
		/// <remarks>
		/// Used for visualization.
		/// Collapsed when <see cref="FullPath"/> is ignored.
		/// Visible otherwise.
		/// </remarks>
		public Visibility Visibility
		{
			get { return _visibility; }
			set
			{
				_visibility = value;
				NotifyOfPropertyChange(() => Visibility);
			}
		}
		private Visibility _visibility;

		/// <summary>
		/// Gets/sets the background color.
		/// </summary>
		/// <remarks>
		/// Used for visualization.
		/// Yellow if <see cref="FullPath"/> is on the favorite list.
		/// Null otherwise.
		/// </remarks>
		public SolidColorBrush BackgroundColor
		{
			get { return _backgroundColor; }
			set
			{
				_backgroundColor = value;
				NotifyOfPropertyChange(() => BackgroundColor);
			}
		}
		private SolidColorBrush _backgroundColor;

		/// <summary>
		/// Gets/sets the reference to the <see cref="MainViewModel"/>.
		/// </summary>
		public MainViewModel MainViewModel
		{
			get { return _mainViewModel; }
			private set { _mainViewModel = value; }
		}
		private MainViewModel _mainViewModel;

		#endregion Properties

		/// <summary>
		/// Ctor.
		/// </summary>
		/// <param name="fullPath">Full path to the directory.</param>
		/// <param name="mainViewModel">Reference to the <see cref="MainViewModel"/>.</param>
		public SenpaiDirectory(string fullPath, MainViewModel mainViewModel)
		{
			FullPath = fullPath;
			Path = new DirectoryInfo(fullPath).Name;
			Opacity = 1.0;
			Visibility = Visibility.Visible;
			_mainViewModel = mainViewModel;
		}

		/// <summary>
		/// Triggers when right clicked.
		/// </summary>
		public void Directory_RightClicked()
		{
			_mainViewModel.Directory_RightMouseDown(this);
		}

		/// <summary>
		/// Triggers when the CheckedChanged event of this directory fires.
		/// </summary>
		public void Directory_CheckedChanged()
		{
			_mainViewModel.Directory_CheckedChanged();
		}
	}
}