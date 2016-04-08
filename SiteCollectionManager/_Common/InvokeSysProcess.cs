using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SiteCollectionManager._Common.GException;
using System.Windows.Forms;
using System.Diagnostics;

namespace SiteCollectionManager._Common
{
    public class InvokeSysProcess
    {
        private static string mBrowser = string.Empty;
        /// <summary>
        /// explorer.
        /// </summary>
        /// <param name="url"></param>
        public static void OpenInExplorer(string url) 
        {
            try
            {
                mBrowser = "explorer";
                System.Diagnostics.Process.Start(mBrowser, url);
            }
            catch (Exception e)
            {
                //
                throw new InvokerProcessException(e, e.Message);
            }
        }

        /// <summary>
        /// copy url to clipboard.
        /// </summary>
        public static void CoypeUrlToClipboard(string txt)
        {
            try
            {
                Clipboard.SetDataObject(txt);
            }
            catch (Exception e)
            {
                throw new InvokerProcessException(e, e.Message);
            }
        }

        public static int StartSTSADM(string sts, string command, bool _isWait) 
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.WindowStyle = ProcessWindowStyle.Hidden;
                info.Arguments = command;
                info.FileName = sts;
                Process tempPro = Process.Start(info);
                if (_isWait) 
                {
                    tempPro.WaitForExit();
                    return tempPro.ExitCode;
                }
                return 0;
            }
            catch (Exception e)
            {
                throw new InvokerProcessException(e, e.Message);
            }
        }

        public static void StartNotepad(string txtPath) 
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = "Notepad.exe";
                info.Arguments = txtPath;
                info.WindowStyle = ProcessWindowStyle.Normal;
                Process tempPro = Process.Start(info);
            }
            catch (Exception ex) 
            {
                throw new ViewBaseInfoSeviceException(ex, ex.Message);
            }
        }

    }
}
