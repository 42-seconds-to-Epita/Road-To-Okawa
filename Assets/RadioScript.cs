using System.Collections;
using NAudio.Wave;
using UnityEngine;
using UnityEngine.Networking;

public class RadioScript : MonoBehaviour
{
    private const string radioURL = "http://str0.creacast.com/radio_vinci_autoroutes_2";

    /// <summary>
    /// The MediaFoundationReader for audio processing.
    /// </summary>
    private MediaFoundationReader mediaFoundationReader;

    /// <summary>
    /// The WaveOutEvent for audio output.
    /// </summary>
    private WaveOutEvent waveOut;

    IEnumerator GetAudioClip()
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(radioURL, AudioType.OGGVORBIS))
        {
            (www.downloadHandler as DownloadHandlerAudioClip).streamAudio = true;
            var operation = www.SendWebRequest();
            while (www.downloadProgress < 0.5)
            {
                Debug.Log("progress: " + www.downloadProgress);
                yield return null;
            }
 
            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Audio clip loaded.");
                AudioClip clip = (www.downloadHandler as DownloadHandlerAudioClip).audioClip;
                AudioSource source = GetComponent<AudioSource>();
                source.clip = clip;
                source.Play();
            }
            yield return operation;
        }
    }

    void Start()
    {
        StartCoroutine(GetAudioClip());
        return;
        Debug.Log("0");
        StartCoroutine(PlayRadio());

        for (int n = -1; n < WaveOut.DeviceCount; n++)
        {
            var caps = WaveOut.GetCapabilities(n);
            Debug.Log($"{n}: {caps.ProductName}");
        }
    }

    private IEnumerator PlayRadio()
    {
        Debug.Log("T");
        yield return null;
        try
        {
            Debug.Log("Test");
            mediaFoundationReader = new MediaFoundationReader(radioURL);
            waveOut = new WaveOutEvent();
            waveOut.Init(mediaFoundationReader);
            waveOut.Play();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error playing radio: {ex.Message}");
        }
    }

    void OnDestroy()
    {
        if (waveOut != null)
        {
            waveOut.Stop();
            waveOut.Dispose();
        }

        if (mediaFoundationReader != null)
        {
            mediaFoundationReader.Dispose();
        }
    }
    
    public class StreamingAudioPlayer : MonoBehaviour
    {
        public AudioSource audioSrc;

        public string streamAudio;
        public AudioType audioStreamType = AudioType.UNKNOWN;
        [Min(1024)] public int bytesToDownloadBeforePlaying = 100000;

        private void Start()
        {
            //Example usage that can be called from other scripts
            PlayAudioAsync(streamAudio);
        }

        /// <summary>
        /// Play audio with the currently assigned values.
        /// </summary>
        public void PlayAudioAsync()
        {
            PlayAudioAsync(streamAudio);
        }

        /// <summary>
        /// Play audio from given URL, using component's current values.
        /// </summary>
        /// <param name="streamURL">The source URL of the audio you want to play</param>
        public void PlayAudioAsync(string streamURL)
        {
            PlayAudioAsync(streamURL, audioStreamType, (ulong)bytesToDownloadBeforePlaying);
        }


        /// <summary>
        /// Play audio from given URL, using custom AudioType and byte buffering.
        /// </summary>
        /// <param name="streamURL">The source URL of the audio you want to play</param>
        /// <param name="audioType"></param>
        /// <param name="bytesBeforePlayback">How many bytes to download first before starting playback. Useful to buffer longer for slow downloads.</param>
        public void PlayAudioAsync(string streamURL, AudioType audioType, ulong bytesBeforePlayback = 10000)
        {
            StartCoroutine(PlayAudioAsyncCoroutine(streamURL, audioType, bytesBeforePlayback));
        }

        private IEnumerator PlayAudioAsyncCoroutine(string streamURL, AudioType audioType, ulong bytesBeforePlayback = 10000)
        {
            if(string.IsNullOrWhiteSpace(streamURL)) { Debug.LogWarning("No audio stream URL provided. Playback skipped."); yield break; }

            using (var uwr = UnityWebRequestMultimedia.GetAudioClip(streamURL, audioType))
            {
                DownloadHandlerAudioClip dlHandler = (DownloadHandlerAudioClip)uwr.downloadHandler;

                //Allow AudioClip to be played before it's finished downloading
                dlHandler.streamAudio = true;
                //Begin downloading the clip
                var download = uwr.SendWebRequest();
                yield return null;
       
                //Wait for enough bytes to download
                while (uwr.downloadedBytes < bytesBeforePlayback)
                {
                    if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
                    {
                        Debug.LogWarning($"Error downloading audio stream from:{streamURL} : {uwr.error}");
                        yield break;
                    }
                    yield return null; 
                }

                AudioClip audioClip = dlHandler.audioClip;
                //It will be null if it wasn't able to format the audio
                if (audioClip == null)
                {
                    Debug.Log("Couldn't process audio stream.");
                    yield break;
                }

                //Assign the streaming clip and play it now that we're ready.
                audioSrc.clip = audioClip;
                audioSrc.Play();

                Debug.Log("Playing audio stream!");
                //Continue downloading the rest of the audio stream.
                yield return download;

                Debug.Log("Finished downloading audio stream!");
            }
        }
    }
}