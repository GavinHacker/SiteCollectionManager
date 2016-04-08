using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiteCollectionManager._Common.GException
{
    public class _Exception : Exception
    {
        internal Exception mInrException;
        internal string mInternalExceptionMessage;

        public _Exception(string _internalException)
        {
            mInternalExceptionMessage = _internalException;
        }

        // used when re-throw a caught exception
        public _Exception(Exception innerException, string _internalException)
            : base(innerException.Message, innerException)
        {
            mInrException = innerException;
            mInternalExceptionMessage = _internalException;
            mInternalExceptionMessage += mInrException.Message;
        }

        public override string Message
        {
            get
            {
                return mInternalExceptionMessage;
            }
        }

        public override string StackTrace
        {
            get
            {
                if (mInrException == null)
                {
                    return base.StackTrace;
                }
                else
                {
                    string trace = mInrException.StackTrace + "\r\n" + base.StackTrace;
                    return trace;
                }
            }
        }

        public override string ToString() 
        {
            return Message + "\r\n" + StackTrace;
        }
    }

    public class InvokerProcessException : _Exception
    {
        internal string mExceptionType = "InvokerProcessException";

        public InvokerProcessException(string _internalException)
            : base(_internalException)
        {
        }

        public InvokerProcessException(Exception innerException, string _internalException)
            : base(innerException, _internalException)
        {
        }

        public override string Message
        {
            get
            {
                return mExceptionType + " " + base.mInternalExceptionMessage;
            }
        }

        public override string StackTrace
        {
            get
            {
                //if (mInrException == null)
                //{
                return mExceptionType + "\r\n" + base.StackTrace;
                //}
                //else
                //{
                //    string trace = mExceptionType + "\r\n" + mInrException.StackTrace;
                //    return trace;
                //}
            }
        }
    }

    public class ClearSeviceException : _Exception
    {
        internal string mExceptionType = "ClearSPDataException";

        public ClearSeviceException(string _internalException)
            : base(_internalException)
        {
        }

        public ClearSeviceException(Exception innerException, string _internalException)
            : base(innerException, _internalException)
        {
        }

        public override string Message
        {
            get
            {
                return mExceptionType + " " + base.mInternalExceptionMessage;
            }
        }

        public override string StackTrace
        {
            get
            {
                //if (mInrException == null)
                //{
                return mExceptionType + "\r\n" + base.StackTrace;
                //}
                //else
                //{
                //    string trace = mExceptionType + "\r\n" + mInrException.StackTrace;
                //    return trace;
                //}
            }
        }
    }

    public class ViewBaseInfoSeviceException : _Exception
    {
        internal string mExceptionType = "ViewBaseInfoSevice";

        public ViewBaseInfoSeviceException(string _internalException)
            : base(_internalException)
        {
        }

        public ViewBaseInfoSeviceException(Exception innerException, string _internalException)
            : base(innerException, _internalException)
        {
        }

        public override string Message
        {
            get
            {
                return mExceptionType + " " + base.mInternalExceptionMessage;
            }
        }

        public override string StackTrace
        {
            get
            {
                return mExceptionType + "\r\n" + base.StackTrace;
            }
        }
    }

    public class RestoreSPDataException : _Exception
    {
        internal string mExceptionType = "Restore sharepoint data failed.";
        internal int mExitCode = int.MaxValue;

        public RestoreSPDataException(string _internalException, int exitCode)
            : base(_internalException)
        {
            mExitCode = exitCode;
        }

        public RestoreSPDataException(Exception innerException, string _internalException, int exitCode)
            : base(innerException, _internalException)
        {
            mExitCode = exitCode;
        }

        public override string Message
        {
            get
            {
                return mExceptionType + " " + base.mInternalExceptionMessage;
            }
        }

        public override string StackTrace
        {
            get
            {
                return mExceptionType + "\r\n" + base.StackTrace;
            }
        }
    }
}
