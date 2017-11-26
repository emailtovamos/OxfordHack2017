using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

namespace CSHttpClientSample
{
	static class Program
	{

		const string subscriptionKey = "052d61decbca441bb8d5b5fb6ce48413";

		const string uriBase = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/analyze";


		public static void Main()
		{
			Console.WriteLine("Analyze an image:");
			Console.Write("Enter the path to an image you wish to analzye: ");
			string imageFilePath = "shot.png";

			MakeAnalysisRequest(imageFilePath);

			Console.ReadLine();
		}

		static void MakeAnalysisRequest(string imageFilePath)
		{
			UnityWebRequest client = new UnityWebRequest();

			client.SetRequestHeader("Ocp-Apim-Subscription-Key", subscriptionKey);

			string requestParameters = "visualFeatures=Categories,Description,Color&language=en";

			string uri = uriBase + "?" + requestParameters;

			byte[] byteData = GetImageAsByteArray(imageFilePath);

			UploadHandlerRaw MyUploadHandler = new UploadHandlerRaw( byteData );

			MyUploadHandler.contentType= "application/x-www-form-urlencoded"; // might work with 'multipart/form-data'

			client.uploadHandler= MyUploadHandler;

			client.url = uri;

			client.SendWebRequest ();

			Debug.Log(client.downloadHandler.text);

			/* using (ByteArrayContent content = new ByteArrayContent(byteData))
			{
				content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

				response = await client.PostAsync(uri, content);

				string contentString = await response.Content.ReadAsStringAsync();

				Console.WriteLine("\nResponse:\n");
				Console.WriteLine(JsonPrettyPrint(contentString));
			}*/
		}

		static byte[] GetImageAsByteArray(string imageFilePath)
		{
			FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
			BinaryReader binaryReader = new BinaryReader(fileStream);
			return binaryReader.ReadBytes((int)fileStream.Length);
		}


		static string JsonPrettyPrint(string json)
		{
			if (string.IsNullOrEmpty(json))
				return string.Empty;

			json = json.Replace(Environment.NewLine, "").Replace("\t", "");

			StringBuilder sb = new StringBuilder();
			bool quote = false;
			bool ignore = false;
			int offset = 0;
			int indentLength = 3;

			foreach (char ch in json)
			{
				switch (ch)
				{
				case '"':
					if (!ignore) quote = !quote;
					break;
				case '\'':
					if (quote) ignore = !ignore;
					break;
				}

				if (quote)
					sb.Append(ch);
				else
				{
					switch (ch)
					{
					case '{':
					case '[':
						sb.Append(ch);
						sb.Append(Environment.NewLine);
						sb.Append(new string(' ', ++offset * indentLength));
						break;
					case '}':
					case ']':
						sb.Append(Environment.NewLine);
						sb.Append(new string(' ', --offset * indentLength));
						sb.Append(ch);
						break;
					case ',':
						sb.Append(ch);
						sb.Append(Environment.NewLine);
						sb.Append(new string(' ', offset * indentLength));
						break;
					case ':':
						sb.Append(ch);
						sb.Append(' ');
						break;
					default:
						if (ch != ' ') sb.Append(ch);
						break;
					}
				}
			}

			return sb.ToString().Trim();
		}
	}
}
