using Caliburn.Micro;
using SenpaiCopy.Properties;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SenpaiCopy
{
	/// <summary>
	/// ViewModel for the <see cref="SettingsView"/>.
	/// </summary>
	public class SettingsViewModel : PropertyChangedBase
	{
		#region Properties

		/// <summary>
		/// The hotkey to go to the previous image.
		/// </summary>
		public Key PreviousHotkey
		{
			get { return _previousHotkey; }
			set
			{
				_previousHotkey = value;
				NotifyOfPropertyChange(() => PreviousHotkey);
			}
		}
		private Key _previousHotkey;

		/// <summary>
		/// The hotkey to execute.
		/// </summary>
		public Key ExecuteHotkey
		{
			get { return _executeHotkey; }
			set
			{
				_executeHotkey = value;
				NotifyOfPropertyChange(() => ExecuteHotkey);
			}
		}
		private Key _executeHotkey;

		/// <summary>
		/// The hotkey to go to the next image.
		/// </summary>
		public Key NextHotkey
		{
			get { return _nextHotkey; }
			set
			{
				_nextHotkey = value;
				NotifyOfPropertyChange(() => NextHotkey);
			}
		}
		private Key _nextHotkey;

		/// <summary>
		/// The hotkey to clear all CheckBoxes.
		/// </summary>
		public Key ClearCheckBoxesHotkey
		{
			get { return _clearCheckBoxesHotkey; }
			set
			{
				_clearCheckBoxesHotkey = value;
				NotifyOfPropertyChange(() => ClearCheckBoxesHotkey);
			}
		}
		private Key _clearCheckBoxesHotkey;

		/// <summary>
		/// The enabled formats.
		/// </summary>
		public ObservableCollection<string> EnabledFormats
		{
			get { return _enabledFormats; }
			private set { _enabledFormats = value; }
		}
		private ObservableCollection<string> _enabledFormats;

		/// <summary>
		/// Gets/sets a list with supported image formats.
		/// </summary>
		public List<string> SupportedImageFormats
		{
			get { return _supportedImageFormats; }
			private set
			{
				_supportedImageFormats = value;
				NotifyOfPropertyChange(() => SupportedFormats);
			}
		}
		private List<string> _supportedImageFormats;

		/// <summary>
		/// Gets/sets a list with supported formats that require to be played by the VlcPlayer.
		/// </summary>
		public List<string> SupportedVlcFormats
		{
			get { return _supportedVlcFormats; }
			private set
			{
				_supportedVlcFormats = value;
				NotifyOfPropertyChange(() => SupportedFormats);
			}
		}
		private List<string> _supportedVlcFormats;

		/// <summary>
		/// Index of the selected enabled format.
		/// </summary>
		public int EnabledIndex
		{
			get { return _enabledIndex; }
			set
			{
				_enabledIndex = value;
				NotifyOfPropertyChange(() => CanAddToSupported);
			}
		}
		private int _enabledIndex;

		/// <summary>
		/// Index of the selected supported format.
		/// </summary>
		public int SupportedIndex
		{
			get { return _supportedIndex; }
			set
			{
				_supportedIndex = value;
				NotifyOfPropertyChange(() => CanAddToEnabled);
			}
		}
		private int _supportedIndex;

		/// <summary>
		/// Gets/sets if deleted files should be sent to the recycle bin.
		/// </summary>
		public bool SendToRecycleBin
		{
			get { return _sendToRecycleBin; }
			set
			{
				_sendToRecycleBin = value;
				NotifyOfPropertyChange(() => SendToRecycleBin);
			}
		}
		private bool _sendToRecycleBin;

		/// <summary>
		/// Gets/sets if files that already exist in the target folder
		/// should be overwritten. If not, the source file gets renamed
		/// before copying.
		/// </summary>
		public bool OverwriteFiles
		{
			get { return _overwriteFiles; }
			set
			{
				_overwriteFiles = value;
				NotifyOfPropertyChange(() => OverwriteFiles);
			}
		}
		private bool _overwriteFiles;

		/// <summary>
		/// Gets/sets if statistics should be tracked.
		/// </summary>
		public bool EnableStatisticTracking
		{
			get { return _enableStatisticTracking; }
			set
			{
				_enableStatisticTracking = value;
				NotifyOfPropertyChange(() => EnableStatisticTracking);
			}
		}
		private bool _enableStatisticTracking;

		/// <summary>
		/// Gets/sets if logging is enabled.
		/// </summary>
		public bool EnableLogging
		{
			get { return _enableLogging; }
			set
			{
				_enableLogging = value;
				NotifyOfPropertyChange(() => EnableLogging);
			}
		}
		private bool _enableLogging;

		#region Read-Only Properties

		/// <summary>
		/// Gets whether its possible to add to the enabled formats.
		/// </summary>
		public bool CanAddToEnabled
		{
			get { return SupportedIndex != -1; }
		}

		/// <summary>
		/// Gets whether its possible to add to the supported formats.
		/// </summary>
		public bool CanAddToSupported
		{
			get { return EnabledIndex != -1; }
		}

		/// <summary>
		/// Supported formats.
		/// </summary>
		public ObservableCollection<string> SupportedFormats
		{
			get { return new ObservableCollection<string>(SupportedImageFormats.Concat(SupportedVlcFormats).Where(i => !EnabledFormats.Contains(i)).OrderBy(i => i)); }
		}

		#endregion Read-Only Properties

		#endregion Properties

		/// <summary>
		/// Ctor.
		/// </summary>
		public SettingsViewModel()
		{
			SupportedIndex = -1;
			EnabledIndex = -1;
			LoadSettings();
		}

		/// <summary>
		/// Loads the saved settings.
		/// </summary>
		private void LoadSettings()
		{
			PreviousHotkey = (Key)Settings.Default.PreviousHotkey;
			ExecuteHotkey = (Key)Settings.Default.ExecuteHotkey;
			NextHotkey = (Key)Settings.Default.NextHotkey;
			ClearCheckBoxesHotkey = (Key)Settings.Default.ClearCheckBoxesHotkey;

			EnabledFormats = new ObservableCollection<string>(Settings.Default.EnabledFormats.Split(';').OrderBy(i => i));
			SupportedImageFormats = new List<string>(Settings.Default.SupportedImageFormats.Split(';'));
			SupportedVlcFormats = new List<string>(Settings.Default.SupportedVlcFormats.Split(';'));

			SendToRecycleBin = Settings.Default.SendToRecycleBin;
			OverwriteFiles = Settings.Default.WarnIfOverwrite;
			EnableStatisticTracking = Settings.Default.EnableStatisticTracking;
			EnableLogging = Settings.Default.EnableLogging;
		}

		/// <summary>
		/// Adds the selected supported format to the <see cref="EnabledFormats"/>.
		/// </summary>
		public void AddToEnabled()
		{
			EnabledFormats.Add(SupportedFormats[SupportedIndex]);
			NotifyOfPropertyChange(() => SupportedFormats);
		}

		/// <summary>
		/// Adds the selected enabled format to the <see cref="SupportedFormats"/>.
		/// </summary>
		public void AddToSupported()
		{
			EnabledFormats.RemoveAt(EnabledIndex);
			NotifyOfPropertyChange(() => SupportedFormats);
		}

		/// <summary>
		/// Save settings and close the window.
		/// </summary>
		/// <param name="sender">Should be the <see cref="SettingsView"/>, but is the clicked button for some reason.</param>
		public void Save(object sender)
		{
			Settings.Default.PreviousHotkey = (int)PreviousHotkey;
			Settings.Default.ExecuteHotkey = (int)ExecuteHotkey;
			Settings.Default.NextHotkey = (int)NextHotkey;
			Settings.Default.ClearCheckBoxesHotkey = (int)ClearCheckBoxesHotkey;

			Settings.Default.EnabledFormats = string.Join(";", EnabledFormats.Select(i => i.ToString()).ToArray());

			Settings.Default.SendToRecycleBin = SendToRecycleBin;
			Settings.Default.WarnIfOverwrite = OverwriteFiles;
			Settings.Default.EnableStatisticTracking = EnableStatisticTracking;
			Settings.Default.EnableLogging = EnableLogging;

			Settings.Default.Save();
			(((sender as FrameworkElement).Parent as FrameworkElement).Parent as Window).Close(); //TODO ULTRA HACKY CHECK THIS!!!!!
		}

		/// <summary>
		/// Restore settings and close the window.
		/// </summary>
		/// <param name="sender">Should be the <see cref="SettingsView"/>, but is the clicked button for some reason.</param>
		public void Cancel(object sender)
		{
			LoadSettings();
			(((sender as FrameworkElement).Parent as FrameworkElement).Parent as Window).Close(); //TODO ULTRA HACKY CHECK THIS!!!!!
		}
	}
}