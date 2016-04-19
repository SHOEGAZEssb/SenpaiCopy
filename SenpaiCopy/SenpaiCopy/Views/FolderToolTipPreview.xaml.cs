using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using XamlAnimatedGif;

namespace SenpaiCopy
{
	/// <summary>
	/// Interaction logic for FolderToolTipPreview.xaml
	/// </summary>
	public partial class FolderToolTipPreview : UserControl
	{
		#region Properties

		/// <summary>
		/// DependencyProperty of <see cref="FolderPath"/>
		/// </summary>
		public static readonly DependencyProperty FolderPathProperty = DependencyProperty.Register("FolderPath", typeof(string),
		 typeof(FolderToolTipPreview), new PropertyMetadata("", OnFolderPathPropertyChanged));

		/// <summary>
		/// Path of the folder.
		/// </summary>
		public string FolderPath
		{
			get { return (string)GetValue(FolderPathProperty); }
			set { SetValue(FolderPathProperty, value); }
		}

		#endregion Properties

		/// <summary>
		/// Ctor.
		/// </summary>
		public FolderToolTipPreview()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Loads the first image from the given <see cref="FolderPath"/> and assigns it to the image.
		/// </summary>
		/// <param name="source">DependencyObject that triggered this event.</param>
		/// <param name="e">EventArgs that contain the set value.</param>
		private static void OnFolderPathPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
		{
			try
			{
				FolderToolTipPreview control = source as FolderToolTipPreview;
				if (control.DataContext != null)
				{
					SettingsViewModel svm = (control.DataContext as SenpaiDirectory).MainViewModel.SettingsViewModel;

					string dir = e.NewValue.ToString();

					//get all files
					string[] files = Directory.GetFiles(dir, "*", SearchOption.TopDirectoryOnly);

					//get all image files
					foreach (string file in files)
					{
						string lowerFile = file.ToLower();
						if (svm.EnabledFormats.Any(i => lowerFile.EndsWith(i)))
						{
							if (lowerFile != "" && !svm.SupportedVlcFormats.Any(f => lowerFile.EndsWith(f)))
							{
								AnimationBehavior.SetSourceUri(control.img, new Uri(lowerFile));
								return;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logging.LogInfo("Error displaying folder tool tip: " + ex.Message);
			}
		}
	}
}