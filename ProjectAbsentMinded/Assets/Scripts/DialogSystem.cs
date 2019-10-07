using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    string displayDialog = "";
    Queue<string> dialog = new Queue<string>();
    int displayPosition = 0;
    float displaySpeed = .09f;
    float lastUpdateTime;
    bool finishedDisplay = false;
    float displayNextMessageTime = 5f;
    System.Random rand;

    AudioSource audioData;
    List<AudioClip> clips = new List<AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        lastUpdateTime = Time.time + displaySpeed;
        GenerateIntro();
        GetComponentInChildren<UnityEngine.Canvas>().enabled = false;

        audioData = GetComponentInChildren<AudioSource>();
        clips.Add(Resources.Load<AudioClip>("Music/Kid_Crying_1"));
        clips.Add(Resources.Load<AudioClip>("Music/Kid_Crying_2"));
        clips.Add(Resources.Load<AudioClip>("Music/Kid_Crying_3"));
        clips.Add(Resources.Load<AudioClip>("Music/Kid_Crying_4"));
        rand = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastUpdateTime + displaySpeed <= Time.time)
        {
            if (displayPosition < displayDialog.Length && displayDialog.Length > 0)
            {
                displayPosition += 1;
                lastUpdateTime = Time.time;
                if (!audioData.isPlaying)
                {
                    audioData.PlayOneShot(clips[rand.Next(4)]);
                }
            } else
            {
                finishedDisplay = true;
            }

            GetComponentInChildren<UnityEngine.UI.Text>().text = displayDialog.Substring(0, displayPosition);
        }

        if (finishedDisplay && (lastUpdateTime + displayNextMessageTime <= Time.time))
        {
            audioData.Stop();
            //If there is another part of message print it
            if (dialog.Count > 0)
            {
                displayDialog = dialog.Dequeue();
                displayPosition = 0;

                GetComponentInChildren<UnityEngine.Canvas>().enabled = true;
            }
            else  //Else hide the canvas
            {
                GetComponentInChildren<UnityEngine.Canvas>().enabled = false;
            }
        }
    }

    public void GenerateClue(int amountIncorrect, string randomDifference)
    {
        displayDialog = "";
        dialog.Clear();
        if (amountIncorrect == 1)
        {
            dialog.Enqueue("This is a really nice looking item.");
            dialog.Enqueue("It is almost perfect.");
            dialog.Enqueue("The " + randomDifference + " could be better.");
        } 
        else if (amountIncorrect == 2)
        {
            dialog.Enqueue("This item is OK.");
            dialog.Enqueue("It isn't great though.");
            dialog.Enqueue("The " + randomDifference + " makes me feel weird.");
        }
        else
        {
            dialog.Enqueue("This is a terrible item");
            dialog.Enqueue("Absolutely awful, do better.");
            dialog.Enqueue("The " + randomDifference + " makes me sick.");
        }
    }

    public void WinDialog()
    {
        dialog.Enqueue("Wow what an amazing item!");
        dialog.Enqueue("This is exactly what I wanted, thanks bro!");
        dialog.Enqueue("**FLUSH**");
    }

    private void GenerateIntro()
    {
        dialog.Enqueue("Hey Bro!");
        dialog.Enqueue("Can you bring me that thing?");
        dialog.Enqueue("What thing?");
        dialog.Enqueue("Stop kiddin around! Go get it for me!");
    }
}
