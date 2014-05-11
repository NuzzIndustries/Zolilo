using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zolilo.Web
{
    /// <summary>
    /// Marker interface.  Designates that the page requires login
    /// </summary>
    public interface IRestrictedPage { }

    public interface IPageIsReturnBegin { }
    public interface IPageIsReturnPersist { }
}
