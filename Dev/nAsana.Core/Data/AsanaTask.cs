namespace Asana.Core.Data
{
	using System;
	using System.Collections.Generic;

	public class AsanaTask
	{
		#region Constructors and Destructors

		public AsanaTask()
		{
			this.followers = new List<AsanaFollower>();
		}

		#endregion

		#region Public Properties

		public virtual long assignee { get; set; }

		public virtual string assignee_status { get; set; }
		public virtual bool completed { get; set; }
		public virtual DateTime? completed_at { get; set; }
		public virtual DateTime created_at { get; set; }
		public virtual DateTime? due_on { get; set; }
		public virtual List<AsanaFollower> followers { get; set; }
		public virtual DateTime modified_at { get; set; }
		public virtual string name { get; set; }
		public virtual string notes { get; set; }
		public virtual List<AsanaProject> projects { get; set; }
		public virtual long workspace { get; set; }

		#endregion
	}
}