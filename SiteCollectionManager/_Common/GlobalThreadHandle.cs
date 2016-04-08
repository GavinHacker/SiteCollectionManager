using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;

namespace SiteCollectionManager._Common
{
    [Obsolete("GStart Instead.")]
    public delegate void __GStart();

    public delegate void GStart(object sender, EventArgs e);

    public delegate void GCompleted();

    [Obsolete("")]
    internal class __GlobalThreadHandle
    {
        private static Thread mThreadHandle = null;

        public static ThreadPriority Priority 
        {
            get 
            {
                return mThreadHandle.Priority;
            }
        }

        public static ThreadState State 
        {
            get 
            {
                return mThreadHandle.ThreadState;
            }
        }

        public static string Name 
        {
            get 
            {
                return mThreadHandle.Name;
            }
        }

        private ThreadExceptionEventHandler exHandler = null;
        
        public static void Start(__GStart rStart) 
        {
            mThreadHandle =new Thread(new ThreadStart(rStart));
            mThreadHandle.Start();
        }

        public static void Start(__GStart targ, ThreadPriority prio, string name)
        {
            mThreadHandle = new Thread(new ThreadStart(targ));
            mThreadHandle.Name = name;
            mThreadHandle.IsBackground = true;
            mThreadHandle.Priority = prio;
            mThreadHandle.Start();
        }

        private static void WorkInternal(object sender, EventArgs e)
        {
            mThreadHandle.Join();
        }

        private static void WorkCompleted(object sender, EventArgs e) 
        {

        }
    }

    internal class GlobalThreadHandle : BackgroundWorker 
    {
        public GlobalThreadHandle(string name) 
        {
            GThreadName = name;
        }

        public GlobalThreadHandle(string name, GStart mExecution, GStart mCompleted)
        {
            GThreadName = name;
            this.mExecutionUnit = mExecution;
            this.mCompletedUnit = mCompleted;
        }

        private string GThreadName
        {
            get;
            set;
        }

        private GStart mExecutionUnit = null;

        private GStart mCompletedUnit = null;

        public void Start() 
        {
            base.DoWork += new DoWorkEventHandler(mExecutionUnit);
            base.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mCompletedUnit);
            base.RunWorkerAsync();
        }

        public void SetAysnInfo(GStart mExecution, GStart mCompleted) 
        {
            this.mExecutionUnit = mExecution;
            this.mCompletedUnit = mCompleted;
        }
    }
}
