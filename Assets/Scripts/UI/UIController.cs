using Altterra.Navigation;
using GuessGame.UnityClient.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [field: SerializeField]
    public ScreensNavigation ScreensNavigation { get; private set; }

    [field: SerializeField]
    public ReceptionUI ReceptionUI { get; private set; }

    [field: SerializeField]
    public QueueUI QueueUI { get; private set; }

    [field: SerializeField]
    public GameRoomUI GameRoomUI { get; private set; }
    public static UIController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogWarning("Instance of 'UIController' already exists!");
            Destroy(this);
        }
    }

    public void OpenReception()
    {
        ScreensNavigation.ShowScreen("Reception");
    }

    public void OpenQueue()
    {
        ScreensNavigation.ShowScreen("Queue");
    }

    public void OpenGameRoom()
    {
        ScreensNavigation.ShowScreen("GameRoom");
    }
}
