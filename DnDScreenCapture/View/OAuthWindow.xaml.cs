﻿using DnDScreenCapture.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DnDScreenCapture.View
{
    /// <summary>
    /// OAuthWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class OAuthWindow : Window
    {
        public OAuthWindow(Uri oauthUri)
        {
            InitializeComponent();
            var vm = new OAuthWindowViewModel();
            this.OAuthBrowser.Source = oauthUri;
            OAuthBrowser.Navigating += (sender, obj) =>
            {
                var url = obj.Uri.ToString();
                Console.WriteLine($"navigation:{url}");
                if(url.IndexOf(Properties.Resources.CallbackScheme) >= 0)
                {
                    var token = url.Substring(url.IndexOf("?oauth_token=") + 13, 27);
                    var verifier = url.Substring(url.IndexOf("&oauth_verifier=") + 16);
                    Console.WriteLine($"token:{token} verifier:{verifier}");
                    oauthCallbackHandler?.Invoke(new OAuthCallbackEventArgs(token, verifier));
                }

            };
            //new System.Windows.Navigation.NavigatingCancelEventHandler()
            this.DataContext = vm;
            vm.Initialized(oauthUri);
        }
        
        public event OAuthCallbackHandler oauthCallbackHandler;

        public delegate void OAuthCallbackHandler(OAuthCallbackEventArgs ev);
        public class OAuthCallbackEventArgs: EventArgs
        {
            public string Token { get; set; }
            public string Verifier { get; set; }
            public OAuthCallbackEventArgs(string token, string verifier)
            {
                Token = token;
                Verifier = verifier;
            }
        }
    }
}
