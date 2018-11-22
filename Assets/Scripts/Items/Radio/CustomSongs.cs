using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Environment = System.Environment;

public class CustomSongs : MonoBehaviour {

    string[] filePaths;
    string audioFolderPath;
    bool audioIsLoaded = false;
    int doneAudioFiles = 0;
    FileInfo[] wavFiles, oggFiles;
    public bool AudioIsLoaded
    {
        get
        {
            return audioIsLoaded;
        }
    }
    List<AudioClip> customAudioClips = new List<AudioClip>();
	// Use this for initialization
	void Start () {
        
        
    }

    private void Awake()
    {
        getPath();
        getFiles();
    }

    // Update is called once per frame
    void Update () {
		if(Input.GetKeyDown(KeyCode.N))
        {
            foreach(AudioClip clip in customAudioClips)
            {
                Debug.Log(clip.name);
            }
        }
	}

    void getPath()
    {
        audioFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/");
        audioFolderPath += "/My Games/Containment Breach VR/Custom Radio Tracks/";
        System.IO.Directory.CreateDirectory(audioFolderPath);
        Debug.Log(audioFolderPath);
    }

    void getFiles()
    {
        DirectoryInfo d = new DirectoryInfo(audioFolderPath);
        wavFiles = d.GetFiles("*.wav"); //Getting wav files
        oggFiles = d.GetFiles("*.ogg"); //Getting ogg files

        foreach (FileInfo file in wavFiles)
        {
            StartCoroutine(LoadAudio(file));
        }
        foreach (FileInfo file in oggFiles)
        {
            StartCoroutine(LoadAudio(file));
        }
        customAudioClips.Sort();
        foreach(AudioClip clip in customAudioClips)
        {
            Debug.Log("A Clip: " + clip.name);
        }
        
    }

    private WWW GetAudioFromFile(string path, string filename)
    {
        string audioToLoad = string.Format(path + "{0}", filename);
        WWW request = new WWW(audioToLoad);
        return request;
    }

    IEnumerator LoadAudio(FileInfo file)
    {
        Debug.Log(audioFolderPath + ", " + file.Name);
        WWW audioFile = GetAudioFromFile(audioFolderPath, file.Name);
        while(!audioFile.isDone)
        {
            yield return null;
        }
        doneAudioFiles++;
        AudioClip newClip = audioFile.GetAudioClip(true);
        newClip.name = file.Name;
        customAudioClips.Add(newClip);
        if (doneAudioFiles == wavFiles.Length + oggFiles.Length)
        {
            audioIsLoaded = true;
        }
    }

    public AudioClip[] getAudioClips()
    {
        return customAudioClips.ToArray();
    }
}
