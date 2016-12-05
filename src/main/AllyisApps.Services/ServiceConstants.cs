﻿namespace AllyisApps.Services
{
    /// <summary>
    /// Constants for file column header strings (like excel), to be used in imports and exports 
    /// </summary>
    class ColumnHeaders
    {
        #region customer
        /// <summary>
        /// Customer-Property: Name
        /// </summary>
        public const string CustomerName = "Customer Name";

        /// <summary>
        /// Customer-Property: Street Address
        /// </summary>
        public const string CustomerStreetAddress = "Customer Street Address";

        /// <summary>
        /// Customer-Property: City
        /// </summary>
        public const string CustomerCity = "Customer City";

        /// <summary>
        /// Customer-Property: Country
        /// </summary>
        public const string CustomerCountry = "Customer Country";

        /// <summary>
        /// Customer-Property: State
        /// </summary>
        public const string CustomerState = "Customer State";

        /// <summary>
        /// Customer-Property: Postal Code
        /// </summary>
        public const string CustomerPostalCode = "Customer Postal Code";

        /// <summary>
        /// Customer-Property: Email
        /// </summary>
        public const string CustomerEmail = "Customer Email";

        /// <summary>
        /// Customer-Property: Phone Number
        /// </summary>
        public const string CustomerPhoneNumber = "Customer Phone Number";

        /// <summary>
        /// Customer-Property: Fax Number
        /// </summary>
        public const string CustomerFaxNumber = "Customer Fax Number";

        /// <summary>
        /// Customer-Property: EIN
        /// </summary>
        public const string CustomerEIN = "Customer EIN";
        #endregion customer

        #region project
        /// <summary>
        /// Project-Property: Name
        /// </summary>
        public const string ProjectName = "Project Name";

        /// <summary>
        /// Project-Property: Type
        /// </summary>
        public const string ProjectType = "Project Type";

        /// <summary>
        /// Project-Property: StartUTC
        /// </summary>
        public const string ProjectStartDate = "Project Start Date";

        /// <summary>
        /// Project-Property: EndUTC
        /// </summary>
        public const string ProjectEndDate = "Project End Date";
        #endregion project

        #region user
        /// <summary>
        /// User-Property: Email
        /// </summary>
        public const string UserEmail = "User Email";

        /// <summary>
        /// User-Property: First Name
        /// </summary>
        public const string UserFirstName = "User First Name";

        /// <summary>
        /// User-Property: Last Name
        /// </summary>
        public const string UserLastName = "User Last Name";
        #endregion
    }
}
