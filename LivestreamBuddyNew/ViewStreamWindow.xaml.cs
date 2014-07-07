using System;
using System.Windows;
using LiveStreamBuddy.Controls;

namespace LiveStreamBuddy
{
	/// <summary>
	///     Interaction logic for ViewStreamWindow.xaml
	/// </summary>
	public partial class ViewStreamWindow : Window
	{
		public ViewStreamWindow()
		{
			InitializeComponent();
		}

		public ViewStreamWindow(string channelName, double minimumHeight)
			: this()
		{
			Title = "View stream - " + channelName;
			ViewStream = new ViewStream(channelName, true, minimumHeight);
			container.Child = ViewStream;

			minimumHeight += 40;
			double width = minimumHeight*1.48;

			Width = width;
			Width = width;
			Height = minimumHeight;
			MinHeight = minimumHeight;
		}

		public ViewStreamWindow(ViewStream viewStream, string channelName, double minimumHeight)
			: this()
		{
			Title = "View stream - " + channelName;
			ViewStream = viewStream;
			container.Child = ViewStream;

			minimumHeight += 40;
			double width = minimumHeight*1.48;

			Width = width;
			Width = width;
			Height = minimumHeight;
			MinHeight = minimumHeight;
		}

		# region Public Members

		public ViewStream ViewStream;

		# endregion

		# region Private Members

		# endregion

		# region Event Handlers

		private void Window_Closed_1(object sender, EventArgs e)
		{
			container.Child = null;
		}

		# endregion
	}
}