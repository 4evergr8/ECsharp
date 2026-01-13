using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

static class TrayIconApp
{
    public static void RunTrayIconInBackground(Action onExitAction)
    {
        Thread trayThread = new Thread(() =>
        {
            Application.Run(new TrayApplicationContext(onExitAction));
        });

        trayThread.IsBackground = true;
        trayThread.SetApartmentState(ApartmentState.STA); // WinForms 必须为 STA
        trayThread.Start();
    }

    private class TrayApplicationContext : ApplicationContext
    {
        private NotifyIcon notifyIcon;
        private Icon trayIcon;
        private readonly Action onExitAction;

        public TrayApplicationContext(Action onExit)
        {
            onExitAction = onExit;

            try
            {
                var asm = Assembly.GetExecutingAssembly();
                using (Stream stream = asm.GetManifestResourceStream("HonorPCManagerisJ8.J8.ico"))
                {
                    trayIcon = new Icon(stream);
                }
            }
            catch
            {
                MessageBox.Show("无法加载嵌入图标 J8.ico", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExitThread();
                return;
            }

            ContextMenuStrip trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("退出", null, OnExit);

            notifyIcon = new NotifyIcon
            {
                Icon = trayIcon,
                Text = "J8 托盘程序",
                ContextMenuStrip = trayMenu,
                Visible = true
            };
        }

        private void OnExit(object sender, EventArgs e)
        {
            onExitAction?.Invoke();
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
            trayIcon?.Dispose();
            Environment.Exit(0);
        }


        protected override void ExitThreadCore()
        {
            if (notifyIcon != null)
            {
                notifyIcon.Visible = false;
                notifyIcon.Dispose();
                trayIcon?.Dispose();
            }
            base.ExitThreadCore();
        }
    }
}
