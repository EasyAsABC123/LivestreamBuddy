using System.Windows;
using System.Windows.Controls;
using Awesomium.Core;

namespace LiveStreamBuddy.Controls
{
	/// <summary>
	///     Interaction logic for ViewStream.xaml
	/// </summary>
	public partial class ViewStream : UserControl
	{
		public ViewStream()
		{
			InitializeComponent();

			webViewStream.SizeChanged += webViewStream_SizeChanged;
			webViewStream.NativeViewInitialized += webViewStream_NativeViewInitialized;

			IsShowing = false;
		}

		public ViewStream(string channelName, bool isWindow, double minimumHeight, bool show = true)
			: this()
		{
			this.channelName = channelName;
			this.minimumHeight = minimumHeight;
			this.isWindow = isWindow;
			showOnLoad = show;

			if (isWindow)
			{
				webViewStream.ViewType = WebViewType.Window;
			}

			setWebViewStreamHeight(this.minimumHeight);
		}

		# region Private Members

		private readonly string channelName;
		private readonly bool isWindow;
		private readonly double minimumHeight;
		private bool showOnLoad;

		# endregion

		# region Public Members

		public bool IsShowing { get; set; }

		# endregion

		# region Private Methods

		private void loadHTML()
		{
			webViewStream.DocumentReady += webViewStream_DocumentReady;

			string allowFullscreen = "false";

			if (isWindow)
			{
				allowFullscreen = "true";
			}

			if (showOnLoad)
			{
				IsShowing = true;
				webViewStream.LoadHTML(
					"<html><head><script>function resizePlayer(width, height){var player=document.getElementById('live_embed_player_flash');if (width==-1){width=window.innerWidth - 16;}if (height==-1){height=window.innerHeight - 16;}player.style.width=width + 'px';player.style.maxWidth=width + 'px';player.style.height=height + 'px';player.style.maxHeight=height + 'px';}</script></head><body><object type='application/x-shockwave-flash' height='271' width='456' id='live_embed_player_flash' data='http://www.twitch.tv/widgets/live_embed_player.swf?channel=" +
					channelName + "' bgcolor='#000000'><param name='allowFullScreen' value='" + allowFullscreen +
					"'/><param name='allowScriptAccess' value='always'/><param name='allowNetworking' value='all'/><param name='movie' value='http://www.twitch.tv/widgets/live_embed_player.swf'/><param name='flashvars' value='hostname=www.twitch.tv&channel=" +
					channelName + "&auto_play=true&start_volume=25'/></object></body></html>");
			}
			else
			{
				Hide();
				showOnLoad = true;
			}
		}

		private void setWebViewStreamHeight(double height)
		{
			webViewStream.Height = height;
		}

		# endregion

		# region Public Methods

		public void Shutdown()
		{
			if (!webViewStream.IsDisposed)
			{
				webViewStream.Dispose();
			}
		}

		public void Hide()
		{
			IsShowing = false;
			webViewStream.LoadHTML("<html><head></head><body></body></html>");
		}

		public void Show()
		{
			loadHTML();
		}

		# endregion

		# region Event Handlers

		private void webViewStream_NativeViewInitialized(object sender, WebViewEventArgs e)
		{
			var navigationInterceptor = webViewStream.GetService(typeof (INavigationInterceptor)) as INavigationInterceptor;

			if (navigationInterceptor != null)
			{
				navigationInterceptor.AddRule("*", NavigationRule.Deny);
			}

			loadHTML();
		}

		private void webViewStream_DocumentReady(object sender, UrlEventArgs e)
		{
			webViewStream.DocumentReady -= webViewStream_DocumentReady;

			try
			{
				webViewStream.ExecuteJavascript("resizePlayer(-1, -1)");
			}
			catch
			{
			}
		}

		private void webViewStream_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (webViewStream.IsDocumentReady)
			{
				webViewStream.ExecuteJavascript("resizePlayer(" + (webViewStream.ActualWidth - 16) + ", " +
				                                (webViewStream.ActualHeight - 16) + ")");
			}
		}

		private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
		{
			if (e.PreviousSize.Height > 0)
			{
				double newHeight = e.NewSize.Height;

				if (newHeight >= minimumHeight)
				{
					setWebViewStreamHeight(newHeight);
				}
				else
				{
					setWebViewStreamHeight(minimumHeight);
				}
			}
		}

		# endregion
	}
}