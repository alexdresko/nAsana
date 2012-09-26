namespace Asana.Core.Proxy
{
	using System.Collections.Generic;

	public interface IChangeable
	{
		bool TrackChanges { get; set; }

		Dictionary<string, object> Changes { get; set; }

		bool IsDirty { get; set; }

		void Reset();
	}
}