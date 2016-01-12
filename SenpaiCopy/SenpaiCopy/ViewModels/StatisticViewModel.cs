using Caliburn.Micro;
using System;
using System.Windows;

namespace SenpaiCopy
{
	/// <summary>
	/// ViewModel for the <see cref="StatisticView"/>.
	/// </summary>
	public class StatisticViewModel : PropertyChangedBase
	{
		#region Properties

		/// <summary>
		/// Gets/sets the total amount of deleted images.
		/// </summary>
		public int DeletedImages
		{
			get { return Properties.Settings.Default.DeletedImagesStatistic; }
			set
			{
				Properties.Settings.Default.DeletedImagesStatistic = value;
				Properties.Settings.Default.Save();
				NotifyOfPropertyChange(() => DeletedImages);
			}
		}

		/// <summary>
		/// Gets/sets the total size of deleted images.
		/// </summary>
		/// <remarks>
		/// Only images that only got deleted
		/// and not copied should count this up.
		/// </remarks>
		public double DeletedImagesSize
		{
			get { return Properties.Settings.Default.DeletedImagesSizeStatistic; }
			set
			{
				Properties.Settings.Default.DeletedImagesSizeStatistic = Math.Round(value, 2);
				Properties.Settings.Default.Save();
				NotifyOfPropertyChange(() => DeletedImagesSize);
			}
		}

		/// <summary>
		/// Gets/sets the total amount of copied images.
		/// </summary>
		public int CopiedImages
		{
			get { return Properties.Settings.Default.CopiedImagesStatistic; }
			set
			{
				Properties.Settings.Default.CopiedImagesStatistic = value;
				Properties.Settings.Default.Save();
				NotifyOfPropertyChange(() => CopiedImages);
			}
		}

		/// <summary>
		/// Gets/sets the total size of copied images.
		/// </summary>
		public double TotalCopiedImagesSize
		{
			get { return Properties.Settings.Default.CopiedImagesSizeStatistic; }
			set
			{
				Properties.Settings.Default.CopiedImagesSizeStatistic = Math.Round(value, 2);
				Properties.Settings.Default.Save();
				NotifyOfPropertyChange(() => TotalCopiedImagesSize);
			}
		}

		/// <summary>
		/// Gets/sets the total amount of copies made.
		/// </summary>
		public int TotalCopiedImages
		{
			get { return Properties.Settings.Default.TotalCopiedImagesStatistic; }
			set
			{
				Properties.Settings.Default.TotalCopiedImagesStatistic = value;
				Properties.Settings.Default.Save();
				NotifyOfPropertyChange(() => TotalCopiedImages);
			}
		}

		/// <summary>
		/// Gets/sets the number of startups.
		/// </summary>
		public int Startups
		{
			get { return Properties.Settings.Default.StartupStatistic; }
			set
			{
				Properties.Settings.Default.StartupStatistic = value;
				Properties.Settings.Default.Save();
				NotifyOfPropertyChange(() => Startups);
			}
		}

		#endregion Properties

		/// <summary>
		/// Resets the whole statistic back to zero.
		/// </summary>
		public void ResetStatistic()
		{
			if (MessageBox.Show("Do you really want to reset all statistics?", "Reset Statistic", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
			{
				DeletedImages = 0;
				DeletedImagesSize = 0.0;
				CopiedImages = 0;
				TotalCopiedImagesSize = 0.0;
				Startups = 0;
			}
		}
	}
}