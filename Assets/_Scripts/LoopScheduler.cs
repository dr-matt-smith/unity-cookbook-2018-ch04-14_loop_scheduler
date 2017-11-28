using UnityEngine;

// source: adapted from sample code on the Unity script reference pages
// https://docs.unity3d.com/ScriptReference/AudioSource.PlayScheduled.html

[RequireComponent(typeof(AudioSource))]
public class LoopScheduler : MonoBehaviour 
{
    public float bpm = 140.0F;
    public int numBeatsPerSegment = 16;
    public AudioClip[] clips = new AudioClip[2];

    public float timeBetweenClipsStarts = 2.0F;

    private double nextEventTime;
    private AudioSource[] audioSources;
    private bool running = false;


    private int nextLoopIndex = 0;
    private int numLoops;

    void Start()
    {
        numLoops = clips.Length;
        audioSources = new AudioSource[numLoops];
        int i = 0;
        while (i < numLoops)
        {
            GameObject child = new GameObject("Player");
            child.transform.parent = gameObject.transform;
            audioSources[i] = child.AddComponent<AudioSource>();
            audioSources[i].clip = clips[i];
            i++;
        }
        nextEventTime = AudioSettings.dspTime + timeBetweenClipsStarts;
        running = true;
    }
    void Update()
    {
        if (!running)
            return;

        double time = AudioSettings.dspTime;
        if (time + 1.0F > nextEventTime)
        {
            StartNextLoop();
        }

        PrintLoopPlayingStatus();
}

    private void StartNextLoop()
    {
        audioSources[nextLoopIndex].PlayScheduled(nextEventTime);
        Debug.Log("Scheduled source " + nextLoopIndex + " to start at time " + nextEventTime);
        nextEventTime += 60.0F / bpm * numBeatsPerSegment;

        nextLoopIndex++;
        if(nextLoopIndex >= numLoops){
            nextLoopIndex = 0;
        }
    }

    private void PrintLoopPlayingStatus()
    {
        string statusMessage = "CLips playing:: ";
        int i = 0;
        while (i < numLoops)
        {
            statusMessage += audioSources[i].isPlaying + " ";
            i++;
        }

        print(statusMessage);
    }
}