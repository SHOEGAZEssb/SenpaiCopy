using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace SebiCopyWPF
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private List<Image> _imageList = new List<Image>();
    private List<string> _pathList = new List<string>();
    private int _currentImageIndex = 0;
    private BitmapImage _currentImage;

    /// <summary>
    /// Ctor.
    /// </summary>
    public MainWindow()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Lets the user select a folder with images and loads them.
    /// </summary>
    private void btnSelectImageFolder_Click(object sender, RoutedEventArgs e)
    {
      FolderBrowserDialog dlg = new FolderBrowserDialog();
      if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        lblImageFolderPath.Content = dlg.SelectedPath;

        //get all files
        string[] files = Directory.GetFiles(dlg.SelectedPath);

        //get all image files
        foreach (string file in files)
        {
          string test = file.Substring(file.Length - 4, 4);
          if (file.Substring(file.Length - 4, 4) == ".png" || file.Substring(file.Length - 4, 4) == ".bmp" || file.Substring(file.Length - 4, 4) == ".jpg" || file.Substring(file.Length - 5, 5) == ".jpeg" || file.Substring(file.Length - 4, 4) == ".gif")
          {
            _pathList.Add(file);
          }
        }
      }

      btnCopy.IsEnabled = true;
      btnIgnore.IsEnabled = true;
      UpdatePictureBox();
    }

    /// <summary>
    /// Updates the currently shown image.
    /// </summary>
    private void UpdatePictureBox()
    {
      if (_pathList.Count > _currentImageIndex)
      {
        try
        {

          _currentImage = LoadBitmapImage(_pathList[_currentImageIndex]);
          if (_currentImage.UriSource.LocalPath.EndsWith(".gif"))
            ImageBehavior.SetAnimatedSource(pictureBoxCurrentImage, _currentImage);
          else
            pictureBoxCurrentImage.Source = _currentImage;
          lblCurrentImageFile.Content = _pathList[_currentImageIndex];
        }
        catch (OutOfMemoryException)
        {
          System.Windows.Forms.MessageBox.Show("The RAM is a lie. Folgendes Bild hat den Error verursacht: " + _pathList[_currentImageIndex] + " | Wir skippen das Bild einfach mal.");
          if (_currentImage != null)
            _currentImage = null;

          _currentImageIndex++;
          UpdatePictureBox();
        }
      }
      else
      {
        pictureBoxCurrentImage.Source = null;
        btnCopy.IsEnabled = false;
        btnIgnore.IsEnabled = false;
        lblCurrentImageFile.Content = "No more lel.";
      }
    }

    /// <summary>
    /// Copies the current image to the selected folders and removes it.
    /// </summary>
    private void btnCopy_Click(object sender, RoutedEventArgs e)
    {
      List<string> dirsToCopyTo = new List<string>();

      foreach (object c in unfiformGridSubfolders.Children)
      {
        PathCheckBox chk = (PathCheckBox)c;
        if ((bool)chk.IsChecked)
          dirsToCopyTo.Add(chk.FullPath);
      }

      foreach (string dir in dirsToCopyTo)
      {
        File.Copy(_pathList[_currentImageIndex], dir + @"\" + System.IO.Path.GetFileName(_pathList[_currentImageIndex]), true);
      }

      File.Delete(_pathList[_currentImageIndex]);

      foreach (object c in unfiformGridSubfolders.Children)
      {
        (c as PathCheckBox).IsChecked = false;
      }

      _currentImageIndex++;
      UpdatePictureBox();
    }

    /// <summary>
    /// Skips the current image doing nothing with it.
    /// </summary>
    private void btnIgnore_Click(object sender, RoutedEventArgs e)
    {
      _currentImageIndex++;
      UpdatePictureBox();
    }

    /// <summary>
    /// Opens a dialog to select a subfolder and creates checkBoxes for each path.
    /// </summary>
    private void btnSelectSubfolderFolder_Click(object sender, RoutedEventArgs e)
    {
      FolderBrowserDialog dlg = new FolderBrowserDialog();
      if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        unfiformGridSubfolders.Children.Clear();

        lblSubfolderFolderPath.Content = dlg.SelectedPath;

        SearchOption so;
        if ((bool)checkBoxSubSubDirectories.IsChecked)
          so = SearchOption.AllDirectories;
        else
          so = SearchOption.TopDirectoryOnly;

        string[] subfolders = Directory.GetDirectories(dlg.SelectedPath, "*", so);

        List<PathCheckBox> _checkBoxes = new List<PathCheckBox>();
        foreach (string folder in subfolders)
        {
          PathCheckBox chk = new PathCheckBox();
          chk.Content = new DirectoryInfo(folder).Name;
          chk.FullPath = folder;
          chk.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
          chk.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
          chk.Margin = new Thickness(10, 0, 0, 0);
          unfiformGridSubfolders.Children.Add(chk);
        }
      }
    }

    /// <summary>
    /// Opens the current image in the standard image viewer.
    /// </summary>
    private void pictureBoxCurrentImage_MouseDown(object sender, MouseButtonEventArgs e)
    {
      Process.Start(lblCurrentImageFile.Content.ToString());
    }

    /// <summary>
    /// Loads an image file from the given <paramref name="fileName"/>.
    /// This does still allow operations done to the file.
    /// </summary>
    /// <param name="fileName">Image file to load.</param>
    /// <returns>Loaded image file.</returns>
    private static BitmapImage LoadBitmapImage(string fileName)
    {
        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.UriSource = new Uri(fileName);
        bitmapImage.EndInit();
        bitmapImage.Freeze();
        return bitmapImage;
    }
  }
}