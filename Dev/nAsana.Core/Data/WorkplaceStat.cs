namespace Asana.Core.Data
{
	using System;

	public class WorkplaceStat
	{
		#region Public Properties

		public int DueToday { get; set; }

		public DateTime? LastModified { get; set; }

		public int NeverDue { get; set; }

		public DateTime? NewestTask { get; set; }

		public int NotDueYet { get; set; }

		public DateTime? OldestTask { get; set; }

		public double PastDueDays { get; set; }

		public int PastDueTasks { get; set; }

		public int ProjectFollowers { get; set; }

		public int TaskFollowers { get; set; }

		public int TasksCompleted { get; set; }

		public int TotalTasks { get; set; }

		public bool archived { get; set; }

		public DateTime? created_at { get; set; }

		public long id { get; set; }

		public DateTime? modified_at { get; set; }

		public string name { get; set; }

		public string notes { get; set; }

		public long workspaceid { get; set; }

		#endregion
	}
}