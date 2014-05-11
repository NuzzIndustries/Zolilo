using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zolilo.Accounts;
using Zolilo.Data;
using Zolilo.Web;

namespace Zolilo.Web
{
    //Singleton access object
    internal class TheBrain
    {
        static bool brainInitializing = false;
        
        #region Objects
        private static TheBrain theBrain_Instance;
        CommunicationsDirector communicationsDirector;
        WebDirector webDirector;
        #endregion

        internal TheBrain()
        {
            LogManager.Logger.Trace("Initializing TheBrain");
            LogManager.Logger.Debug("Attempting to initialize TheBrain explicitly (Coding Issue)");
            throw new ApplicationException("Cannot create an instance of TheBrain");
        }

        private TheBrain(object anyObject)
        {
            try
            {
                LogManager.Logger.Trace("Initializing TheBrain");
                communicationsDirector = new CommunicationsDirector();
                webDirector = new WebDirector();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        void Initialize()
        {
            communicationsDirector.Initialize();
            webDirector.Initialize();
        }


        #region Properties

        public CommunicationsDirector CommunicationsDirector
        {
            get { return communicationsDirector; }
        }

        public WebDirector WebDirector
        {
            get { return webDirector; }
        }
        
        internal static TheBrain TheBrain_Instance
        {
            get
            {
                if (theBrain_Instance == null)
                {
                    try
                    {
                        //*Initializing Brain
                        theBrain_Instance = new TheBrain(null);
                        theBrain_Instance.Initialize();
                    }
                    catch (Exception e)
                    {
                        //Invalidate object if it did not initialize
                        //This will force it to re-initialize
                        theBrain_Instance = null;
                        throw e;
                    }
                }
                return theBrain_Instance;
            }
        }

        #endregion
    }
}