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
	public class ChangeInterceptor : IInterceptor
	{
		public Changeable Changeable { get; set; }

		#region Constructors and Destructors

		public ChangeInterceptor(Changeable changeable)
		{
			Changeable = changeable;
		}

		#endregion

		#region Public Properties


		#endregion

		#region Public Methods and Operators



		public void Intercept(IInvocation invocation)
		{
			if (this.Changeable.TrackChanges)
			{
				const string SetPrefix = "set_";
				if (invocation.Method.Name.StartsWith(SetPrefix, StringComparison.OrdinalIgnoreCase)
				    && invocation.Arguments.Length == 1)
				{
					var name = invocation.Method.Name.Substring(SetPrefix.Length);
					var value = invocation.Arguments[0];
					this.Changeable.Changes[name] = value;
				}
			}

			invocation.Proceed();
		}


		#endregion
	}
}