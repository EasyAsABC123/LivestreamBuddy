using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Awesomium.Core;
using LobsterKnifeFight;

namespace LiveStreamBuddy
{
	/// <summary>
	///     Interaction logic for AuthWindow.xaml client_id: a8t13c4i3clujv1irur7rfpu3u1weuz client_secret:
	///     rrm09e9t3lvjm023pvnb9lt6qhqvue9
	/// </summary>
	public partial class AuthWindow : Window
	{
		public AuthWindow()
		{
			InitializeComponent();

			saveAccessToken = true;
			isTwitchLoaded = false;
			windowClosed = false;

			webBrowser.NativeViewInitialized += webBrowser_NativeViewInitialized;
			webBrowser.DocumentReady += webBrowser_DocumentReady;
			webBrowser.LoadingFrame += webBrowser_LoadingFrame;
		}

		public AuthWindow(User user, UserScope[] scopes)
			: this()
		{
			this.user = user;
			Scopes = scopes;
		}

		# region Public Properties

		public string AccessToken { get; private set; }

		public UserScope[] Scopes { get; private set; }

		# endregion

		# region Private Properties

		private readonly User user;
		private bool isTwitchLoaded;
		private bool saveAccessToken;
		private bool windowClosed;

		# endregion

		# region Events

		private void webBrowser_NativeViewInitialized(object sender, WebViewEventArgs e)
		{
			webBrowser.LoadHTML("<html><body><h3>Loading...</h3></body></html>");

			string userscopes = Scopes.Aggregate(string.Empty,
				(current, scope) => current + (EnumHelper.GetUserScope(scope) + " "));

			userscopes = userscopes.Remove(userscopes.Length - 1, 1);

			webBrowser.Source =
				new Uri(
					"https://api.twitch.tv/kraken/oauth2/authorize?response_type=token&client_id=a8t13c4i3clujv1irur7rfpu3u1weuz&redirect_uri=http://xim.schucreations.com&scope=" +
					userscopes);
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
						var value = (string) userLoginField.value;

						if (!string.IsNullOrEmpty(value))
						{
							if (string.IsNullOrEmpty(user.Name))
							{
								user.Name = value;
							}
							else if (string.Compare(user.Name, value, StringComparison.OrdinalIgnoreCase) != 0)
							{
								saveAccessToken = false;
							}
						}
					}
					catch
					{
						saveAccessToken = true;
					}
				}
			}
		}

		private void webBrowser_DocumentReady(object sender, UrlEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.Url.Fragment))
			{
				if (saveAccessToken)
				{
					int keyIndex = e.Url.Fragment.IndexOf("access_token=");

					if (keyIndex > -1)
					{
						int endIndex = e.Url.Fragment.IndexOf('&', keyIndex) - 14;

						keyIndex += 13;
						AccessToken = e.Url.Fragment.Substring(keyIndex, endIndex);
					}

					DialogResult = true;
				}
				else
				{
					DialogResult = false;
				}

				windowClosed = true;
				Close();
			}
			else if (e.Url.Host.ToLower().Contains("twitch"))
			{
				isTwitchLoaded = true;
			}
			else if (e.Url.Host.ToLower().Contains("google") && !windowClosed)
			{
				DialogResult = false;
				Close();
			}
		}

		private void Window_Closing_1(object sender, CancelEventArgs e)
		{
			if (!isTwitchLoaded)
			{
				e.Cancel = true;
			}
		}

		# endregion
	}
}