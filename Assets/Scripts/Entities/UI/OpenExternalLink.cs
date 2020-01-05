using UnityEngine;

public class OpenExternalLink : MonoBehaviour
{
    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }
}
