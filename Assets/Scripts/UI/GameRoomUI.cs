using GuessGame.UnityClient.Network.States;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameRoomUI : MonoBehaviour
{
    [SerializeField]
    private Text header;

    [SerializeField]
    private Text id;

    [SerializeField]
    private RectTransform panel;

    [SerializeField]
    private InputField answerInputField;

    [SerializeField]
    private Button guessButton;

    [SerializeField]
    private GameObject playerPrefab;

    private Dictionary<PlayerState, PlayerUI> players = new Dictionary<PlayerState, PlayerUI>();

    public void Clear()
    {
        for (int i = panel.childCount - 1; i >= 0; i--)
        {
            Destroy(panel.GetChild(i).gameObject);
        }
        players.Clear();
        header.text = "Guess Game";
        id.text = "ID";
        answerInputField.text = string.Empty;
        TurnOffGuess();
    }

    public void SetHeader(string text)
    {
        header.text = text;
    }

    public void SetID(string text)
    {
        id.text = text;
    }

    public void UpdatePlayers(List<PlayerState> players)
    {
        foreach (var player in players)
        {
            if (this.players.Count(x => x.Key.ID == player.ID) == 0)
            {
                GameObject go = Instantiate(playerPrefab);
                PlayerUI playerUI = go.GetComponentInChildren<PlayerUI>();

                playerUI.SetPlayerNickname(player.Nickname);
                go.transform.SetParent(panel.transform, false);
                this.players.Add(player, playerUI);
            }
        }
    }

    public void UpdateWinners(List<Guid> winners)
    {
        foreach (var winner in winners)
        {
            var player = players.FirstOrDefault(x => x.Key.ID == winner);
            player.Value.SetPlayerStatus($"Winner!");
        }
    }

    public void UpdateAnswers(Dictionary<Guid, int> answers)
    {
        foreach (var answer in answers)
        {
            var player = players.FirstOrDefault(x => x.Key.ID == answer.Key);
            player.Value.SetPlayerStatus($"I think it's {answer.Value}!");
        }
    }

    public void TurnOnGuess()
    {
        guessButton.interactable = true;
        answerInputField.interactable = true;
    }

    public void TurnOffGuess()
    {
        guessButton.interactable = true;
        answerInputField.interactable = true;
    }

    public bool TryGetAnswer(out int answer)
    {
        int result = 0;

        if (int.TryParse(answerInputField.text, out result))
        {
            if (result < 0 | result > 100)
            {
                answer = int.MinValue;
                return false;
            } else
            {
                answer = result;
                return true;
            }
        }

        answer = int.MinValue;
        return false;
    }
}
