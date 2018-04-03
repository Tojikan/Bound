using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Playlist of BGMs that can be played or set for levels
//Array to crossreference index so music can be saved/loaded as a single int in a file
[CreateAssetMenu(fileName = "Playlist", menuName = "Data Objects/Music Playlist")]
public class MusicList : ScriptableObject
{
    public string playlistName = "New Playlist";
    public AudioClip[] musicPlaylist;
}
