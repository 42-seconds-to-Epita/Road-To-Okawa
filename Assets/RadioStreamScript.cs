/*using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using Un4seen.Bass;

public class RadioStreamScript : MonoBehaviour { 
    public string url = "http://str0.creacast.com/radio_vinci_autoroutes_2";
    private int stream;

    void Awake()
    {
        Debug.Log("1");
        BassNet.Registration("XX", "XX");
        Debug.Log("2");
    }

    // Use this for initialization
    void Start()
    {
        Debug.Log("3");
        Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_PLAYLIST, 0);

        Debug.Log("4");
        Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);

        stream = Bass.BASS_StreamCreateURL(url, 0, BASSFlag.BASS_DEFAULT, null, IntPtr.Zero);

        PlayStream(url);
    }

    public void PlayStream(string url)
    {
        if (stream != 0)
        {
            Bass.BASS_ChannelPlay(stream, false);
        }
        else
        {
            Debug.Log("BASS Error Code = " + Bass.BASS_ErrorGetCode());
        }
    }

    public void StopStream()
    {
        Bass.BASS_ChannelStop(stream);
    }

    // Get the Channel Information
    public string GetChannelInfo()
    {
        BASS_CHANNELINFO info = new BASS_CHANNELINFO();
        Bass.BASS_ChannelGetInfo(stream, info);
        return info.ToString();
    }

    public void SetVolume(float value)
    {
        Bass.BASS_SetVolume(value);
    }

    void OnApplicationQuit()
    {
        // free the stream
        Bass.BASS_StreamFree(stream);
        // free BASS
        Bass.BASS_Free();
    }

}*/
