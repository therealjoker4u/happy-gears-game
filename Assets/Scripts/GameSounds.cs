using System.Collections.Generic;
using UnityEngine;

public class GameSounds : MonoBehaviour
{
    private static AudioSource audioSource;
    private static Dictionary<string, AudioClip> clips;
    private static bool clipsAreLoaded = false;
    void Awake()
    {
        if (!clipsAreLoaded)
        {
            clips = new Dictionary<string, AudioClip>();
            string[] clipsToLoad = {
                "lip",
                "star",
            };

            foreach (string clipName in clipsToLoad)
            {
                clips.Add(clipName, Resources.Load<AudioClip>($"Sounds/{clipName}"));

            }
        }

        if(audioSource == null)
        {
            GameObject sourceObj = new GameObject("Sounds Source");
            DontDestroyOnLoad(sourceObj);
            sourceObj.AddComponent<AudioSource>();
            audioSource = sourceObj.GetComponent<AudioSource>();
            audioSource.volume = 0.6f;
        }
    }

    public static void PlayOneShot(string clipName)
    {

        if (clips.ContainsKey(clipName))
        {
            audioSource.PlayOneShot(clips[clipName]);
        }
        else
        {
            Debug.LogWarning($"Audio clip '{clipName}' does not exist in clips dictionary!");
        }
    }
    
}
