using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CallBackendService : MonoBehaviour
{
    public string apiUrl = "https://**REPLACE**/test/endpoint"; // Replace with your actual API URL

    // Define the structure of the JSON object
    [Serializable]
    public class JsonObject
    {
        public string testValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Start the coroutine to make an API call and retrieve the JSON data
        StartCoroutine(GetJsonFromApi());
    }

    // Coroutine to retrieve JSON from the API
    IEnumerator GetJsonFromApi()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            // Disable SSL certificate verification (ONLY FOR DEVELOPMENT)
            webRequest.certificateHandler = new BypassCertificate();

            // Send the request and wait for a response
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error retrieving JSON: " + webRequest.error);
            }
            else
            {
                string jsonContent = webRequest.downloadHandler.text;
                Debug.Log("Received JSON: " + jsonContent);
            }
        }
    }

    // Custom certificate handler to bypass SSL verification
    private class BypassCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }

    // Method to parse the JSON contentS
    public void ParseJsonContent(string jsonContent)
    {
        try
        {
            // Deserialize the JSON content directly into an array
            JsonObject[] jsonObjectArray = JsonHelper.FromJson<JsonObject>(jsonContent);

            // Convert the array to a list
            List<JsonObject> jsonObjectList = new List<JsonObject>(jsonObjectArray);

            // Check for null or empty to ensure deserialization was successful
            if (jsonObjectList == null || jsonObjectList.Count == 0)
            {
                Debug.LogError("No JsonObjects found in the JSON response.");
                return;
            }

            // Iterate through the list and log each object's data
            foreach (JsonObject obj in jsonObjectList)
            {
                Debug.Log($"testValue: {obj.testValue}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error parsing JSON: " + e.Message);
        }
    }
}

// Helper class for JSON array deserialization
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        // Wrap the JSON array in an object to enable deserialization
        string wrappedJson = "{ \"Items\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(wrappedJson);
        return wrapper.Items;
    }

    // Wrapper class
    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
