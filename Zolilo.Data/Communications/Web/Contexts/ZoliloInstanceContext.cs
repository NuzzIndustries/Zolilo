using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zolilo.Web;
using Zolilo.Data;

namespace Zolilo.Web
{
    public class ZoliloInstanceContext : ZoliloStateTable
    {
        internal static string GetNewInstanceID()
        {
            Random r = new Random();
            int randomInteger = 0;
            do
            {
                randomInteger = r.Next();
            }
            while (zContext.Session.PageInstances.ContainsKey(randomInteger.ToString()));
            zContext.Session.PageInstances.Add(randomInteger.ToString(), new ZoliloInstanceContext());
            return randomInteger.ToString();
        }
    }
}