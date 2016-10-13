using UnityEngine;
using System.Collections;

public class TServerScripts : MonoBehaviour{

    private string url;
    private WWWForm post;
    private WWW server;

    public void SaveWebData(string filename, string data)
    {
        StartCoroutine(WriteStringToServer(filename, data));
    }

	private IEnumerator WriteStringToServer(string fileName, string data)
    {
        url = "http://neuroscapelab.com/tovagame/save.php";

        post = new WWWForm();
        post.AddField("filename", fileName);
        post.AddField("results", data);

        server = new WWW(url, post);

        yield return server;

        if (server.error != null)
            print("unity error: " + server.error);
        //else
            //print(server.text);

        server.Dispose();
    }
}
