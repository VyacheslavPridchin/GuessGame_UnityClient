using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GuessGame.UnityClient.Network
{
    [Serializable]
    public class Payload<T>
    {
        public Payload(T data)
        {
            Data = data;
        }

        public T Data;

        public string GetJson()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
