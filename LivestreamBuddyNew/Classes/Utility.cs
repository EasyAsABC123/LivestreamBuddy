using System;
using System.IO;
using System.Text;
using LobsterKnifeFight;

namespace LiveStreamBuddy.Classes
{
	public static class Utility
	{
		public static void GetAccessToken(User user, UserScope[] scopes, bool getNewAccessToken = false)
		{
			if (getNewAccessToken || string.IsNullOrEmpty(user.AccessToken))
			{
				var authWindow = new AuthWindow(user, scopes);
				bool? result = authWindow.ShowDialog();

				if (result.Value && !string.IsNullOrEmpty(authWindow.AccessToken))
				{
					user.AccessToken = authWindow.AccessToken;
				}

				DataFileManager.SetUser(user);
			}
		}

		public static void GetAccessToken(User user, bool getNewAccessToken = false)
		{
			GetAccessToken(user,
				new[] {UserScope.UserRead, UserScope.ChatLogin, UserScope.ChannelEditor, UserScope.ChannelCommercial},
				getNewAccessToken);
		}

		public static void ClearUserData(User user)
		{
			user.Name = string.Empty;
			user.AccessToken = string.Empty;
			user.AccessTokens.Clear();

			DataFileManager.SetUser(user);
		}

		public static void Log(Type originator, string methodName, string message, string stacktrace)
		{
			try
			{
				DateTime now = DateTime.Now;
				string logFilename = string.Format("log{0}{1}{2}.txt",
					now.Year,
					now.Month,
					now.Day);

				using (StreamWriter writer = File.AppendText(logFilename))
				{
					string originatorString = "Unknown";

					if (originator != null)
					{
						originatorString = originator.ToString();
					}

					if (string.IsNullOrEmpty(methodName))
					{
						methodName = "Unknown()";
					}

					var sb = new StringBuilder();
					sb.AppendFormat("{0}:{1}:{2}:{3} - {4}.{5} - {6}",
						now.Hour,
						now.Minute,
						now.Second,
						now.Millisecond,
						originatorString,
						methodName,
						message);

					if (!string.IsNullOrEmpty(stacktrace))
					{
						sb.Append("\r\nStacktrace: " + stacktrace);
					}

					writer.WriteLine(sb.ToString());
				}
			}
			catch
			{
			}
		}
	}
}