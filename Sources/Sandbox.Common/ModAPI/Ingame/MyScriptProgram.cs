using System;
using System.Reflection;
using VRage;
using VRage.Library.Utils;

namespace Sandbox.ModAPI.Ingame
{
    /// <summary>
    ///     Base class from script programs
    /// </summary>
    public abstract class MyScriptProgram
    {
        private readonly object[] m_argumentArray = new object[1];
        private readonly MethodInfo m_mainMethod;
        private readonly bool m_mainMethodSupportsArgument;

        private IMyGridTerminalSystem m_gridTerminalSystem;
        private IMyScriptProgramEnvironment m_environment;

        private string m_storage;
        private TimeSpan m_elapsedTime;

        protected MyScriptProgram()
        {
            Storage = "";

            var type = GetType();
            m_mainMethod = type.GetMethod("Main", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(string) }, null);
            m_mainMethodSupportsArgument = m_mainMethod != null;
            if (m_mainMethod == null)
            {
                m_mainMethod = type.GetMethod("Main", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            }

            if (m_mainMethod == null)
            {
                throw new ScriptHasNoMainMethodException();
            }
        }

        #region script API

        // These methods and properties are accessed from script
        // Some of them (Storage, ElapsedTime) are also accessed from MyProgrammableBlock

        public IMyGridTerminalSystem GridTerminalSystem
        {
            get { return m_gridTerminalSystem; }
            internal set { m_gridTerminalSystem = value; }
        }

        public string Storage
        {
            get { return m_storage; }
            set { m_storage = value; }
        }

        public IMyProgrammableBlock Me
        {
            get { return m_environment.ProgrammableBlock; }
        }

        public TimeSpan ElapsedTime
        {
            get { return m_elapsedTime; }
            set { m_elapsedTime = value; }
        }

        public void Echo(string message)
        {
            m_environment.WriteEcho(message);
        }

        public void Echo(string message, params object[] args)
        {
            m_environment.WriteEcho(string.Format(message, args));
        }

        public bool SetTimeout(int timeout, MyScriptTimeoutFunction callback)
        {
            return m_environment.SetTimeout(timeout, callback);
        }

        public bool SetTimeout(int timeout, Action callback)
        {
            return m_environment.SetTimeout(timeout, _ => callback());
        }

        #endregion

        #region script internals

        // These methods with "Internal_" prefix are called from MyProgrammableBlock

        internal void Internal_SetScriptProgramEnvironment(IMyScriptProgramEnvironment environment)
        {
            m_environment = environment;
        }

        internal void Internal_Run(string argument)
        {
            if (m_mainMethodSupportsArgument)
            {
                // Don't know if it's really necessary to predefine this argument array, I suspect not
                // due to the cleverness of the compiler, but I do it this way just in case. 
                // Obviously if programmable block execution becomes asynchronous at some point this 
                // must be reworked.
                m_argumentArray[0] = argument ?? string.Empty;
                m_mainMethod.Invoke(this, m_argumentArray);
            }
            else
            {
                m_mainMethod.Invoke(this, null);
            }
        }

        #endregion
    }
}