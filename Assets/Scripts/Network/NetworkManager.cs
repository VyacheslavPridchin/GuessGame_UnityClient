using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace GuessGame.UnityClient.Network
{
    public class NetworkManager : MonoBehaviour
    {
        [field: SerializeField]
        public string BaseUrl { get; private set; }

        private readonly HttpClient client = new HttpClient();
        public static NetworkManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Debug.LogWarning("Instance of 'NetworkManager' already exists!");
                Destroy(this);
            }
        }

        public async Task<ResponseWrapper<T>> GetAsync<T>(string endpoint)
        {
            try
            {
                Debug.Log($"GET request: {BaseUrl}/{endpoint}");
                HttpResponseMessage response = await client.GetAsync($"{BaseUrl}/{endpoint}");
                string responseBody = await response.Content.ReadAsStringAsync();

                ResponseWrapper<T> wrappedResponse = JsonConvert.DeserializeObject<ResponseWrapper<T>>(responseBody);

                if (response.IsSuccessStatusCode)
                {
                    wrappedResponse.StatusCode = (int)response.StatusCode;
                    return wrappedResponse;
                }
                else
                {
                    Debug.LogError($"Error: {wrappedResponse.ErrorMessage}");
                    return new ResponseWrapper<T>
                    {
                        StatusCode = (int)response.StatusCode,
                        ErrorMessage = wrappedResponse.ErrorMessage
                    };
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"GetAsync error: {e.Message}");
                return new ResponseWrapper<T>
                {
                    StatusCode = 500,
                    ErrorMessage = e.Message
                };
            }
        }

        public async Task<ResponseWrapper<T1>> PostAsync<T1, T2>(string endpoint, Payload<T2> payload)
        {
            try
            {
                string jsonPayload = JsonConvert.SerializeObject(payload);
                HttpContent content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");
                Debug.Log($"POST request: {BaseUrl}/{endpoint}\nPayload:\n{jsonPayload}");

                HttpResponseMessage response = await client.PostAsync($"{BaseUrl}/{endpoint}", content);
                string responseBody = await response.Content.ReadAsStringAsync();

                ResponseWrapper<T1> wrappedResponse = JsonConvert.DeserializeObject<ResponseWrapper<T1>>(responseBody);

                if (response.IsSuccessStatusCode)
                {
                    wrappedResponse.StatusCode = (int)response.StatusCode;
                    return wrappedResponse;
                }
                else
                {
                    Debug.LogError($"Error: {wrappedResponse.ErrorMessage}");
                    return new ResponseWrapper<T1>
                    {
                        StatusCode = (int)response.StatusCode,
                        ErrorMessage = wrappedResponse.ErrorMessage
                    };
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"PostAsync error: {e.Message}");
                return new ResponseWrapper<T1>
                {
                    StatusCode = 500,
                    ErrorMessage = e.Message
                };
            }
        }
    }
}
