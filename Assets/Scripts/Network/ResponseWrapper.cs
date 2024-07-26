using System;

namespace GuessGame.UnityClient.Network
{
    [Serializable]
    public class ResponseWrapper<T>
    {
        public int StatusCode;
        public string ErrorMessage;
        public T Data;
    }
}