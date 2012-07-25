// ReSharper disable InconsistentNaming

namespace nAsana.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using nAsana.Core.Data;

	/// <summary>
	/// 	Summary description for AsanaRepositoryTests
	/// </summary>
	[TestClass]
	public class AsanaRepositoryTests
	{
		#region Public Methods and Operators

		[TestMethod]
		public void CreateProject_GivenSampleDataAndTestWorkspace_CreatesProject()
		{
			var existingProjectCount = GetTestWorkspaceProjects().Count;

			var returnedProject = CreateTestProject();

			Assert.IsNotNull(returnedProject);

			Assert.AreEqual(existingProjectCount + 1, GetTestWorkspaceProjects().Count);
		}

		[TestMethod]
		public void GetProjects_GivenTestWorkspace_ReturnsProjects()
		{
			var projects = GetTestWorkspaceProjects();

			Assert.IsNotNull(projects);

			Assert.IsTrue(projects.Count() > 0);
		}

		[TestMethod]
		public void GetUsers_ReturnsUsers()
		{
			var users = this.GetUsers();

			Assert.IsNotNull(users);

			Assert.IsTrue(users.Count > 0);
		}

		[TestMethod]
		public void GetWorkspaces_ReturnsWorkspaces()
		{
			var workspaces = GetWorkspaces();

			Assert.IsNotNull(workspaces);

			Assert.IsTrue(workspaces.Count > 0);
		}

		[TestMethod]
		public void UpdateProject_GivenValidProject_UpdatesProject()
		{
			var projectToUpdate = CreateTestProject();

			projectToUpdate.notes = "Some new notes";

			var updatedProject = UpdateProject(projectToUpdate);
			Assert.AreEqual(projectToUpdate.notes, updatedProject.notes);
		}

		#endregion

		#region Methods

		private static void AssertValidResponse<T>(AsanaResponse<T> response)
		{
			if (response.StatusCode == HttpStatusCode.BadRequest)
			{
				Assert.Fail(
					string.Format("{0} response returned {1}", typeof(T), string.Join(", ", response.Errors.Select(p => p.message))));
			}
		}

		private static AsanaProject CreateTestProject()
		{
			var newProject = new AsanaProject { name = Guid.NewGuid().ToString() };

			var repository = new AsanaRespository();

			var testWorkspace = GetTestWorkspace();
			var returnedProject = repository.CreateProject(testWorkspace, newProject);
			AssertValidResponse(returnedProject);
			return returnedProject.Data;
		}

		private static AsanaWorkspace GetTestWorkspace()
		{
			var repository = new AsanaRespository();
			var asanaResponse = repository.GetWorkspaces();
			AssertValidResponse(asanaResponse);
			var workspace = asanaResponse.Data.Single(p => p.name.ToLower() == "api tests");
			return workspace;
		}

		private static List<AsanaProject> GetTestWorkspaceProjects()
		{
			var repository = new AsanaRespository();
			var workspace = GetTestWorkspace();
			var projects = repository.GetProjects(workspace.id, null);
			AssertValidResponse(projects);
			return projects.Data;
		}

		private static List<AsanaWorkspace> GetWorkspaces()
		{
			var repository = new AsanaRespository();
			var asanaResponse = repository.GetWorkspaces();
			AssertValidResponse(asanaResponse);
			var workspaces = asanaResponse.Data;
			return workspaces;
		}

		private static AsanaProject UpdateProject(AsanaProject projectToUpdate)
		{
			var repository = new AsanaRespository();
			var response = repository.UpdateProject(projectToUpdate);
			AssertValidResponse(response);
			return response.Data;
		}

		private List<AsanaUser> GetUsers()
		{
			var repository = new AsanaRespository();
			var asanaResponse = repository.GetUsers();
			AssertValidResponse(asanaResponse);
			var users = asanaResponse.Data;
			return users;
		}

		#endregion
	}
}