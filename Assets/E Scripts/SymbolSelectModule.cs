using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class SymbolSelectModule : MonoBehaviour
{
    public KMSelectable Button1;
    public KMSelectable Button2;
    public KMSelectable Button3;

    public Material LED_On;
    public Material LED_Off;
    public MeshRenderer[] LEDs;
    static string[] Characters = {
        "〆",
        "Հ",
        "〽",
        "ﾁ",
        "չ",
        "ㄙ",
        "ᱨ",
        "ｾ",
        "৶",
        "ゝ",
        "ᱩ",
        "Է",
        "ծ",
        "Ꝭ"

};
    bool activated = false;


    string ButtonLabels = ""; // the labels of the buttons
    string OrderedSequence = ""; // the correct code
    string ButtonsSelected = ""; // the code that we're typing

    static int _moduleIdCounter = 1;
    int _moduleId;

    void Start()
    {
        _moduleId = _moduleIdCounter++;
        GetComponent<KMBombModule>().OnActivate += ActivateModule;

        // ButtonLabels = GenerateButtons(); // Generate Symbols
        // OrderedSequence = GenerateCorrectCode(); // Generate Solution
        KMSelectable[] buttons = GetComponent<KMSelectable>().Children;
        for (int i = 0; i < 3; i++)
        {
            buttons[i].GetComponentInChildren<TextMesh>().text = ButtonLabels.Substring(i, 1);
            int j = i;
            buttons[i].OnInteract += delegate ()
            {
                buttons[j].GetComponentInChildren<Animator>().SetTrigger("PushTrigger");
                // OnPress(j); //???
                return false;
            };
        }

    }
    public KMSelectable[] buttons;

    int correctIndex;
    bool isActivated = false;

    void Init()
    {
        correctIndex = Random.Range(0, 4);

        for(int i = 0; i < buttons.Length; i++)
        {
            string label = i == correctIndex ? "O" : "X";

            TextMesh buttonText = buttons[i].GetComponentInChildren<TextMesh>();
            buttonText.text = label;
            int j = i;
            buttons[i].OnInteract += delegate () { OnPress(j == correctIndex); return false; };
        }
    }

    void ActivateModule()
    {
        isActivated = true;
    }

    void OnPress(bool correctButton)
    {
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        GetComponent<KMSelectable>().AddInteractionPunch();

        if (!isActivated)
        {
            Debug.Log("Pressed button before module has been activated!");
            GetComponent<KMBombModule>().HandleStrike();
        }
        else
        {
            Debug.Log("Pressed " + correctButton + " button");
            if (correctButton)
            {
                GetComponent<KMBombModule>().HandlePass();
            }
            else
            {
                GetComponent<KMBombModule>().HandleStrike();
            }
        }
    }
}
