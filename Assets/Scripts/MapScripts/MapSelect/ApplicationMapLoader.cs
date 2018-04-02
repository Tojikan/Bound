using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundMaps;

//Reads from map directory
public class ApplicationMapLoader : MonoBehaviour {

    //Returns a list of all map metas
	public LinkedList<MapMetaObject> GetMapMeta (string directory) {
		var directories = Directory.GetDirectories (directory);

		LinkedList<MapMetaObject> maps = new LinkedList<MapMetaObject>();
		foreach (string dir in directories) {
			string metaLoc = dir + "/meta.json";
			try
			{
					string metaString = ReadString(metaLoc);
					MapMetaObject mapMeta = JsonUtility.FromJson<MapMetaObject>(metaString);
					maps.AddLast(mapMeta);
			}
			catch (System.Exception e)
			{
					Debug.Log(e);
					Debug.Log("Invalid map");
			}
		}
		return maps;
	}

	public string ReadString(string file)
	{
			//Open up a stream to the file
			StreamReader reader = new StreamReader(file);

			//Reads it into a string
			string mapInfo = reader.ReadToEnd();

			//Remember to close the stream
			reader.Close();

			return mapInfo;
	}
}
