using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace STEMUtil.MyComponents
{
    class FixedProgressBar : ProgressBar
    {
        [DllImportAttribute("uxtheme.dll")]
        private static extern int SetWindowTheme(IntPtr hWnd, string appname, string idlist);

        protected override void OnHandleCreated(EventArgs e)
        {
            SetWindowTheme(this.Handle, "", "");
            base.OnHandleCreated(e);
        }
    }
}
