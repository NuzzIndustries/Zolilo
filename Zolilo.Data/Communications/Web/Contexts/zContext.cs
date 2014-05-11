using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Zolilo.Data;

namespace Zolilo.Web
{
    public static class zContext
    {
        //7 levels of contextual layers
        //System
        //Session
        //Instance
        //Supervisor
        //Frame
        //Request
        //Individual objects

        internal static ZoliloSystem System
        {
            get { return ZoliloSystem.TheBrain_Instance; }
        }

        public static ZoliloSession Session
        {
            get { return ZoliloSession.Current; }
        }

        public static ZoliloInstanceContext UserInstance
        {
            get { return Session.PageInstances[Page.InstanceID]; }
        }

        public static DR_Accounts Account
        {
            get
            {
                if (Session == null)
                    return null;
                return Session.CurrentAccount;
            }
        }
    
        public static ZoliloPageSupervisorContext Supervisor
        {
            get 
            {
                if (Page == null)
                    return null;
                if (Page.SupervisorControl == null)
                    return null;
                return Page.SupervisorControl.Context;
            }
        }

        public static ZoliloPageFrameContext Frame
        {
            get 
            {
                if (zContext.Supervisor == null)
                    return null;
                if (Page.UFrameID == null)
                    return null;
                if (zContext.Supervisor.Frames[Page.UFrameID] == null)
                    zContext.Supervisor.Frames[Page.UFrameID] = new ZoliloPageFrameContext(Page.UFrameID);
                return (ZoliloPageFrameContext)zContext.Supervisor.Frames[Page.UFrameID];
            }
        }

        public static ZoliloRequestContext Request
        {
            get { return ZoliloRequestContext.Current; }
        }

        public static ZoliloPage Page
        {
            get { return Request.Page; }
        }
    }
}
