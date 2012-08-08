// -----------------------------------------------------------------------
// <copyright file="AsanaRespository.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Asana.Core.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;

	using Newtonsoft.Json;

	using RestSharp;

	/// <summary>
	/// 	TODO: Update summary.
	/// </summary>
	public class AsanaRespository
	{
		#region Constants

		private const string ApiKey = "dYRZlbE.9TE1BmGSEyvaVTJTLn05bLiG";

		#endregion

		#region Static Fields

		private static readonly string _asanaBaseUrl;

		#endregion

		#region Constructors and Destructors

		static AsanaRespository()
		{
			_asanaBaseUrl = "https://app.asana.com/api/1.0/";
		}

		#endregion

		#region Public Methods and Operators

		public AsanaResponse<AsanaProject> CreateProject(AsanaWorkspace workspace, AsanaProject newProject)
		{
			return UseRequest<AsanaProject>(
				GetWorkspaceProjectsUrl(workspace.id), 
				request =>
					{
						newProject.workspaceid = workspace.id;
						request.Method = Method.POST;
						request.AddObject(newProject);
					});
		}

		public List<AsanaNavigation> GetAsanaNav()
		{
			var workspaces = this.GetWorkspaces();
			var results =
				workspaces.Data.Select(
					p => new AsanaNavigation { Name = p.name, Id = p.id, Projects = this.GetProjects(p.id, null).Data }).ToList();

			return results;
		}

		public AsanaResponse<List<AsanaProject>> GetProjects(long workspaceId, List<string> optFields)
		{
			var url = GetWorkspaceProjectsUrl(workspaceId, optFields);

			return UseRequest<List<AsanaProject>>(url, request => { });
		}

		public AsanaResponse<List<AsanaTask>> GetTasks(long id)
		{
			return this.GetTasks(id, null);
		}

		public AsanaResponse<List<AsanaUser>> GetUsers()
		{
			return GetData<List<AsanaUser>>("users");
		}

		public List<AsanaProject> GetWorkspaceProjectsAndTasks(long id)
		{
			var optFields = new List<string>
				{
       "created_at", "name", "archived", "modified_at", "notes", "workspaceid", "archived", "followers" 
    };
			var projects = this.GetProjects(id, optFields).Data;

			projects.ForEach(
				p =>
				p.Tasks =
				this.GetTasks(p.id, new List<string> { "created_at", "modified_at", "due_on", "completed", "followers", "name" }).
					Data);
			return projects;
		}

		public WorkspaceStats GetWorkspaceStats(long id)
		{
			var projects = this.GetWorkspaceProjectsAndTasks(id);

			var results = new WorkspaceStats();

			var stats = projects.Where(f => !f.archived).Select(
				project =>
					{
						var workplaceStat = new WorkplaceStat();
						workplaceStat.archived = project.archived;
						workplaceStat.created_at = project.created_at;
						workplaceStat.id = project.id;
						workplaceStat.modified_at = project.modified_at;
						workplaceStat.name = project.name;
						workplaceStat.notes = project.notes;
						workplaceStat.workspaceid = project.workspaceid;
						workplaceStat.TotalTasks = project.Tasks.Count;
						workplaceStat.TasksCompleted = project.Tasks.Count(p => p.completed);
						workplaceStat.OldestTask =
							project.Tasks.OrderByDescending(p => p.created_at).Select(p => p.created_at).FirstOrDefault();
						workplaceStat.NewestTask = project.Tasks.OrderBy(p => p.created_at).Select(p => p.created_at).FirstOrDefault();
						workplaceStat.PastDueTasks = project.Tasks.Count(p => p.due_on.HasValue && p.due_on.Value <= DateTime.Now);
						workplaceStat.TaskFollowers = project.Tasks.SelectMany(p => p.followers).Select(f => f.id).Distinct().Count();
						workplaceStat.ProjectFollowers = project.followers.Count();
						workplaceStat.DueToday = project.Tasks.Count(p => p.due_on.HasValue && p.due_on.Value.Date == DateTime.Now.Date);
						workplaceStat.NotDueYet = project.Tasks.Count(p => p.due_on.HasValue && p.due_on.Value >= DateTime.Now);
						workplaceStat.NeverDue = project.Tasks.Count(p => !p.due_on.HasValue);
						workplaceStat.LastModified =
							project.Tasks.OrderByDescending(p => p.modified_at).Select(p => p.modified_at).FirstOrDefault();
						workplaceStat.PastDueDays =
							project.Tasks.Where(p => p.due_on.HasValue && p.due_on.Value <= DateTime.Now).Sum(
								p => (DateTime.Now - p.due_on).Value.TotalDays);
						return workplaceStat;
					});

			results.Stats = stats.ToList();
			return results;
		}

		public AsanaResponse<List<AsanaWorkspace>> GetWorkspaces()
		{
			return GetData<List<AsanaWorkspace>>("workspaces");
		}

		public AsanaResponse<AsanaProject> UpdateProject(AsanaProject projectToUpdate)
		{
			return UseRequest<AsanaProject>(
				string.Format("projects/{0}", projectToUpdate.id), 
				request =>
					{
						request.AddObject(projectToUpdate);
						request.Method = Method.PUT;
					});
		}

		#endregion

		#region Methods

		private static AsanaResponse<T> GetData<T>(string resource) where T : new()
		{
			return UseRequest<T>(resource, request => { });
		}

		private static AsanaResponse<T> GetResponse<T>(string resource, Action<RestRequest> action) where T : new()
		{
			var client = new RestClient(_asanaBaseUrl);
			client.Authenticator = new HttpBasicAuthenticator(ApiKey, string.Empty);
			var request = new RestRequest(resource);
			action(request);
			var content = client.Execute(request);
			var response = new AsanaResponse<T>
				{
					Content = content.Content, 
					ContentEncoding = content.ContentEncoding, 
					ContentLength = content.ContentLength, 
					ContentType = content.ContentType, 
					ErrorException = content.ErrorException, 
					ErrorMessage = content.ErrorMessage, 
					RawBytes = content.RawBytes, 
					Request = content.Request, 
					ResponseStatus = content.ResponseStatus, 
					ResponseUri = content.ResponseUri, 
					Server = content.Server, 
					StatusCode = content.StatusCode, 
					StatusDescription = content.StatusDescription
				};
			return response;
		}

		private static string GetUrl(string url, List<string> optFields)
		{
			if (optFields != null)
			{
				url += "?opt_fields=" + string.Join(",", optFields);
			}

			return url;
		}

		private static string GetWorkspaceProjectsUrl(long workspaceId, List<string> optFields)
		{
			var url = GetUrl(string.Format("workspaces/{0}/projects", workspaceId.ToString()), optFields);

			return url;
		}

		private static string GetWorkspaceProjectsUrl(long workspaceId)
		{
			return GetWorkspaceProjectsUrl(workspaceId, null);
		}

		private static AsanaResponse<T> UseRequest<T>(string resource, Action<RestRequest> action) where T : new()
		{
			var response = GetResponse<T>(resource, action);

			switch (response.StatusCode)
			{
				case HttpStatusCode.BadRequest:
					response.Errors = JsonConvert.DeserializeObject<AsanaErrors>(response.Content).errors;
					break;
				case HttpStatusCode.OK:
				case HttpStatusCode.Created:
					response.Data = JsonConvert.DeserializeObject<DataClass<T>>(response.Content).data;
					break;
				default:
					throw new ApplicationException(string.Format("Unhandled status code {0}", "ARG0"));
			}

			return response;
		}

		private string GetProjectTasksUrl(long id)
		{
			return this.GetProjectTasksUrl(id, null);
		}

		private string GetProjectTasksUrl(long projectId, List<string> optFields)
		{
			return GetUrl(string.Format("/projects/{0}/tasks", projectId.ToString()), optFields);
		}

		private AsanaResponse<List<AsanaTask>> GetTasks(long projectId, List<string> optFields)
		{
			return GetData<List<AsanaTask>>(this.GetProjectTasksUrl(projectId, optFields));
		}

		#endregion
	}
}