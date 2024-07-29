using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private Text nickname;

    [SerializeField]
    private Image image;

    [SerializeField]
    private Text status;

    public void SetPlayerStatus(string status)
    {
        this.status.text = status; 
    }

    public void SetPlayerNickname(string nickname)
    {
        this.nickname.text = nickname;
        StartCoroutine(SetImage($"https://api.dicebear.com/9.x/bottts/png?seed={nickname}&size=256"));
    }

    private IEnumerator SetImage(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error downloading image: " + request.error);
                StartCoroutine(SetImage(url));
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                image.sprite = sprite;
                image.preserveAspect = true;
            }
        }
    }
}
