using System;

namespace RESTfulAD.Models
{
    [Flags]
    public enum GroupScope : uint
    {
        NotSet = 0x00000000,
        //// <summary>
        //// Specifies a group that is created by the system.
        //// </summary>
        //System = 0x00000001,
        /// <summary>
        /// Specifies a group with global scope.
        /// </summary>
        Global = 0x00000002,
        /// <summary>
        /// Specifies a group with domain local scope.
        /// </summary>
        DomainLocal = 0x00000004,
        /// <summary>
        /// Specifies a group with universal scope.
        /// </summary>
        Universal = 0x00000008,
        ///// <summary>
        ///// Specifies an APP_BASIC group for Windows Server Authorization Manager.
        ///// </summary>
        //App_Basic = 0x00000010,
        ///// <summary>
        ///// Specifies an APP_QUERY group for Windows Server Authorization Manager.
        ///// </summary>
        //App_Query = 0x00000020,
        /// <summary>
        /// Specifies a security group.If this flag is not set, then the group is a distribution group.
        /// </summary>
        Security = 0x80000000,
    }
}
