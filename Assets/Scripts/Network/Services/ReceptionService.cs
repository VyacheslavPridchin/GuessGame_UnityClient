using GuessGame.UnityClient.Network.States;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GuessGame.UnityClient.Network.Services
{
    public class ReceptionService
    {
        public async Task<Guid> CheckIn(string nickname)
        {
            string endpoint = $"reception/check_in";
            var response = await NetworkManager.Instance.PostAsync<Guid, string>(endpoint, new Payload<string>(nickname));
            if (response.StatusCode == 200)
            {   
                return response.Data;
            }
            else
            {
                Debug.LogError($"CheckIn error: {response.ErrorMessage}");
                throw new Exception(response.ErrorMessage);
            }
        }

        public async Task CheckOut(Guid playerId)
        {
            string endpoint = $"reception/check_out";
            var response = await NetworkManager.Instance.PostAsync<bool, Guid>(endpoint, new Payload<Guid>(playerId));
            if (response.StatusCode != 200)
            {
                Debug.LogError($"CheckOut error: {response.ErrorMessage}");
                throw new Exception(response.ErrorMessage);
            }
        }

        public async Task<Dictionary<Guid, PlayerState>> GetPlayers()
        {
            string endpoint = "reception/get_players";
            var response = await NetworkManager.Instance.GetAsync<Dictionary<Guid, PlayerState>>(endpoint);
            if (response.StatusCode == 200)
            {
                return response.Data;
            }
            else
            {
                Debug.LogError($"GetPlayers error: {response.ErrorMessage}");
                throw new Exception(response.ErrorMessage);
            }
        }

        public async Task<Dictionary<Guid, long>> GetScore()
        {
            string endpoint = "reception/get_score";
            var response = await NetworkManager.Instance.GetAsync<Dictionary<Guid, long>>(endpoint);
            if (response.StatusCode == 200)
            {
                return response.Data;
            }
            else
            {
                Debug.LogError($"GetPlayers error: {response.ErrorMessage}");
                throw new Exception(response.ErrorMessage);
            }
        }

        public async Task<PlayerState> GetPlayer(Guid playerId)
        {
            string endpoint = $"reception/get_player?playerId={playerId}";
            var response = await NetworkManager.Instance.GetAsync<PlayerState>(endpoint);
            if (response.StatusCode == 200)
            {
                return response.Data;
            }
            else
            {
                Debug.LogError($"GetPlayer error: {response.ErrorMessage}");
                throw new Exception(response.ErrorMessage);
            }
        }
    }
}