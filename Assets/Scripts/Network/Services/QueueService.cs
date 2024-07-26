using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GuessGame.UnityClient.Network.Services
{
    public class QueueService
    {
        public async Task Join(Guid playerId)
        {
            string endpoint = $"queue/join";
            var response = await NetworkManager.Instance.PostAsync<bool, Guid>(endpoint, new Payload<Guid>(playerId));
            if (response.StatusCode != 200)
            {
                Debug.LogError($"Join error: {response.ErrorMessage}");
            }
        }

        public async Task<List<Guid>> GetPlayers()
        {
            string endpoint = "queue/get_players";
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

        public async Task<Guid> GetInvitation(Guid playerId)
        {
            string endpoint = $"queue/get_invitation?playerId={playerId}";
            var response = await NetworkManager.Instance.GetAsync<Guid>(endpoint);
            if (response.StatusCode == 200)
            {
                return response.Data;
            }
            else
            {
                Debug.LogError($"GetInvitation error: {response.ErrorMessage}");
                return Guid.Empty;
            }
        }

        public async Task Leave(Guid playerId)
        {
            string endpoint = $"queue/leave";
            var response = await NetworkManager.Instance.PostAsync<bool, Guid>(endpoint, new Payload<Guid>(playerId));
            if (response.StatusCode != 200)
            {
                Debug.LogError($"Leave error: {response.ErrorMessage}");
            }
        }
    }
}