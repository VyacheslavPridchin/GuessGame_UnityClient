using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GuessGame.UnityClient.Network.Services
{
    public class GameRoomService
    {
        [Serializable]
        public class Guess
        {
            public Guess(Guid playerId, int number)
            {
                PlayerId = playerId;
                Number = number;
            }

            public Guid PlayerId { get; private set; }
            public int Number { get; private set; }
        }

        public async Task Join(Guid playerId, Guid roomId)
        {
            string endpoint = $"gameroom/{roomId}/join";
            var response = await NetworkManager.Instance.PostAsync<bool, Guid>(endpoint, new Payload<Guid>(playerId));
            if (response.StatusCode != 200)
            {
                Debug.LogError($"Join error: {response.ErrorMessage}");
            }
        }

        public async Task GuessNumber(int number, Guid playerId, Guid roomId)
        {
            string endpoint = $"gameroom/{roomId}/guess_number";
            var response = await NetworkManager.Instance.PostAsync<bool, Guess>(endpoint, new Payload<Guess>(new Guess(playerId, number)));
            if (response.StatusCode != 200)
            {
                Debug.LogError($"GuessNumber error: {response.ErrorMessage}");
            }
        }

        public async Task<List<Guid>> GetPlayers(Guid roomId)
        {
            string endpoint = $"gameroom/{roomId}/get_players";
            var response = await NetworkManager.Instance.GetAsync<List<Guid>>(endpoint);
            if (response.StatusCode == 200)
            {
                return response.Data;
            }
            else
            {
                Debug.LogError($"GetPlayers error: {response.ErrorMessage}");
                return new List<Guid>();
            }
        }

        public async Task<Dictionary<Guid, int>> GetAnswers(Guid roomId)
        {
            string endpoint = $"gameroom/{roomId}/get_answers";
            var response = await NetworkManager.Instance.GetAsync<Dictionary<Guid, int>>(endpoint);
            if (response.StatusCode == 200)
            {
                return response.Data;
            }
            else
            {
                Debug.LogError($"UpdateAnswers error: {response.ErrorMessage}");
                return new Dictionary<Guid, int>();
            }
        }

        public async Task<string> GetCurrentAction(Guid roomId)
        {
            string endpoint = $"gameroom/{roomId}/get_current_action";
            var response = await NetworkManager.Instance.GetAsync<string>(endpoint);
            if (response.StatusCode == 200)
            {
                return response.Data;
            }
            else
            {
                Debug.LogError($"UpdateCurrentAction error: {response.ErrorMessage}");
                return string.Empty;
            }
        }

        public async Task<List<Guid>> GetWinners(Guid roomId)
        {
            string endpoint = $"gameroom/{roomId}/get_winners";
            var response = await NetworkManager.Instance.GetAsync<List<Guid>>(endpoint);
            if (response.StatusCode == 200)
            {
                return response.Data;
            }
            else
            {
                Debug.LogError($"GetWinners error: {response.ErrorMessage}");
                return new List<Guid>();
            }
        }

        public async Task<int> GetTrueNumber(Guid roomId)
        {
            string endpoint = $"gameroom/{roomId}/get_true_number";
            var response = await NetworkManager.Instance.GetAsync<int>(endpoint);
            if (response.StatusCode == 200)
            {
                return response.Data;
            }
            else
            {
                Debug.LogError($"GetWinners error: {response.ErrorMessage}");
                return int.MinValue;
            }
        }
    }
}