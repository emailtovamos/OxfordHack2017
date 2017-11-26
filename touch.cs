using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class touch : MonoBehaviour {
	public int resWidth;
	public int resHeight;

	public bool takeHiResShot;

	const string subscriptionKey = "052d61decbca441bb8d5b5fb6ce48413";

	const string uriBase = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/analyze";
	string imageFilePath = "./shot.png";
	string responseData;


	public void Start(){
		takeHiResShot = false;
	}

	public void FixedUpdate()
	{
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)//Input.GetMouseButtonUp (0))
		{
			TakeShot ();

		}
	}

	void TakeShot(){
		Debug.Log("Getting Vision Data...");
		ScreenCapture.CaptureScreenshot ("shot.png");
		StartCoroutine (GetVisionDataFromImages());
	}

	static byte[] GetImageAsByteArray(string imageFilePath)
	{
		FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
		BinaryReader binaryReader = new BinaryReader(fileStream);
		return binaryReader.ReadBytes((int)fileStream.Length);
	}

	IEnumerator GetVisionDataFromImages()
	{
		byte[] bytes = GetImageAsByteArray(imageFilePath);

		string requestParameters = "visualFeatures=Categories,Description,Color&language=en";

		string uri = uriBase + "?" + requestParameters;


		var headers = new Dictionary<string, string>() {
			{ "Ocp-Apim-Subscription-Key", subscriptionKey },
			{ "Content-Type", "application/octet-stream" }
		};

		WWW www = new WWW(uri, bytes, headers);
		yield return www;
		responseData = www.text; // Save the response as JSON string
		Debug.Log(responseData);

		GameObject.Find("speech").GetComponent<TextMesh>().text = "TESTING";
			//+findValue(responseData);
		//Debug.Log (findValue(responseData));



	}

	static string findValue(string responseData){
		int idx = responseData.IndexOf ("\"text\":");
		int idx2 = responseData.IndexOf ("\"confidence");
		return responseData.Substring(idx + 8, idx2 - idx - 10);
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