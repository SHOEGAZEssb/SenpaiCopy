using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Linq;
using Microsoft.VisualBasic.FileIO;

namespace SenpaiCopy
{
	/// <summary>
	/// ViewModel for the MainView.
	/// </summary>
	class MainViewModel : PropertyChangedBase
	{
		#region Member

		private ObservableCollection<FileInfo> _imagePathList;
		private ObservableCollection<PathCheckBox> _checkBoxList;
		private int _currentImageIndex;
		private ImageSource _currentImage;
		private bool _includeImageSubDirectories;
		private bool _includeFolderSubDirectories;
		private string _imagePath;
		private string _folderPath;
		private bool _deleteImage;
		private bool _resetCheckBoxes;
		private PathCheckBox _currentRightClickedCheckBox;
		private ObservableCollection<string> _ignoredPaths;
		private string _checkBoxFilter;
		private string _imagePathFilter;
		private FileInfo _selectedImage;

		#endregion

		#region Properties

		/// <summary>
		/// The list of image paths. 
		/// </summary>
		public ObservableCollection<FileInfo> ImagePathList
		{
			get
			{
				if (ImagePathFilter != "")
					return new ObservableCollection<FileInfo>(_imagePathList.Where(i => i.Name.ToLower().Contains(ImagePathFilter.ToLower())).ToList<FileInfo>());
				else
					return _imagePathList;
			}
		}

		/// <summary>
		/// Gets/sets the string for filtering images.
		/// </summary>
		public string ImagePathFilter
		{
			get { return _imagePathFilter; }
			set
			{
				_imagePathFilter = value;
				NotifyOfPropertyChange(() => ImagePathList);
			}
		}

		/// <summary>
		/// Gets/sets the currently shown image.
		/// </summary>
		public ImageSource CurrentImage
		{
			get { return _currentImage; }
			private set
			{
				_currentImage = value;
				NotifyOfPropertyChange(() => CurrentImage);
				NotifyOfPropertyChange(() => CanCopy);
				NotifyOfPropertyChange(() => ExecuteButtonColor);
			}
		}

		/// <summary>
		/// Gets a list of all PathCheckBoxes
		/// </summary>
		public ObservableCollection<PathCheckBox> CheckBoxList
		{
			get
			{
				if (CheckBoxFilter != "")
					return new ObservableCollection<PathCheckBox>(_checkBoxList.Where(i => i.Content.ToString().ToLower().Contains(CheckBoxFilter.ToLower())).ToList<PathCheckBox>());
				else
					return _checkBoxList;
			}
		}

		/// <summary>
		/// Gets/sets the string for filtering paths.
		/// </summary>
		public string CheckBoxFilter
		{
			get { return _checkBoxFilter; }
			set
			{
				_checkBoxFilter = value;
				NotifyOfPropertyChange(() => CheckBoxList);
			}
		}

		/// <summary>
		/// Gets/sets wether subdirectories get included in select image path.
		/// </summary>
		public bool IncludeImageSubDirectories
		{
			get { return _includeImageSubDirectories; }
			set { _includeImageSubDirectories = value; }
		}

		/// <summary>
		/// Gets/sets wether subdirectories get included in select folder path.
		/// </summary>
		public bool IncludeFolderSubDirectories
		{
			get { return _includeFolderSubDirectories; }
			set { _includeFolderSubDirectories = value; }
		}

		/// <summary>
		/// The path to the folder with the images.
		/// </summary>
		public string ImagePath
		{
			get { return _imagePath; }
			private set
			{
				_imagePath = value;
				NotifyOfPropertyChange(() => ImagePath);
			}
		}

		/// <summary>
		/// The path to the folder of the folders.
		/// </summary>
		public string FolderPath
		{
			get { return _folderPath; }
			private set
			{
				_folderPath = value;
				NotifyOfPropertyChange(() => FolderPath);
				NotifyOfPropertyChange(() => CanAddFolder);
			}
		}

		/// <summary>
		/// Gets/sets wether the image should be deleted after copying.
		/// </summary>
		public bool DeleteImage
		{
			get { return _deleteImage; }
			set
			{
				_deleteImage = value;
				NotifyOfPropertyChange(() => CanCopy);
				NotifyOfPropertyChange(() => ExecuteButtonColor);
			}
		}

		/// <summary>
		/// Gets/sets wether the path checkBoxes should be reset after executing.
		/// </summary>
		public bool ResetCheckBoxes
		{
			get { return _resetCheckBoxes; }
			set { _resetCheckBoxes = value; }
		}

		/// <summary>
		/// Gets/sets the ignored paths.
		/// </summary>
		public ObservableCollection<string> IgnoredPaths
		{
			get { return _ignoredPaths; }
			private set { _ignoredPaths = value; }
		}

		/// <summary>
		/// The currently selected image in the list.
		/// </summary>
		public FileInfo SelectedImage
		{
			get { return _selectedImage; }
			set
			{
				_selectedImage = value;
				if (value != null)
				{
					_currentImageIndex = _imagePathList.IndexOf(SelectedImage);
					UpdatePictureBox();
				}
			}
		}

		/// <summary>
		/// Gets if the previous button is enabled.
		/// </summary>
		public bool CanPrevious
		{
			get { return _currentImageIndex != 0; }
		}

		/// <summary>
		/// Gets if the copy button is enabled.
		/// </summary>
		public bool CanCopy
		{
			get { return CurrentImage != null && (DeleteImage || _checkBoxList.Count(i => (bool)i.IsChecked) != 0); }
		}

		/// <summary>
		/// Gets if the next button is enabled.
		/// </summary>
		public bool CanNext
		{
			get { return _imagePathList.Count - 1 > _currentImageIndex; }
		}

		/// <summary>
		/// Gets wether the user can click the "Add Folder" button in
		/// the context menu of the <see cref="FolderPath"/> label.
		/// </summary>
		public bool CanAddFolder
		{
			get { return FolderPath != null && FolderPath != ""; }
		}

		/// <summary>
		/// Gets the color for the execute button.
		/// </summary>
		public SolidColorBrush ExecuteButtonColor
		{
			get
			{
				if (!CanCopy)
					return new SolidColorBrush(Colors.Gray);
				if (DeleteImage && _checkBoxList.Count(i => (bool)i.IsChecked) == 0)
					return new SolidColorBrush(Colors.Red);
				else
					return new SolidColorBrush(Colors.Green);
			}
		}

		#endregion

		/// <summary>
		/// Ctor.
		/// </summary>
		public MainViewModel()
		{
			_checkBoxFilter = "";
			_imagePathFilter = "";
			_imagePathList = new ObservableCollection<FileInfo>();
			_checkBoxList = new ObservableCollection<PathCheckBox>();
			IncludeFolderSubDirectories = true;
			DeleteImage = true;
			ResetCheckBoxes = true;
			LoadIgnoredPaths();
		}

		/// <summary>
		/// Loads the ignored paths from the text file.
		/// </summary>
		private void LoadIgnoredPaths()
		{
			IgnoredPaths = new ObservableCollection<string>();

			if (File.Exists("IgnoredPaths.txt"))
			{
				string[] paths = File.ReadAllLines("IgnoredPaths.txt");
				foreach (string path in paths)
				{
					IgnoredPaths.Add(path);
				}
			}
			else
				File.Create("IgnoredPaths.txt");
		}

		/// <summary>
		/// Lets the user select a folder with images and loads them.
		/// </summary>
		public void SelectImageFolder()
		{
			FolderBrowserDialog dlg = new FolderBrowserDialog();
			dlg.SelectedPath = SenpaiCopy.Properties.Settings.Default.LastSelectedImagePath;
			if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				ImagePath = dlg.SelectedPath;
				_imagePathList.Clear();

				System.IO.SearchOption so;
				if ((bool)IncludeImageSubDirectories)
					so = System.IO.SearchOption.AllDirectories;
				else
					so = System.IO.SearchOption.TopDirectoryOnly;

				//get all files
				string[] files = Directory.GetFiles(dlg.SelectedPath, "*", so);

				//get all image files
				foreach (string file in files)
				{
					string lowerFile = file.ToLower();
					if (lowerFile.EndsWith(".png") || lowerFile.EndsWith(".bmp") || lowerFile.EndsWith(".jpg") || lowerFile.EndsWith(".jpeg") || lowerFile.EndsWith(".gif"))
						_imagePathList.Add(new FileInfo(file));
				}

				SenpaiCopy.Properties.Settings.Default.LastSelectedImagePath = dlg.SelectedPath;
				SenpaiCopy.Properties.Settings.Default.Save();
			}

			UpdatePictureBox();
		}

		/// <summary>
		/// Opens a dialog to select a subfolder and creates checkBoxes for each path.
		/// </summary>
		public void SelectFolderPath()
		{
			FolderBrowserDialog dlg = new FolderBrowserDialog();
			dlg.SelectedPath = SenpaiCopy.Properties.Settings.Default.LastSelectedFolderPath;
			if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				_checkBoxList.Clear();

				FolderPath = dlg.SelectedPath;

				System.IO.SearchOption so;
				if ((bool)IncludeFolderSubDirectories)
					so = System.IO.SearchOption.AllDirectories;
				else
					so = System.IO.SearchOption.TopDirectoryOnly;

				string[] subfolders = Directory.GetDirectories(dlg.SelectedPath, "*", so);

				List<PathCheckBox> _checkBoxes = new List<PathCheckBox>();
				foreach (string folder in subfolders)
				{
					AddCheckBox(folder);
				}

				SenpaiCopy.Properties.Settings.Default.LastSelectedFolderPath = dlg.SelectedPath;
				SenpaiCopy.Properties.Settings.Default.Save();
				NotifyOfPropertyChange(() => ExecuteButtonColor);
			}
		}

		/// <summary>
		/// Adds a new CheckBox with the given <paramref name="folder"/> to the list.
		/// </summary>
		/// <param name="folder">Folder to add.</param>
		private void AddCheckBox(string folder)
		{
			if (!IgnoredPaths.Contains(folder))
			{
				PathCheckBox chk = new PathCheckBox();
				chk.Content = new DirectoryInfo(folder).Name;
				chk.FullPath = folder;
				chk.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
				chk.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
				chk.Margin = new Thickness(10, 0, 0, 0);
				chk.MouseRightButtonDown += new System.Windows.Input.MouseButtonEventHandler(CheckBox_RightMouseDown);
				chk.Checked += CheckBox_CheckedChanged;
				chk.Unchecked += CheckBox_CheckedChanged;
				chk.ToolTip = folder;
				_checkBoxList.Add(chk);
			}
		}

		/// <summary>
		/// Triggers when a CheckBox gets checked or unchecked.
		/// Checks if the execute button needs a different color.
		/// </summary>
		private void CheckBox_CheckedChanged(object sender, RoutedEventArgs e)
		{
			NotifyOfPropertyChange(() => ExecuteButtonColor);
		}

		/// <summary>
		/// Updates the currently shown image.
		/// </summary>
		private void UpdatePictureBox()
		{
			if (_imagePathList.Count > _currentImageIndex)
			{
				try
				{
					CurrentImage = LoadBitmapImage(_imagePathList[_currentImageIndex].FullName);
				}
				catch (OutOfMemoryException)
				{
					System.Windows.Forms.MessageBox.Show("The RAM is a lie. Folgendes Bild hat den Error verursacht: " + _imagePathList[_currentImageIndex].FullName + " | Wir skippen das Bild einfach mal.");
					if (CurrentImage != null) // TODO: check if still needed.
						CurrentImage = null;

					_currentImageIndex++;
					UpdatePictureBox();
				}
			}
			else // no more images.
			{
				CurrentImage = null;
				_currentImageIndex = 0;
			}

			NotifyOfPropertyChange(() => CanPrevious);
			NotifyOfPropertyChange(() => CanNext);
		}

		/// <summary>
		/// Copies the current image to the selected folders.
		/// </summary>
		public void Execute()
		{
			List<string> dirsToCopyTo = new List<string>();

			foreach (PathCheckBox c in _checkBoxList)
			{
				if ((bool)c.IsChecked)
					dirsToCopyTo.Add(c.FullPath);
			}

			foreach (string dir in dirsToCopyTo)
			{
				_imagePathList[_currentImageIndex].CopyTo(dir + @"\" + _imagePathList[_currentImageIndex].Name, true);
			}

			if (DeleteImage)
			{
				FileSystem.DeleteFile(_imagePathList[_currentImageIndex].FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
				_imagePathList.RemoveAt(_currentImageIndex);
			}
			else
				_currentImageIndex++;

			if (ResetCheckBoxes)
			{
				foreach (PathCheckBox c in _checkBoxList)
				{
					c.IsChecked = false;
				}
			}

			UpdatePictureBox();
		}

		/// <summary>
		/// Skips the current image doing nothing with it.
		/// </summary>
		public void Next()
		{
			_currentImageIndex++;
			UpdatePictureBox();
		}

		/// <summary>
		/// Goes to the previous image.
		/// </summary>
		public void Previous()
		{
			_currentImageIndex--;
			UpdatePictureBox();
		}

		/// <summary>
		/// Opens the current image in the standard image viewer.
		/// </summary>
		public void ShowImage()
		{
			Process.Start((CurrentImage as BitmapImage).UriSource.LocalPath);
		}

		/// <summary>
		/// Removes the selected checkBox.
		/// </summary>
		public void RemoveCheckBox()
		{
			_checkBoxList.Remove(_currentRightClickedCheckBox);
		}

		/// <summary>
		/// Adds the right clicked checkBox path to the <see cref="IgnoredPaths"/>
		/// </summary>
		public void IgnorePath()
		{
			File.AppendAllText("IgnoredPaths.txt", _currentRightClickedCheckBox.FullPath + "\r\n");
			IgnoredPaths.Add(_currentRightClickedCheckBox.FullPath);
			RemoveCheckBox();
		}

		/// <summary>
		/// Shows a dialog to select a folder to add to the list.
		/// </summary>
		public void AddFolder()
		{
			AddFolder(_currentRightClickedCheckBox.FullPath);
		}

		/// <summary>
		/// Shows a dialog to select a folder to add to the list,
		/// setting the dialog to the <paramref name="dialogStartingPath"/> by default.
		/// </summary>
		/// <param name="dialogStartingPath">Default selected path of the FolderBrowserDialog.</param>
		public void AddFolder(string dialogStartingPath)
		{
			FolderBrowserDialog dlg = new FolderBrowserDialog();
			dlg.SelectedPath = dialogStartingPath;
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				if (!_checkBoxList.Any(i => i.FullPath == dlg.SelectedPath))
					AddCheckBox(dlg.SelectedPath);
				else
					System.Windows.MessageBox.Show("This folder has already been added!");
			}
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

		/// <summary>
		/// Sets the current right clicked checkBox.
		/// </summary>
		private void CheckBox_RightMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			_currentRightClickedCheckBox = sender as PathCheckBox;
		}

		/// <summary>
		/// Opens the right clicked folder in the explorer.
		/// </summary>
		public void ShowPathInExplorer()
		{
			Process.Start(_currentRightClickedCheckBox.FullPath);
		}
	}
}