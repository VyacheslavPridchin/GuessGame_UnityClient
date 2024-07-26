using Altterra.Utils;
using GuessGame.UnityClient.Managers;
using GuessGame.UnityClient.Network.Services;
using GuessGame.UnityClient.Network.States;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameRoomManager : Singleton<GameRoomManager>
{
    public enum CurrentActions { WaitPlayers, WaitAnswers, AnnouncementResults, Complete, Interrupted }

    private GameRoomService gameRoomService;
    private ReceptionService receptionService;
    public Guid? RoomID { get; private set; }

    public CurrentActions CurrentAction { get; private set; } = CurrentActions.WaitAnswers;

    private void Awake()
    {
        gameRoomService = new GameRoomService();
        receptionService = new ReceptionService();
    }

    public void SetID(Guid id)
    {
        RoomID = id;
    }

    public async void Join()
    {
        await gameRoomService.Join(ReceptionManager.Instance.ID.Value, RoomID.Value);
        UIController.Instance.GameRoomUI.SetID(RoomID.Value.ToString());

        InvokeRepeating("UpdateCurrentAction", 0f, 1f);
    }

    public async void UpdateCurrentAction()
    {
        string action = await gameRoomService.GetCurrentAction(RoomID.Value);

        if (Enum.TryParse(action, out CurrentActions currentAction))
        {
            CurrentAction = currentAction;
        }
        else return;


        switch (CurrentAction)
        {
            case CurrentActions.WaitPlayers:
                UIController.Instance.GameRoomUI.SetHeader("Waiting for another players...");
                UpdatePlayers();
                break;
            case CurrentActions.WaitAnswers:
                Debug.Log("Guess!");
                UIController.Instance.GameRoomUI.SetHeader("Guess!");
                UIController.Instance.GameRoomUI.TurnOnGuess();
                UpdatePlayers();
                UpdateAnswers();
                break;
            case CurrentActions.AnnouncementResults:
                UIController.Instance.GameRoomUI.SetHeader($"Results! True number: {await GetTrueNumber()}");
                ShowResult();
                UIController.Instance.GameRoomUI.TurnOffGuess();

                CancelInvoke("UpdateCurrentAction");
                Invoke("GoToQueue", 5f);
                break;
            case CurrentActions.Complete:
                Debug.Log("Room is Complete");
                UIController.Instance.GameRoomUI.SetHeader("Room is Complete");
                CancelInvoke("UpdateCurrentAction");
                Invoke("GoToQueue", 5f);
                break;
            case CurrentActions.Interrupted:
                Debug.Log("Room is Interrupted");
                UIController.Instance.GameRoomUI.SetHeader("Room is Interrupted");
                CancelInvoke("UpdateCurrentAction");
                Invoke("GoToQueue", 5f);
                break;
        }
    }

    public async void UpdatePlayers()
    {
        var ids = await gameRoomService.GetPlayers(RoomID.Value);
        List<PlayerState> players = new List<PlayerState>();

        foreach (var id in ids)
            players.Add((await receptionService.GetPlayer(id)));

        UIController.Instance.GameRoomUI.UpdatePlayers(players);
    }

    private async void ShowResult()
    {
        var winners = await gameRoomService.GetWinners(RoomID.Value);
        UIController.Instance.GameRoomUI.UpdateWinners(winners);
    }

    private async void UpdateAnswers()
    {
        var answers = await gameRoomService.GetAnswers(RoomID.Value);
        UIController.Instance.GameRoomUI.UpdateAnswers(answers);
    }

    public async void SendAnswer()
    {
        int answer = 0;

        if (UIController.Instance.GameRoomUI.TryGetAnswer(out answer))
        {
            await gameRoomService.GuessNumber(answer, ReceptionManager.Instance.ID.Value, RoomID.Value);
            UIController.Instance.GameRoomUI.TurnOffGuess();
        }
    }

    public async Task<int> GetTrueNumber()
    {
        return await gameRoomService.GetTrueNumber(RoomID.Value);
    }

    public void GoToQueue()
    {
        UIController.Instance.GameRoomUI.Clear();
        UIController.Instance.OpenQueue();
        QueueManager.Instance.StartUpdatePlayers();
    }
}
