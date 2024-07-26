using System;

namespace GuessGame.UnityClient.Network.States
{
    public class PlayerState
    {
        public PlayerState() { }
        public PlayerState(string name, Guid id)
        {
            ID = id;
            Nickname = name;
        }

        public Guid ID { get; set; } = Guid.Empty;
        public string Nickname { get; set; } = string.Empty;
    }
}