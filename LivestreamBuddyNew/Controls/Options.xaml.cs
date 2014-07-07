using System;
using System.Windows;
using System.Windows.Controls;
using LiveStreamBuddy.Classes;

namespace LiveStreamBuddy.Controls
{
	/// <summary>
	///     Interaction logic for Options.xaml
	/// </summary>
	public partial class Options : UserControl
	{
		public Options()
		{
			InitializeComponent();

			UserOptions = DataFileManager.GetOptions();

			chkStreamOpenStyle.IsChecked = UserOptions.OpenStreamsInNewTab;
			chkStreamViewShow.IsChecked = UserOptions.ShowStreamFeedWhenOpening;
			chkStreamShowChatTimestamps.IsChecked = UserOptions.ShowTimestampsInChat;
			chkEnableDebugLogging.IsChecked = UserOptions.EnableDebugLogging;
			txtChatTextSize.Text = UserOptions.ChatTextSize.ToString();
			chkShowEmoticonsInChat.IsChecked = UserOptions.ShowEmoticonsInChat;
			chkLogAllIRCTraffic.IsChecked = UserOptions.LogAllIRCTraffic;
		}

		# region Public Members

		public Classes.Options UserOptions { get; set; }

		# endregion

		# region Event Handlers

		private void ButtonSaveClick(object sender, RoutedEventArgs e)
		{
			string isValid = string.Empty;

			UserOptions.OpenStreamsInNewTab = chkStreamOpenStyle.IsChecked.Value;
			UserOptions.ShowStreamFeedWhenOpening = chkStreamViewShow.IsChecked.Value;
			UserOptions.ShowTimestampsInChat = chkStreamShowChatTimestamps.IsChecked.Value;
			UserOptions.EnableDebugLogging = chkEnableDebugLogging.IsChecked.Value;
			UserOptions.LogAllIRCTraffic = chkLogAllIRCTraffic.IsChecked.Value;

			int chatTextSize;
			if (int.TryParse(txtChatTextSize.Text, out chatTextSize))
			{
				UserOptions.ChatTextSize = chatTextSize;
			}
			else
			{
				isValid += "You must use real numbers for the 'Chat text size' field." + Environment.NewLine;
			}

			UserOptions.ShowEmoticonsInChat = chkShowEmoticonsInChat.IsChecked.Value;

			if (string.IsNullOrEmpty(isValid))
			{
				DataFileManager.SetOptions(UserOptions);
				DoOnSaved();
			}
			else
			{
				MessageBox.Show(isValid);
			}
		}

		# endregion

		# region Custom Events

		public delegate void OptionsSavedHandler(object sender, EventArgs e);

		public event OptionsSavedHandler OnSaved;

		private void DoOnSaved()
		{
			if (OnSaved != null)
			{
				OnSaved(this, null);
			}
		}

		# endregion
	}
}