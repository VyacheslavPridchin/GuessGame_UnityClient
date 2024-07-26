using Altterra.Utils;
using GuessGame.UnityClient.Managers;
using GuessGame.UnityClient.Network.Services;
using System;
using System.Collections.Generic;

public class QueueManager : Singleton<QueueManager>
{
    private QueueService queueService;
    private ReceptionService receptionService;

    private void Awake()
    {
        queueService = new QueueService();
        receptionService = new ReceptionService();
    }

    public async void Leave()
    {
        await queueService.Leave(ReceptionManager.Instance.ID.Value);
        StopUpdatePlayers();
        UIController.Instance.OpenReception();
    }

    public async void Join()
    {
        await queueService.Join(ReceptionManager.Instance.ID.Value);
        InvokeRepeating("GetInvitation", 1f, 1f);
    }

    public void StartUpdatePlayers()
    {
        InvokeRepeating("UpdatePlayers", 1f, 1f);
    }

    public void StopUpdatePlayers()
    {
        CancelInvoke("UpdatePlayers");
    }

    public void StopGetInvitation()
    {
        CancelInvoke("GetInvitation");
    }

    public async void UpdatePlayers()
    {
        var ids = await queueService.GetPlayers();
        List<string> players = new List<string>();

        foreach (var id in ids)
            players.Add((await receptionService.GetPlayer(id)).Nickname);

        UIController.Instance.QueueUI.UpdatePlayers(players);
    }


    public async void GetInvitation()
    {
        var id = await queueService.GetInvitation(ReceptionManager.Instance.ID.Value);

        if (id != null && id != Guid.Empty)
        {
            GameRoomManager.Instance.SetID(id);
            GameRoomManager.Instance.Join();
            UIController.Instance.OpenGameRoom();
            StopGetInvitation();
            StopUpdatePlayers();
        }
    }
}
