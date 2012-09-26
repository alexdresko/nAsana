namespace Asana.Core.Proxy
{
	using System.Collections.Generic;

	public class Changeable : IChangeable
	{
		#region Constructors and Destructors

		public Changeable()
		{
			this.Changes = new Dictionary<string, object>();
		}

		#endregion

		#region Public Properties

		public Dictionary<string, object> Changes { get; set; }

		public bool IsDirty { get; set; }

		public bool TrackChanges { get; set; }

		#endregion

		#region Public Methods and Operators

		public void Reset()
		{
			this.Changes.Clear();
		}

		#endregion
	}
}