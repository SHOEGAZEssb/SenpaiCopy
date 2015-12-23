using Caliburn.Micro;
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
	internal class SettingsViewModel : PropertyChangedBase
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
		/// Supported formats.
		/// </summary>
		public ObservableCollection<string> SupportedFormats
		{
			get { return _supportedFormats; }
			private set { _supportedFormats = value; }
		}
		private ObservableCollection<string> _supportedFormats;

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
    /// Gets/sets if a message box should pop up, if a file already exists.
    /// </summary>
    public bool WarnIfOverwrite
    {
      get { return _warnIfOverwrite; }
      set
      {
        _warnIfOverwrite = value;
        NotifyOfPropertyChange(() => WarnIfOverwrite);
      }
    }
    private bool _warnIfOverwrite;

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
			PreviousHotkey = (Key)Properties.Settings.Default.PreviousHotkey;
			ExecuteHotkey = (Key)Properties.Settings.Default.ExecuteHotkey;
			NextHotkey = (Key)Properties.Settings.Default.NextHotkey;
			ClearCheckBoxesHotkey = (Key)Properties.Settings.Default.ClearCheckBoxesHotkey;

			EnabledFormats = new ObservableCollection<string>(Properties.Settings.Default.EnabledFormats.Split(';').OrderBy(i => i));
			List<string> tempSupportedFormats = new List<string>(Properties.Settings.Default.SupportedFormats.Split(';'));
			SupportedFormats = new ObservableCollection<string>(tempSupportedFormats.Where(i => !EnabledFormats.Contains(i)).OrderBy(i => i));

      SendToRecycleBin = Properties.Settings.Default.SendToRecycleBin;
      WarnIfOverwrite = Properties.Settings.Default.WarnIfOverwrite;
			EnableStatisticTracking = Properties.Settings.Default.EnableStatisticTracking;
		}

		/// <summary>
		/// Adds the selected supported format to the <see cref="EnabledFormats"/>.
		/// </summary>
		public void AddToEnabled()
		{
			EnabledFormats.Add(SupportedFormats[SupportedIndex]);
			SupportedFormats.RemoveAt(SupportedIndex);
		}

		/// <summary>
		/// Adds the selected enabled format to the <see cref="SupportedFormats"/>.
		/// </summary>
		public void AddToSupported()
		{
			SupportedFormats.Add(EnabledFormats[EnabledIndex]);
			EnabledFormats.RemoveAt(EnabledIndex);
		}

		/// <summary>
		/// Save settings and close the window.
		/// </summary>
		/// <param name="sender">Should be the <see cref="SettingsView"/>, but is the clicked button for some reason.</param>
		public void Save(object sender)
		{
			Properties.Settings.Default.PreviousHotkey = (int)PreviousHotkey;
			Properties.Settings.Default.ExecuteHotkey = (int)ExecuteHotkey;
			Properties.Settings.Default.NextHotkey = (int)NextHotkey;
			Properties.Settings.Default.ClearCheckBoxesHotkey = (int)ClearCheckBoxesHotkey;

			Properties.Settings.Default.EnabledFormats = string.Join(";", EnabledFormats.Select(i => i.ToString()).ToArray());

      Properties.Settings.Default.SendToRecycleBin = SendToRecycleBin;
      Properties.Settings.Default.WarnIfOverwrite = WarnIfOverwrite;
			Properties.Settings.Default.EnableStatisticTracking = EnableStatisticTracking;

			Properties.Settings.Default.Save();
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