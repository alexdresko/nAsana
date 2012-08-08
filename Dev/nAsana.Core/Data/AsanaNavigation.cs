namespace Asana.Core.Data
{
	using System.Collections.Generic;

	public class AsanaNavigation
	{
		#region Public Properties

		public long Id { get; set; }

		public string Name { get; set; }

		public List<AsanaProject> Projects { get; set; }

		#endregion
	}
}