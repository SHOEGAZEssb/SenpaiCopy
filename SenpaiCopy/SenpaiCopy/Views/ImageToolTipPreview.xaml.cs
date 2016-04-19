using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using XamlAnimatedGif;

namespace SenpaiCopy
{
	/// <summary>
	/// Interaction logic for ImageToolTipPreview.xaml
	/// </summary>
	public partial class ImageToolTipPreview : UserControl
	{
		#region Properties

		/// <summary>
		/// DependencyProperty of <see cref="FileName"/>
		/// </summary>
		public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register("FileName", typeof(string),
		 typeof(ImageToolTipPreview), new PropertyMetadata("", OnFileNamePropertyChanged));

		/// <summary>
		/// File name of the image.
		/// </summary>
		public string FileName
		{
			get { return (string)GetValue(FileNameProperty); }
			set { SetValue(FileNameProperty, value); }
		}

		#endregion Properties

		/// <summary>
		/// Ctor.
		/// </summary>
		public ImageToolTipPreview()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Loads an image from the given <see cref="FileName"/> and assigns it to the image.
		/// </summary>
		/// <param name="source">DependencyObject that triggered this event.</param>
		/// <param name="e">EventArgs that contain the set value.</param>
		private static void OnFileNamePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
		{
			ImageToolTipPreview control = source as ImageToolTipPreview;
			string fileName = e.NewValue.ToString();
			if (fileName == "" || Properties.Settings.Default.SupportedVlcFormats.Split(';').Any(f => fileName.EndsWith(f)))
				return;
			AnimationBehavior.SetSourceUri(control.img, new Uri(fileName));
		}
	}
}