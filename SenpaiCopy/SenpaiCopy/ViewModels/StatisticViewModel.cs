using Caliburn.Micro;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SenpaiCopy
{
	/// <summary>
	/// Helper class that holds statistic data.
	/// Used for serializing/deserializing;
	/// </summary>
	public class Statistics
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
			}
		}

		#endregion Properties
	}

	/// <summary>
	/// ViewModel for the <see cref="StatisticView"/>.
	/// </summary>
	public class StatisticViewModel : PropertyChangedBase
	{
		private Statistics _statistics = new Statistics();

		#region Properties

		/// <summary>
		/// Gets/sets the total amount of deleted images.
		/// </summary>
		public int DeletedImages
		{
			get { return _statistics.DeletedImages; }
			set
			{
				_statistics.DeletedImages = value;
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
			get { return _statistics.DeletedImagesSize; }
			set
			{
				_statistics.DeletedImagesSize = Math.Round(value, 2);
				NotifyOfPropertyChange(() => DeletedImagesSize);
			}
		}

		/// <summary>
		/// Gets/sets the total amount of copied images.
		/// </summary>
		public int CopiedImages
		{
			get { return _statistics.CopiedImages; }
			set
			{
				_statistics.CopiedImages = value;
				NotifyOfPropertyChange(() => CopiedImages);
			}
		}

		/// <summary>
		/// Gets/sets the total size of copied images.
		/// </summary>
		public double TotalCopiedImagesSize
		{
			get { return _statistics.TotalCopiedImagesSize; }
			set
			{
				_statistics.TotalCopiedImagesSize = Math.Round(value, 2);
				NotifyOfPropertyChange(() => TotalCopiedImagesSize);
			}
		}

		/// <summary>
		/// Gets/sets the total amount of copies made.
		/// </summary>
		public int TotalCopiedImages
		{
			get { return _statistics.TotalCopiedImages; }
			set
			{
				_statistics.TotalCopiedImages = value;
				NotifyOfPropertyChange(() => TotalCopiedImages);
			}
		}

		/// <summary>
		/// Gets/sets the number of startups.
		/// </summary>
		public int Startups
		{
			get { return _statistics.Startups; }
			set
			{
				_statistics.Startups = value;
				NotifyOfPropertyChange(() => Startups);
			}
		}

		#endregion Properties

		/// <summary>
		/// Resets the whole statistic back to zero.
		/// </summary>
		public void ResetStatistic()
		{
			if (System.Windows.MessageBox.Show("Do you really want to reset all statistics?", "Reset Statistic", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
			{
				DeletedImages = 0;
				DeletedImagesSize = 0.0;
				CopiedImages = 0;
				TotalCopiedImagesSize = 0.0;
				Startups = 0;
			}
		}

		/// <summary>
		/// Exports the current statistics as xml file.
		/// </summary>
		public void ExportStatistic()
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "XML Files|*.xml";
			if(sfd.ShowDialog() == DialogResult.OK)
			{
				try
				{
					XmlSerializer serializer = new XmlSerializer(typeof(Statistics));
					using (StreamWriter sw = new StreamWriter(sfd.FileName))
					{
						serializer.Serialize(sw, _statistics);
					};
				}
				catch(Exception ex)
				{
					System.Windows.MessageBox.Show("Error while exporting statistics: " + ex.Message);
				}
			}
		}

		/// <summary>
		/// Loads an xml and deserializes it to an <see cref="Statistics"/> object.
		/// </summary>
		public void ImportStatistic()
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "XML Files|*.xml";
			if(ofd.ShowDialog() == DialogResult.OK)
			{
				try
				{
					XmlSerializer serializer = new XmlSerializer(typeof(Statistics));
					using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
					{
						_statistics = (Statistics)serializer.Deserialize(fs);

						// message all props that the statistics object has changed
						NotifyOfPropertyChange(() => DeletedImages);
						NotifyOfPropertyChange(() => DeletedImagesSize);
						NotifyOfPropertyChange(() => CopiedImages);
						NotifyOfPropertyChange(() => TotalCopiedImagesSize);
						NotifyOfPropertyChange(() => TotalCopiedImages);
						NotifyOfPropertyChange(() => Startups);
					};
				}
				catch(Exception ex)
				{
					System.Windows.MessageBox.Show("Error while importing statistics: " + ex.Message);
				}
			}
		}
	}
}