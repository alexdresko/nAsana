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
		[TestMethod]
		public void GetTask_IsIChangeable()
		{
			var task = GetTask();
			task.completed = true;

			Assert.IsTrue(task is IChangeable);
		}

		[TestMethod]
		public void GetTask_ByDefault_HasNoChanges()
		{
			var task = GetTask();
			var change = task as IChangeable;

			Assert.AreEqual(0, change.Changes.Count);
			Assert.IsFalse(change.IsDirty);
			Assert.IsTrue(change.TrackChanges);
		}

		[TestMethod]
		public void ChangingProperty_CreatesChange()
		{
			var task = GetTask();
			task.completed = true;
			var change = task as IChangeable;

			Assert.AreEqual(1, change.Changes.Count);
			Assert.IsTrue(change.Changes.ContainsKey("completed"));
			Assert.IsTrue((bool)change.Changes["completed"]);
		}

		private static AsanaTask GetTask()
		{
			//var options = new ProxyGenerationOptions();
			//var changeable = new Changeable();
			//options.AddMixinInstance(changeable);
			//var task = Generator.Instance.CreateClassProxy<AsanaTask>(options, new ChangeInterceptor(changeable));
			
			//changeable.TrackChanges = true;
			//return task;

			return new AsanaTask();
		}
	}
}
