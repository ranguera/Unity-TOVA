using UnityEngine;
using System.Collections;
using LSL;

public class LSLSender : MonoBehaviour {

    private double[] rhythmlsl;
    private liblsl.StreamInfo info;
    private liblsl.StreamOutlet outlet;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        rhythmlsl = new double[1];
        info = new liblsl.StreamInfo("TOVA", "Markers", 1);
        outlet = new liblsl.StreamOutlet(info);
    }

    void Start()
    {
        
    }

    public void SendLSLUserAction()
    {
        rhythmlsl[0] = (double)0f;
        outlet.push_sample(rhythmlsl);
    }

    public void SendLSL(int op)
    {
        StartCoroutine(SendStimulusMarker(op));
    }
    
    public void SendLevelStart()
    {
        StartCoroutine(SendStimulusMarker(3));
    }

    public void SendLevelEnd()
    {
        StartCoroutine(SendStimulusMarker(4));
    }

    private IEnumerator SendStimulusMarker(int caseOption)
    {
        yield return new WaitForEndOfFrame();
        rhythmlsl[0] = (double)caseOption;
        outlet.push_sample(rhythmlsl);
    }
}
