using Altterra.Navigation;
using GuessGame.UnityClient.Managers;
using GuessGame.UnityClient.Network.States;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GuessGame.UnityClient.UI
{
    public class ReceptionUI : MonoBehaviour
    {
        [SerializeField]
        private InputField nickname;

        [SerializeField]
        private RectTransform panel;

        [SerializeField]
        private GameObject playerPrefab;

        private Dictionary<PlayerState, PlayerUI> players = new Dictionary<PlayerState, PlayerUI>();

        public string GetNickname()
        {
            return nickname.text;
        }

        public void UpdatePlayers(Dictionary<Guid, PlayerState> players, Dictionary<Guid, long> score)
        {
            // Создаем список игроков, отсортированный по Score
            var sortedPlayers = players.OrderByDescending(player => score.ContainsKey(player.Key) ? score[player.Key] : 0).ToList();

            // Обрабатываем игроков в отсортированном порядке
            foreach (var player in sortedPlayers)
            {
                if (this.players.Count(x => x.Key.ID == player.Key) == 0)
                {
                    // Спавним нового игрока
                    GameObject go = Instantiate(playerPrefab);
                    PlayerUI playerUI = go.GetComponentInChildren<PlayerUI>();

                    playerUI.SetPlayerNickname(player.Value.Nickname);
                    playerUI.SetPlayerStatus($"Score: {(score.ContainsKey(player.Key) ? score[player.Key] : 0)}");

                    go.transform.SetParent(panel.transform, false);
                    this.players.Add(player.Value, playerUI);
                }
                else
                {
                    // Обновляем существующего игрока
                    PlayerUI playerUI = this.players.FirstOrDefault(x => x.Key.ID == player.Key).Value;
                    playerUI.SetPlayerStatus($"Score: {(score.ContainsKey(player.Key) ? score[player.Key] : 0)}");
                }
            }
        }
    }
}