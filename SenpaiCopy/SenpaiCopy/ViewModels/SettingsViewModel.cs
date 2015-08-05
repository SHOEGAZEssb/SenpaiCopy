using Caliburn.Micro;
using System.Windows;
using System.Windows.Input;

namespace SenpaiCopy
{
	/// <summary>
	/// ViewModel for the <see cref="SettingsView"/>.
	/// </summary>
	class SettingsViewModel : PropertyChangedBase
	{
		#region Member

		private Key _previousHotkey;
		private Key _executeHotkey;
		private Key _nextHotkey;
		private Key _clearCheckBoxesHotkey;

		#endregion

		#region Properties

		/// <summary>
		/// The hotkey to go to the previous image.
		/// </summary>
		public Key PreviousHotkey
		{
			get { return _previousHotkey; }
			set { _previousHotkey = value; }
		}

		/// <summary>
		/// The hotkey to execute.
		/// </summary>
		public Key ExecuteHotkey
		{
			get { return _executeHotkey; }
			set { _executeHotkey = value; }
		}

		/// <summary>
		/// The hotkey to go to the next image.
		/// </summary>
		public Key NextHotkey
		{
			get { return _nextHotkey; }
			set { _nextHotkey = value; }
		}

		/// <summary>
		/// The hotkey to clear all CheckBoxes.
		/// </summary>
		public Key ClearCheckBoxesHotkey
		{
			get { return _clearCheckBoxesHotkey; }
			set { _clearCheckBoxesHotkey = value; }
		}

		#endregion

		/// <summary>
		/// Ctor.
		/// </summary>
		public SettingsViewModel()
		{
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
			NotifyOfPropertyChange(() => PreviousHotkey);
			NotifyOfPropertyChange(() => ExecuteHotkey);
			NotifyOfPropertyChange(() => NextHotkey);
			NotifyOfPropertyChange(() => ClearCheckBoxesHotkey);
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
			NotifyOfPropertyChange(() => PreviousHotkey);
			NotifyOfPropertyChange(() => ExecuteHotkey);
			NotifyOfPropertyChange(() => NextHotkey);
			NotifyOfPropertyChange(() => ClearCheckBoxesHotkey);
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