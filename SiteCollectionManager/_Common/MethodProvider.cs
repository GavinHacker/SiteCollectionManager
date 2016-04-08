using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SiteCollectionManager._Sevice;
using System.Reflection;

namespace SiteCollectionManager._Common
{
    public static class MethodProvider
    {
        internal static void Log(this BackupSevice backup, string message) 
        {
            backup.AppendTxtFunc(backup.MessageBoxOfUI, message);
        }

        internal static void Log(this CreateSPDataService create, string message)
        {
            create.AppendTxtFunc(create.MessageBoxOfUI, message);
        }

        internal static void Log(this RestoreSevice restore, string message)
        {
            restore.AppendTxtFunc(restore.MessageBoxOfUI, message);
        }
    }

    public interface ISingleProvider { }

    public class SingleProvider<T> where T : class, ISingleProvider
    {
        public static T Instance 
        {
            get
            {
                return PeekInstance(null); 
            }
        }

        public static T PeekInstance(Object[] args)
        {
            return _Primary.PeekInstance(args); 
        }

        private class _Primary
        {
            static T instance;
            static Object lc = new Object();
            public static T PeekInstance(Object[] args)
            {
                if (instance == null)
                {
                    lock (lc)
                    {
                        if (instance == null)
                        {
                            try
                            {
                                instance = typeof(T).InvokeMember(typeof(T).Name, BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic, null, null, args) as T;
                            }
                            catch { }
                        }
                    }
                }
                return instance;
            }
        }
    }
}
