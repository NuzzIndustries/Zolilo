using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zolilo.Data;
using Zolilo.Web;
using System.Threading;

namespace Zolilo
{
    //Singleton access object
    public class ZoliloSystem
    {
        public static bool systemInitialized = false;
        public static bool systemInitializing = false;
        public static Exception LastInitializationException = null;
        static Thread initThread;
        static long i2 = 1;
        #region Objects
        private static ZoliloSystem system_Instance;
        CommunicationsDirector communicationsDirector;
        WebDirector webDirector;
        #endregion

        internal ZoliloSystem()
        {
            LogManager.Logger.Trace("Initializing ZoliloSystem");
            LogManager.Logger.Debug("Attempting to initialize ZoliloSystem explicitly (Coding Issue)");
            throw new ApplicationException("Cannot create an instance of ZoliloSystem");
        }

        public static void BeginInit()
        {
            if (!systemInitializing && !systemInitialized)
            {
                if (initThread != null)
                {
                    if (initThread.IsAlive)
                        throw new InvalidOperationException("Unable to initialize System.");
                }
                initThread = new Thread(Init);
                initThread.Start();
            }
        }

        static void Init()
        {
            try
            {
                systemInitializing = true;
                /*
                for (int i = 0; i < 1000000000; i++) //testing
                {
                    i2++;
                    if (i == 100000000)
                        i -= 10000;
                    Thread.Sleep(100);
                }
                */
                
                system_Instance = new ZoliloSystem(null);
                system_Instance.Initialize();
                systemInitialized = true;
                systemInitializing = false;
            }
            catch (Exception e)
            {
                //Invalidate object if it did not initialize
                //This will force it to re-initialize
                system_Instance = null;
                systemInitialized = false;
                systemInitializing = false;
                LastInitializationException = e;
            }
        }

        private ZoliloSystem(object anyObject)
        {
            try
            {
                LogManager.Logger.Trace("Initializing ZoliloSystem");
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

        internal CommunicationsDirector CommunicationsDirector
        {
            get { return communicationsDirector; }
        }

        internal WebDirector WebDirector
        {
            get { return webDirector; }
        }
        
        internal static ZoliloSystem TheBrain_Instance
        {
            get
            {
                if (systemInitializing)
                    return null;
                if (!systemInitialized && !systemInitializing)
                {
                    BeginInit();
                    return null;
                }
                
                return system_Instance;
            }
        }

        #endregion
    }
}