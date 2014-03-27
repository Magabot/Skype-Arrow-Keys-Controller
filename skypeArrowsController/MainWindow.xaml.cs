using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;
using System.IO;
using SKYPE4COMLib;

namespace skypeArrowsController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Skype skype;

        private Dispatcher _dispatcher;
        string selectedSkypeChatUser;
        System.Threading.Timer time;
        int skypeChats = 0;

        public MainWindow()
        {
            InitializeComponent();

            skype = new Skype();
            skype.MessageStatus += new _ISkypeEvents_MessageStatusEventHandler(skype_MessageStatus);
            skype.CallStatus += new _ISkypeEvents_CallStatusEventHandler(skype_CallStatus);

            TimerCallback tcb = this.CheckStatus;
            AutoResetEvent ar = new AutoResetEvent(true);
            time = new System.Threading.Timer(tcb, ar, 250, 250);

            _dispatcher = this.Dispatcher;
        }

        private void CheckStatus(Object stateInfo)
        {
            try
            {
                skypeChats = skype.ActiveChats.Count;
            }
            catch (InvalidCastException e)
            {
            }
        }

        public void skype_MessageStatus(ChatMessage pMessage, TChatMessageStatus Status)
        {
            if (Status == TChatMessageStatus.cmsReceived)
            {
            }
        }

        public void skype_CallStatus(Call call, TCallStatus status)
        {
            try
            {
                if (status == TCallStatus.clsInProgress)
                {
                    selectedSkypeChatUser = call.PartnerHandle.ToString();

                    labelSelectedUser.Content = call.PartnerDisplayName.ToString();
                }
            }
            catch
            {

            }
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (selectedSkypeChatUser != "" && selectedSkypeChatUser != null)
            {
                String msg = "";

                if (e.Key == Key.Left)
                {
                    labelOrderValue.Content = "a - left";
                    msg = "a";
                }
                if (e.Key == Key.Right)
                {
                    labelOrderValue.Content = "d - right";
                    msg = "d";
                }
                if (e.Key == Key.Up)
                {
                    labelOrderValue.Content = "w - up";
                    msg = "w";
                }
                if (e.Key == Key.Down)
                {
                    labelOrderValue.Content = "s - down";
                    msg = "s";
                }
                if (e.Key == Key.Space)
                {
                    labelOrderValue.Content = "p - stop";
                    msg = "p";
                }

                _dispatcher.BeginInvoke((Action)(() =>
                {
                    if(msg != "")
                        skype.SendMessage(selectedSkypeChatUser, msg);
                }));
            }
            else
            {
                labelOrderValue.Content = "No user selected";
            }
        }

    }

}
