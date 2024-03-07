using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class RadioScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FMOD.System system;
        FMOD.Factory.System_Create(out system);

        system.init(32, FMOD.INITFLAGS.NORMAL, IntPtr.Zero);

        FMOD.Sound sound;
        system.createStream("http://str0.creacast.com/radio_vinci_autoroutes_2",
            FMOD.MODE.CREATESTREAM, out sound);

        FMOD.ChannelGroup channelGroup;
        system.createChannelGroup("main", out channelGroup);

        FMOD.Channel channel;
        system.playSound(sound, channelGroup, false, out channel);

        channelGroup.setVolume(0.5f);
    }
}