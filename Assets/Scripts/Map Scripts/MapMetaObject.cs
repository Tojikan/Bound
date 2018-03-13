using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoundMaps {

	[Serializable]
	public class MapMetaObject {

		public string name;
		public string fileLocation;

		// Use this for initialization
		public MapMetaObject (string name, string fileLocation) {
			this.name = name;
			this.fileLocation = fileLocation;
		}
	}
}
