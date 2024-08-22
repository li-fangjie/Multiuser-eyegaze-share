using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;

public class VoiceCommands : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> actions = new Dictionary<string, System.Action>();
    public GameObject controlledObject;

    // Start is called before the first frame update
    void Start()
    { 
        actions.Add("hide", Hide);
        actions.Add("show", Show);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }

    // Update is called once per frame
    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        System.Action keywordAction;
        // if the keyword recognized is in our dictionary, call that Action.
        if (actions.TryGetValue(speech.text, out keywordAction))
        {
            Debug.Log(speech.text);
            keywordAction.Invoke();

        }
    }
    private void Hide()
    {
        controlledObject.GetComponent<Renderer>().enabled = false;
    }

    private void Show()
    {
        controlledObject.GetComponent<Renderer>().enabled = true;
    }
}
