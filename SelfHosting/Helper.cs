﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SelfHosting
{
    public static class Helper
    {
        public static void Write(string stage, IPrincipal principal)
        {
            Debug.WriteLine("--------" + stage + "----------");
            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
            {
                Debug.WriteLine("anonymous user");
            }
            else
            {
                Debug.WriteLine("User: " + principal.Identity.Name);
            }
            Debug.WriteLine("\n");
        }
    }
}
