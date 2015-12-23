using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

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
     typeof(ImageToolTipPreview), new PropertyMetadata("", OnCurrentTimePropertyChanged));

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
    private static void OnCurrentTimePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
      ImageToolTipPreview control = source as ImageToolTipPreview;
      ImageBehavior.SetAnimatedSource(control.img, LoadBitmapImage(e.NewValue.ToString()));
    }

    /// <summary>
    /// Loads an image file from the given <paramref name="fileName"/>.
    /// This does still allow operations done to the file.
    /// </summary>
    /// <param name="fileName">Image file to load.</param>
    /// <returns>Loaded image file.</returns>
    private static BitmapImage LoadBitmapImage(string fileName)
    {
      if (fileName == "" || fileName.EndsWith(".webm"))
        return null;

      try
      {
        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.UriSource = new Uri(fileName);
        bitmapImage.EndInit();
        bitmapImage.Freeze();
        return bitmapImage;
      }
      catch(Exception)
      {
        return null;
      }
    }
  }
}
