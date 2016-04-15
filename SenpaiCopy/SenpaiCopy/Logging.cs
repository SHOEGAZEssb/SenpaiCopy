using System;
using System.IO;

namespace SenpaiCopy
{
	/// <summary>
	/// Class used for logging certain infos.
	/// </summary>
	static class Logging
	{
		private static readonly object _lockAnchor = new object();

		/// <summary>
		/// Constructor.
		/// Creates log.txt if it doesn't exist.
		/// </summary>
		static Logging()
		{
			lock (_lockAnchor)
			{
				if (!File.Exists("log.txt"))
					File.Create("log.txt").Close();
			}
		}

		/// <summary>
		/// Writes the specified <paramref name="info"/>
		/// into the logfile.
		/// </summary>
		/// <param name="info">Info to log.</param>
		public static void LogInfo(string info)
		{
			lock (_lockAnchor)
			{
				File.AppendAllLines("log.txt", new[] { DateTime.Now + ": " + info });
			}
		}
	}
}