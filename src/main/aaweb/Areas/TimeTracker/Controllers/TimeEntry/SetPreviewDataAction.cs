//------------------------------------------------------------------------------
// <copyright file="SetPreviewDataAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using AllyisApps.Core;
using AllyisApps.Services.BusinessObjects;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseProductController
	{
		/// <summary>
		/// Sets the PreviewData for DataExportViewModel.
		/// </summary>
		/// <param name="data">The preview data to set.</param>
		/// <param name="pageSize">The page size.</param>
		/// <param name="page">The page.</param>
		/// <returns>A list of time entry info objects.</returns>
		public IEnumerable<TimeEntryInfo> SetPreviewData(IEnumerable<TimeEntryInfo> data, int pageSize = 0, int page = 1)
		{
			int skipNum = pageSize * (page - 1);
			int limit = pageSize == 0 ? data.Count() : pageSize;

			// only process data values for current page
			IEnumerable<TimeEntryInfo> previewData = (from p in data
													  select p).Skip(skipNum).Take(limit);

			return previewData;
		}

		/// <summary>
		/// Sets the pageTotal for a DataExportViewModel in preperation for SetPreviewdata.
		/// </summary>
		/// <param name="data">TimeEntryInfo  needed to find total page size.</param>
		/// <param name="pageSize">Total size of preivew data.</param>
		/// <param name="page">Offset page amount.</param>
		/// <returns>The page total.</returns>
		public int SetPageTotal(IEnumerable<TimeEntryInfo> data, int pageSize = 0, int page = 1)
		{
			int skipNum = pageSize * (page - 1);
			int pageTotal = 0;
			if (data.Count() == 0)
			{
				pageTotal = 1;
			}
			else
			{
				// Count equals pageSize --> fits on 1 page
				pageTotal = 1 + ((data.Count() - 1) / pageSize);
			}

			return pageTotal;
		}
	}
}