namespace AllyisApps.Services
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
