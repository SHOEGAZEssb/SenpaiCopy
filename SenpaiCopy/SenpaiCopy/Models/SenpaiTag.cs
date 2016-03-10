namespace SenpaiCopy.Models
{
	/// <summary>
	/// Represents a tag of a <see cref="SenpaiDirectory"/>.
	/// </summary>
	public class SenpaiTag
	{
		#region Properties

		/// <summary>
		/// Get/sets the name of this tag.
		/// </summary>
		public string Name
		{
			get { return _name; }
			private set { _name = value; }
		}
		private string _name;

		#endregion Properties

		/// <summary>
		/// Ctor.
		/// </summary>
		/// <param name="name">Name of this tag.</param>
		public SenpaiTag(string name)
		{
			Name = name;
		}
	}
}