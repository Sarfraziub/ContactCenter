using System;
using System.Collections.Generic;
using System.Text;

namespace ContactCenter.Lib
{
    public enum UserRole : int
    {
        AGENT = 1 << 0,
        CONTACT_CENTER_MANAGER = 1 << 1,
        ADMIN = 1 << 31
    }
}
