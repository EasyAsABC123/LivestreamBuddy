using System;
using System.Linq;
using System.Windows;
using Awesomium.Core;
using LobsterKnifeFight;

namespace LiveStreamBuddy
{
    /// <summary>
	/// Interaction logic for AuthWindow.xaml client_id: a8t13c4i3clujv1irur7rfpu3u1weuz client_secret: rrm09e9t3lvjm023pvnb9lt6qhqvue9
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();

            this.saveAccessToken = true;
            this.isTwitchLoaded = false;
            this.windowClosed = false;

            webBrowser.NativeViewInitialized += webBrowser_NativeViewInitialized;
            webBrowser.DocumentReady += webBrowser_DocumentReady;
            webBrowser.LoadingFrame += webBrowser_LoadingFrame;
        }

        public AuthWindow(User user, UserScope[] scopes)
            : this()
        {
            this.user = user;
            this.Scopes = scopes;
        }

        # region Public Properties

        public string AccessToken { get; private set; }

        public UserScope[] Scopes { get; private set; }

        # endregion

        # region Private Properties

        private User user;
        private bool saveAccessToken;
        private bool isTwitchLoaded;
        private bool windowClosed;

        # endregion

        # region Events

        void webBrowser_NativeViewInitialized(object sender, WebViewEventArgs e)
        {
            webBrowser.LoadHTML("<html><body><h3>Loading...</h3></body></html>");

            string userscopes = Scopes.Aggregate(string.Empty, (current, scope) => current + (EnumHelper.GetUserScope(scope) + " "));

	        userscopes = userscopes.Remove(userscopes.Length - 1, 1);

			webBrowser.Source = new Uri("https://api.twitch.tv/kraken/oauth2/authorize?response_type=token&client_id=a8t13c4i3clujv1irur7rfpu3u1weuz&redirect_uri=http://xim.schucreations.com&scope=" + userscopes);
        }

	    private void webBrowser_LoadingFrame(object sender, LoadingFrameEventArgs e)
	    {
		    if (!webBrowser.IsDocumentReady)
			    return;

		    dynamic document = (JSObject) webBrowser.ExecuteJavascriptWithResult("document");

		    if (document == null)
			    return;

		    using (document)
		    {
			    dynamic userLoginField = document.getElementById("user_login");

			    if (userLoginField == null)
				    return;

			    using (userLoginField)
			    {
				    try
				    {
					    string value = (string) userLoginField.value;

					    if (!string.IsNullOrEmpty(value))
					    {
						    if (string.IsNullOrEmpty(user.Name))
						    {
							    user.Name = value;
						    }
						    else if (string.Compare(user.Name, value, StringComparison.OrdinalIgnoreCase) != 0)
						    {
							    this.saveAccessToken = false;
						    }
					    }
				    }
				    catch
				    {
					    this.saveAccessToken = true;
				    }
			    }
		    }
	    }

	    void webBrowser_DocumentReady(object sender, UrlEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Url.Fragment))
            {
                if (this.saveAccessToken)
                {
                    int keyIndex = e.Url.Fragment.IndexOf("access_token=");

                    if (keyIndex > -1)
                    {
                        int endIndex = e.Url.Fragment.IndexOf('&', keyIndex) - 14;

                        keyIndex += 13;
                        this.AccessToken = e.Url.Fragment.Substring(keyIndex, endIndex);
                    }

                    this.DialogResult = true;
                }
                else
                {
                    this.DialogResult = false;
                }

                this.windowClosed = true;
                this.Close();
            }
            else if (e.Url.Host.ToLower().Contains("twitch"))
            {
                this.isTwitchLoaded = true;
            }
            else if (e.Url.Host.ToLower().Contains("google") && !this.windowClosed)
            {
                this.DialogResult = false;
                this.Close();
            }
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!this.isTwitchLoaded)
            {
                e.Cancel = true;
            }
        }

        # endregion
    }
}
