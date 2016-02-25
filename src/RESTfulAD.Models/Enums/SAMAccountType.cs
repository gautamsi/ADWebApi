﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTfulAD.Models
{
    public enum SAMAccountType
    {
        SAM_DOMAIN_OBJECT                 =     0x00000000,
        SAM_GROUP_OBJECT                  =     0x10000000,
        SAM_NON_SECURITY_GROUP_OBJECT     =     0x10000001,
        SAM_ALIAS_OBJECT                  =     0x20000000,
        SAM_NON_SECURITY_ALIAS_OBJECT     =     0x20000001,
        SAM_USER_OBJECT                   =     0x30000000,
        SAM_NORMAL_USER_ACCOUNT           =     0x30000000,
        SAM_MACHINE_ACCOUNT               =     0x30000001,
        SAM_TRUST_ACCOUNT                 =     0x30000002,
        SAM_APP_BASIC_GROUP               =     0x40000000,
        SAM_APP_QUERY_GROUP               =     0x40000001,
        SAM_ACCOUNT_TYPE_MAX              =     0x7fffffff,
    }

    //https://msdn.microsoft.com/en-us/library/cc245527.aspx
    //Name                              Value
    //SAM_DOMAIN_OBJECT                 0x0
    //SAM_GROUP_OBJECT                  0x10000000
    //SAM_NON_SECURITY_GROUP_OBJECT     0x10000001
    //SAM_ALIAS_OBJECT                  0x20000000
    //SAM_NON_SECURITY_ALIAS_OBJECT     0x20000001
    //SAM_USER_OBJECT                   0x30000000
    //SAM_NORMAL_USER_ACCOUNT           0x30000000
    //SAM_MACHINE_ACCOUNT               0x30000001
    //SAM_TRUST_ACCOUNT                 0x30000002
    //SAM_APP_BASIC_GROUP               0x40000000
    //SAM_APP_QUERY_GROUP               0x40000001
    //SAM_ACCOUNT_TYPE_MAX              0x7fffffff

}
