// -----------------------------------------------------------------------
// <copyright file="ChangeInterceptor.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Asana.Core.Proxy
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;

	using Castle.DynamicProxy;

	/// <summary>
	/// 	TODO: Update summary.
	/// </summary>
	public class ChangeInterceptor : IInterceptor, IChangeable
	{
		#region Constructors and Destructors

		public ChangeInterceptor()
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



		public void Intercept(IInvocation invocation)
		{
			if (this.TrackChanges)
			{
				const string SetPrefix = "set_";
				if (invocation.Method.Name.StartsWith(SetPrefix, StringComparison.OrdinalIgnoreCase)
				    && invocation.Arguments.Length == 1)
				{
					var name = invocation.Method.Name.Substring(SetPrefix.Length);
					var value = invocation.Arguments[0];
					this.Changes[name] = value;
				}
			}

			invocation.Proceed();
		}

		public void Reset()
		{
			this.Changes.Clear();
		}

		#endregion
	}
}