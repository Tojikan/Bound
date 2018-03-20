using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Playlist", menuName = "Data Objects/Music Playlist")]
public class MusicList : ScriptableObject
{
    public string playlistName = "New Playlist";
    public AudioClip[] musicPlaylist;
}
