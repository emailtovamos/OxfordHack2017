using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;


public class translator : MonoBehaviour {

	const string subscriptionKey = "03b7226c83ea43aa8429d8548d28c85e";

	const string uriBase = "https://api.microsofttranslator.com/V2/Http.svc/Translate";
	string responseData;

	void Start() {
	}

	public void FixedUpdate()
	{
		if (Input.GetMouseButtonUp (0))
		{
			//StartCoroutine (getAuthoizationKey());

		}
	}

	public IEnumerator getAuthoizationKey(){
		WWWForm form = new WWWForm();

		form.AddField("Content-Type", "application/json");
		form.AddField("Accept", "application/jwt");
		form.AddField("Ocp-Apim-Subscription-Key", subscriptionKey);

		UnityWebRequest www = UnityWebRequest.Post("ttps://api.cognitive.microsoft.com/sts/v1.0/issueToken", form);
		yield return www.SendWebRequest();

		if(www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error);
		}
		else {
			Debug.Log("Form upload complete!");
		}
	}
		
	IEnumerator GetText() {
		Debug.Log ("GetText()");

		string translatingString = "Testing this is english";

	//	string my_autorization = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzY29wZSI6Imh0dHBzOi8vZGV2Lm1pY3Jvc29mdHRyYW5zbGF0b3IuY29tLyIsInN1YnNjcmlwdGlvbi1pZCI6IjJjZGIxZGYyOTk2OTQzZWE5NDdjMTYyNjg0NmMyMWNjIiwicHJvZHVjdC1pZCI6IlNwZWVjaFRyYW5zbGF0b3IuRjAiLCJjb2duaXRpdmUtc2VydmljZXMtZW5kcG9pbnQiOiJodHRwczovL2FwaS5jb2duaXRpdmUubWljcm9zb2Z0LmNvbS9pbnRlcm5hbC92MS4wLyIsImF6dXJlLXJlc291cmNlLWlkIjoiL3N1YnNjcmlwdGlvbnMvNTZjODkyNjctZmYxZC00N2FkLTgzYTAtZWZkZmZhZmNjZThjL3Jlc291cmNlR3JvdXBzL1Zpc3VhbFRyYW5zbGF0b3IvcHJvdmlkZXJzL01pY3Jvc29mdC5Db2duaXRpdmVTZXJ2aWNlcy9hY2NvdW50cy9Kb3NlIiwiaXNzIjoidXJuOm1zLmNvZ25pdGl2ZXNlcnZpY2VzIiwiYXVkIjoidXJuOm1zLm1pY3Jvc29mdHRyYW5zbGF0b3IiLCJleHAiOjE1MTE2NjcyODJ9.VeTQBeNtlhpoSGbtPvqDXCHFHZZsqJWETyindBKvOdE";

		string from = "en-US";
		string to = "es-CO";

		string requestParameters = "text="+ translatingString +"from="+ from +"&to="+ to;
		string uri = uriBase + "?" + requestParameters;

		var headers = new Dictionary<string, string>() {
			{ "text", translatingString},
//			{ "Authorization", "Bearer "+ StartCoroutine(getAuthoizationKey)},
			{ "Content-Type", "application/octet-stream" }
		};

		WWW www = new WWW(uri, null, headers);
		yield return www;
		responseData = www.text; // Save the response as JSON string
		Debug.Log(responseData);
	}


}