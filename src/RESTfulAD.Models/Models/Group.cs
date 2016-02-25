using System.Collections.Generic;
using System.DirectoryServices;

namespace RESTfulAD.Models
{
    public class Group : ADObjectBase
    {
        private bool _init = false;
        private GroupScope _scope = GroupScope.NotSet;

        public GroupScope GroupScope
        {
            get
            {
                if (!_init) InitScopeAndType();
                return _scope;
            }
            set
            {
                _scope = value;
            }
        }

        private bool _isSecurity = false;

        public bool IsSecurityGroup
        {
            get
            {
                if (!_init) InitScopeAndType();
                return _isSecurity;
            }
            set
            {
                _isSecurity = value;
            }
        }

        public string OwnerDN
        {
            get
            {
                return this.Properties.GetPropertyValue(PropertyNameConstants.managedBy);
            }
        }

        public ADObjectBase Owner { get; set; }

        public List<string> ExchangeCoOwnerDNs
        {
            get
            {
                return this.Properties.GetPropertyValues<string>(PropertyNameConstants.msExchCoManagedByLink);
            }
        }

        public List<ADObjectBase> CoOwners { get; set; }

        public List<string> MemberDNs
        {
            get
            {
                return this.Properties.GetPropertyValues<string>(PropertyNameConstants.member);
            }
        }

        public List<ADObjectBase> Members { get; set; } = new List<ADObjectBase>();

        private void InitScopeAndType()
        {
            var grouptype = this.Properties.GetPropertyValue<int>(PropertyNameConstants.groupType);
            this.SetGroupTypeAndScope(grouptype);
            _init = true;
        }

        internal void SetGroupTypeAndScope(int hex)
        {
            GroupScope gs = (GroupScope)hex;
            if (gs.HasFlag(GroupScope.Security))
                this.IsSecurityGroup = true;

            if (gs.HasFlag(GroupScope.Global))
                this.GroupScope = GroupScope.Global;

            if (gs.HasFlag(GroupScope.DomainLocal))
                this.GroupScope = GroupScope.DomainLocal;

            if (gs.HasFlag(GroupScope.Universal))
                this.GroupScope = GroupScope.Universal;
        }

        public bool CanChangeMembers { get { return CanIChangeProperty("members"); } }

        public Group(DirectoryEntry de) : base(de)
        {
        }

        public Group(SearchResult sr) : base(sr)
        {
        }

        public Group(ADObjectBase adObj) : base(adObj)
        {
        }

        internal Group(PropertyCache pc) : base(pc)
        {
        }

        public Group() : base()
        {
        }
    }
}

//https://msdn.microsoft.com/en-us/library/ms675935%28v=vs.85%29.aspx
//Value                     Description
//1 (0x00000001)	        Specifies a group that is created by the system.
//2 (0x00000002)            Specifies a group with global scope.
//4 (0x00000004)            Specifies a group with domain local scope.
//8 (0x00000008)	        Specifies a group with universal scope.
//16 (0x00000010)           Specifies an APP_BASIC group for Windows Server Authorization Manager.
//32 (0x00000020)           Specifies an APP_QUERY group for Windows Server Authorization Manager.
//2147483648 (0x80000000)   Specifies a security group.If this flag is not set, then the group is a distribution group.