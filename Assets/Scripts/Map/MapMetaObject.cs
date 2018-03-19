using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoundMaps {

	[Serializable]
	public class MapMetaObject {

		public string name;
		public string fileLocation;
        public string description;
        public string imagePath;

		// Use this for initialization
		public MapMetaObject (string name, string fileLocation, string description, string imagePath ) {
			this.name = name;
			this.fileLocation = fileLocation;
            this.description = description;
            this.imagePath = imagePath;
		}
	}
}

