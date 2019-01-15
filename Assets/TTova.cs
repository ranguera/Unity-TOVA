using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TTova : MonoBehaviour {

    public float minIsi = .3f;
    public float maxISI = 1.5f;
    public float presentationTime = .1f;
    public float responseTime = 1f;
    public int num_go;
    public int num_nogo;
    public int num_rounds_per_condition = 2;
    public Renderer topTarget;
    public Renderer bottomTarget;
    public Image topMarker, bottomMarker;
    public Text fixationCross;

    private float isi;
    private int[] targets;
    private int state;
    private int globalIndex;
    private bool userPressed;
    private bool responseTriggered;
    private float responseSpeed;
    private string path;
    private string id;
    private bool success;
    private int round_index;
    private int round_counter;

    private TLocalSave localSave;
    //private LSLSender lslSender;

	// Use this for initialization
	void Start ()
    {
        targets = new int[num_go + num_nogo];

        // create the target arrays to randomize
        for (int i = 0; i < num_go; i++)
        {
            targets[i] = 1;
        }
        for (int i = num_go; i < num_go+num_nogo; i++)
        {
            targets[i] = 2;
        }
        
        ShuffleArray(ref targets);

        localSave = new TLocalSave();
        //lslSender = GameObject.Find("LSLSender").GetComponent<LSLSender>();

        //lslSender.SendLevelStart();
        
        globalIndex = 0;
        round_counter = 0;
        round_index = 0;
        state = 1;
        isi = Random.Range(minIsi, maxISI);

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();

        switch (state)
        {
                // ISI
            case 1:
                StartCoroutine("ISI");
                break;

                // Presentation
            case 2:
                StartCoroutine("Presentation");
                break;

                // Response
            case 3:
                StartCoroutine("Response");
                break;

                // Catch response
            case 4:
                CatchResponse();
                break;

            // Save data
            case 5:
                SaveTrialData();
                break;

            case 6:
                EndAndSave();
                break;

            case 7:
                StartCoroutine(SameConditionBreak());
                break;

            case 8:
                StartCoroutine(DifferentConditionBreak());
                break;

            // Save data
            case 9:
                StartCoroutine(End());
                break;

            default:
                break;
        }
	}

    private IEnumerator ISI()
    {
        state = 0;
        yield return new WaitForSeconds(isi);
        state = 2;
    }

    private IEnumerator Presentation()
    {
        state = 0;
        if (targets[globalIndex] == 1)
        {
            topTarget.enabled = true;
            //topMarker.color = Color.white;
            //lslSender.SendLSL(1);
        }
        else
        {
            bottomTarget.enabled = true;
            //bottomMarker.color = Color.white;
            //lslSender.SendLSL(2);
        }
        responseSpeed = Time.time;
        yield return new WaitForSeconds(presentationTime);
        topTarget.enabled = bottomTarget.enabled = false;
        //topMarker.color = bottomMarker.color = Color.black;
        state = 3;
    }

    private IEnumerator Response()
    {
        state = 4;
        yield return new WaitForSeconds(responseTime);
        if (targets[globalIndex] == 1 && !responseTriggered)
            success = false;
        else if (targets[globalIndex] == 2 && !responseTriggered)
            success = true;
        state = 5;
    }

    private void CatchResponse()
    {
        if( Input.GetKeyDown(KeyCode.Space) )
        {
            responseSpeed = Time.time - responseSpeed;
            //lslSender.SendLSLUserAction();
            responseTriggered = true;
            if (targets[globalIndex] == 1)
                success = true;
            else
                success = false;
            //state = 5;
        }
    }

    private void SaveTrialData()
    {
        System.DateTime d = System.DateTime.Now;
        localSave.SaveTrial(d, targets[globalIndex], responseTriggered, responseSpeed, success);
        responseTriggered = false;
        globalIndex++;
        isi = Random.Range(minIsi, maxISI);
        if (globalIndex > num_go + num_nogo - 1)
            state = 6;
        else
            state = 1;
    }

    private void EndAndSave()
    {
        localSave.SaveLocalData();
        //lslSender.SendLevelEnd();
        round_counter++;
        round_index++;

        if ( round_counter >= num_rounds_per_condition)
        {
            if( round_index >= num_rounds_per_condition*2)
            {
                state = 9;
            }
            else
            {
                round_counter = 0;
                state = 8;
            }
        }
        else
        {
            state = 7;
        }
            
    }

    private IEnumerator SameConditionBreak()
    {
        this.fixationCross.text = "Press ENTER to start the next round";
        globalIndex = 0;
        ShuffleArray(ref targets);

        while (!Input.GetKey(KeyCode.Return))
        {
            yield return null;
        }
        this.fixationCross.text = "X";
        this.state = 1;
    }
    
    private IEnumerator DifferentConditionBreak()
    {
        this.fixationCross.text = "These are the instructions\n" +
            "with as many lines as we need\n" + 
            "Separated by backslash-n";

        globalIndex = 0;
        targets = new int[num_go + num_nogo];

        // create the target arrays to randomize
        for (int i = 0; i < num_go; i++)
        {
            targets[i] = 2;
        }
        for (int i = num_go; i < num_go + num_nogo; i++)
        {
            targets[i] = 1;
        }

        ShuffleArray(ref targets);

        while (!Input.GetKey(KeyCode.Return))
        {
            yield return null;
        }

        this.fixationCross.text = "X";
        this.state = 1;
    }
    
    private IEnumerator End()
    {
        this.fixationCross.text = "Thanks for participating.\nPress ENTER exit application";

        while (!Input.GetKey(KeyCode.Return))
            yield return null;

        Application.Quit();
    }

    private void ShuffleArray(ref int[] c)
    {
        for (int t = 0; t < c.Length; t++)
        {
            int tmp = c[t];
            int r = UnityEngine.Random.Range(t, c.Length);
            c[t] = c[r];
            c[r] = tmp;
        }
    }
}