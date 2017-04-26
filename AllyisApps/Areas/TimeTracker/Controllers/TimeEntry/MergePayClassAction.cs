//------------------------------------------------------------------------------
// <copyright file="MergePayClassAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.DBModel.TimeTracker;
using AllyisApps.Services;
using AllyisApps.Services.TimeTracker;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
    /// <summary>
    /// Class which manages Time Entry objects.
    /// </summary>
    public partial class TimeEntryController : BaseController
    {
        /// <summary>
        /// Merge a pay class with another one
        /// </summary>
        /// <param name="payClassId">The id of the class to merge.</param>
        /// <returns>Redirects to the MergePayClass view.</returns>
        [HttpGet]
        public ActionResult MergePayClass(int payClassId)
        {
            var allPayClasses = TimeTrackerService.GetPayClasses();
            var destPayClasses = allPayClasses.Where(pc => pc.PayClassID != payClassId);
            string sourcePayClassName = allPayClasses.Where(pc => pc.PayClassID == payClassId).ElementAt(0).Name;

            //Built-in, non-editable pay classes cannot be merged
            if (sourcePayClassName == "Regular" || sourcePayClassName == "Overtime" || sourcePayClassName == "Holiday" || sourcePayClassName == "Paid Time Off" || sourcePayClassName == "Unpaid Time Off")
            {
                Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.TimeEntry.Strings.CannotMergePayClass, Variety.Warning));
                return this.RedirectToAction(ActionConstants.Settings);
            }

            if (Service.Can(Actions.CoreAction.EditOrganization))
            {
                MergePayClassViewModel model = ConstructMergePayClassViewModel(payClassId, sourcePayClassName, destPayClasses);
                return this.View(ViewConstants.MergePayClass, model);
            }

            Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));
            return this.View(ViewConstants.Error, new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Errors.CannotEditSubscriptionsMessage), ControllerConstants.TimeEntry, ActionConstants.MergePayClass));
        }

        /// <summary>
        /// Uses services to populate a <see cref="MergePayClassViewModel"/> and returns it.
        /// </summary>
        /// <param name="sourcePayClassId">The id of the pay class being merged</param>
        /// <param name="destPayClasses">List of all PayClass that can be merged into</param>
        /// <param name="sourcePayClassName">The name of the pay class being merged</param>
        /// <returns>The MergePayClassViewModel.</returns>
        [CLSCompliant(false)]
        public MergePayClassViewModel ConstructMergePayClassViewModel(int sourcePayClassId, string sourcePayClassName, IEnumerable<PayClass> destPayClasses)
        {
            if (destPayClasses != null)
            {
                return new MergePayClassViewModel
                {
                    sourcePayClassId = sourcePayClassId,
                    sourcePayClassName = sourcePayClassName,
                    destinationPayClasses = destPayClasses
                };
            }

            return new MergePayClassViewModel
            {
                sourcePayClassId = sourcePayClassId,
                sourcePayClassName = sourcePayClassName,
            };
        }

        /// <summary>
        /// Merge a pay class into another one: delete the old pay class, change all of its time entries' payclassId to the new one
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="destPayClass">The destination pay class' id.</param>
        /// <returns>A page.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CLSCompliant(false)]
        public ActionResult MergePayClass (MergePayClassViewModel model, int destPayClass)
        {
            IEnumerable<TimeEntryDBEntity> allEntries = TimeTrackerService.GetTimeEntriesThatUseAPayClass(model.sourcePayClassId);
            //update the payClassId for all time entries that used the old pay class
            foreach(TimeEntryDBEntity entity in allEntries)
            {
                TimeEntryInfo updatedEntry = TimeTrackerService.InitializeTimeEntryInfo(entity);
                updatedEntry.PayClassId = destPayClass;
                TimeTrackerService.UpdateTimeEntry(updatedEntry);               
            }
            //delete the old payclass
            if (TimeTrackerService.DeletePayClass(model.sourcePayClassId))
            {
                Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.TimeEntry.Strings.SuccessfulMergePayClass, Variety.Success));
            }
            else
            {
                // Should only be here because of permission failures
                Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));
            }
            return this.RedirectToAction(ActionConstants.Settings);
        }
    }    
}
