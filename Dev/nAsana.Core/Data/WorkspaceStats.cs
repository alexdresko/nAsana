namespace Asana.Core.Data
{
	using System.Collections.Generic;

	public class WorkspaceStats
	{
		#region Public Properties

		public IEnumerable<WorkplaceStat> Stats { get; set; }

		#endregion
	}
}