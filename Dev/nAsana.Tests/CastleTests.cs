// ReSharper disable InconsistentNaming
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace nAsana.Tests
{
	using Asana.Core.Data;
	using Asana.Core.Proxy;

	using Castle.DynamicProxy;

	[TestClass]
	public class CastleTests
	{
		private static readonly ProxyGenerator _generator = new ProxyGenerator();

		[TestMethod]
		public void TestMethod1()
		{
			var task = _generator.CreateClassProxy<AsanaTask>(new StandardInterceptor(), new ChangeInterceptor());
			task.completed = true;

		}
	}
}
