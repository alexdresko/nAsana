namespace Asana.Core.Data
{
	using System.Collections.Generic;

	using RestSharp;

	public class AsanaResponse<T> : RestResponse
	{
		#region Public Properties

		public T Data { get; set; }

		public List<AsanaError> Errors { get; set; }

		#endregion
	}

	public class AsanaError
	{
		#region Public Properties

		public string message { get; set; }

		#endregion
	}
}