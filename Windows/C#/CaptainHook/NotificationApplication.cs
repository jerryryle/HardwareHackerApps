using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

namespace CaptainHook
{
    class NotificationApplication : ApplicationContext
    {
        private NotifyIcon NotificationIcon;
        private ContextMenuStrip NotificationIconContextMenu;
        private ToolStripMenuItem ExitMenuItem;
        private ToolStripMenuItem AboutMenuItem;
        private AboutBox About;
        private KeyboardHook KBHook;
        private System.Timers.Timer KeyTimer;

        public NotificationApplication()
        {
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
            InitializeComponent();
            NotificationIcon.Visible = true;
        }

        private void InitializeComponent()
        {
            NotificationIcon = new NotifyIcon()
            {
                BalloonTipIcon = ToolTipIcon.Info,
                Text = "Captain Hook",
                Icon = Properties.Resources.NotificationIcon_hook
            };

            About = new AboutBox();

            ExitMenuItem = new ToolStripMenuItem()
            {
                Name = "Exit",
                AutoSize = true,
                Text = "E&xit"
            };
            ExitMenuItem.Click += new EventHandler(this.Exit_Click);

            AboutMenuItem = new ToolStripMenuItem()
            {
                Name = "About",
                AutoSize = true,
                Text = "&About ..."
            };
            AboutMenuItem.Click += new EventHandler(this.About_Click);

            NotificationIconContextMenu = new ContextMenuStrip()
            {
                Name = "TrayIconContextMenu",
                AutoSize = true,
            };
            NotificationIconContextMenu.Items.AddRange(new ToolStripItem[] { AboutMenuItem, this.ExitMenuItem });

            NotificationIcon.ContextMenuStrip = NotificationIconContextMenu;

            KeyTimer = new System.Timers.Timer()
            {
                AutoReset = true,
                Enabled = false
            };
            KeyTimer.Elapsed += new ElapsedEventHandler(OnKeyTimer);

            KBHook = new KeyboardHook();
            KBHook.KeyPressed += new EventHandler<KeyboardHook.KeyPressedEventArgs>(this.OnKeyPressed);
            KBHook.EnableHook();
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            NotificationIcon.Visible = false;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void About_Click(object sender, EventArgs e)
        {
            if (!About.Visible)
            {
                About.Show();
            }
        }

        private void OnKeyTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            NotificationIcon.Icon = Properties.Resources.NotificationIcon_hook;
            KeyTimer.Enabled = false;
        }

        private void OnKeyPressed(object sender, KeyboardHook.KeyPressedEventArgs e)
        {
            switch (e.VKCode)
            {
                case VirtualKeys.VK_A:
                    /* This demonstrates a key event handler that performs one action when a key is pressed
                     * and another when the key is released. */
                    if (e.IsDown)
                    {
                        /* Note that this will occurr repeatedly while a key is held. If you care about
                         * catching only the transition from released to held, you'll need to track that
                         * state yourself. */
                        // Key pressed or held.
                        NotificationIcon.Icon = Properties.Resources.NotificationIcon_hookfish;
                    }
                    else
                    {
                        // Key released
                        NotificationIcon.Icon = Properties.Resources.NotificationIcon_hook;
                    }
                    break;

                case VirtualKeys.VK_B:
                    /* This demonstrates a key event handler that performs an action when a key is released
                     * and sets a timer to clear the action a fixed time later. This timer is extended every time
                     * the key is released. */
                    if (!e.IsDown)
                    {
                        NotificationIcon.Icon = Properties.Resources.NotificationIcon_hookbait;
                        KeyTimer.Enabled = false;
                        KeyTimer.Interval = 250;
                        KeyTimer.Enabled = true;
                    }
                    break;

                default:
                    // If we're not processing the key, return here to allow it to be passed to an application.
                    return;
            }
            // If we processed the key, set TrapKey to true to prevent it from reaching an application.
            e.TrapKey = true;
        }

    }
}
