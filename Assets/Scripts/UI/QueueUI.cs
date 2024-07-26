using GuessGame.UnityClient.Managers;
using GuessGame.UnityClient.Network.States;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueueUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform panel;

    [SerializeField]
    private GameObject playerPrefab;

    public void UpdatePlayers(List<string> players)
    {
        for (int i = panel.childCount - 1; i >= 0; i--)
        {
            Destroy(panel.GetChild(i).gameObject);
        }

        foreach (var player in players)
        {
            GameObject go = Instantiate(playerPrefab);
            PlayerUI playerUI = go.GetComponentInChildren<PlayerUI>();
            playerUI.SetPlayerNickname(player);
            playerUI.SetPlayerStatus("Waiting...");
            go.transform.SetParent(panel.transform, false);
        }
    }
}
