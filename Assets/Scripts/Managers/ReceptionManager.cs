using Altterra.Utils;
using GuessGame.UnityClient.Network.Services;
using GuessGame.UnityClient.Network.States;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GuessGame.UnityClient.Managers
{
    public class ReceptionManager : Singleton<ReceptionManager>
    {
        private ReceptionService receptionService;
        public Guid? ID { get; private set; }
        public PlayerState PlayerState { get; private set; }
        public Dictionary<Guid, PlayerState> Players { get; private set; } = new Dictionary<Guid, PlayerState>();
        public Dictionary<Guid, long> Score { get; private set; } = new Dictionary<Guid, long>();

        private void Awake()
        {
            receptionService = new ReceptionService();
            StartUpdatePlayers();
        }

        public void StartUpdatePlayers()
        {
            InvokeRepeating("UpdatePlayers", 1f, 5f);
        }

        public async void CheckIn()
        {
            ID = await receptionService.CheckIn(UIController.Instance.ReceptionUI.GetNickname());

            if(ID !=  null && ID != Guid.Empty)
            {
                UIController.Instance.OpenQueue();
                QueueManager.Instance.StartUpdatePlayers();
            }
        }

        public async void GetPlayer()
        {
            if (ID != null)
            {
                PlayerState = await receptionService.GetPlayer(ID.Value);
            }
            else
            {
                Debug.LogError("Player RoomID is null.");
            }
        }

        public async void UpdatePlayers()
        {
            Players = await receptionService.GetPlayers();
            Score = await receptionService.GetScore();
            UIController.Instance.ReceptionUI.UpdatePlayers(Players, Score);
        }
    }
}