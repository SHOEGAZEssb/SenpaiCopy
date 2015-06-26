using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

    public MainWindow()
    {
      InitializeComponent();
    }

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

    private void UpdatePictureBox()
    {
      if (_pathList.Count > _currentImageIndex)
      {
        try
        {
          
          _currentImage = new BitmapImage(new Uri(_pathList[_currentImageIndex]));
          if (_currentImage.UriSource.AbsolutePath.EndsWith(".gif"))
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

    private void btnCopy_Click(object sender, RoutedEventArgs e)
    {
      _currentImage = null;

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

      _currentImage = null;
      File.Delete(_pathList[_currentImageIndex]);

      foreach (object c in unfiformGridSubfolders.Children)
      {
        (c as PathCheckBox).IsChecked = false;
      }

      _currentImageIndex++;
      UpdatePictureBox();
    }

    private void btnIgnore_Click(object sender, RoutedEventArgs e)
    {
      _currentImage = null;
      _currentImageIndex++;
      UpdatePictureBox();
    }

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

    private void pictureBoxCurrentImage_MouseDown(object sender, MouseButtonEventArgs e)
    {
      Process.Start(lblCurrentImageFile.Content.ToString());
    }
  }
}