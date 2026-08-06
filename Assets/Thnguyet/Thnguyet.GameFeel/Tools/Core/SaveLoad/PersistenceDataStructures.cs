using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// A serializable class used to store scene data, the key is a string (the scene name), the value is a PersistencySceneData
	/// </summary>
	[Serializable]
	public class DictionaryStringSceneData : SerializableDictionary<string, PersistenceSceneData>
	{
		public DictionaryStringSceneData() : base() { }
		protected DictionaryStringSceneData(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}

	/// <summary>
	/// A serializable class used to store object data, the key is a string (the object name), the value is a string (the object data)
	/// </summary>
	[Serializable]
	public class DictionaryStringString : SerializableDictionary<string, string>
	{
		public DictionaryStringString() : base() { }
		protected DictionaryStringString(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
	
	/// <summary>
	/// A serializable class used to store all the data for a persistence manager, a collection of scene datas
	/// </summary>
	[Serializable]
	public class PersistenceManagerData
	{
		public string PersistenceID;
		public string SaveDate;
		public DictionaryStringSceneData SceneDatas;
	}
	
	/// <summary>
	/// A serializable class used to store all the data for a scene, a collection of object datas
	/// </summary>
	[Serializable]
	public class PersistenceSceneData
	{
		public DictionaryStringString ObjectDatas;
	}
	
	/// <summary>
	/// The various types of persistence events that can be triggered by the PersistencyManager
	/// </summary>
	public enum PersistenceEventType { DataSavedToMemory, DataLoadedFromMemory, DataSavedFromMemoryToFile, DataLoadedFromFileToMemory }

	/// <summary>
	/// A data structure used to store persistence event data.
	/// To use :
	/// PersistencyEvent.Trigger(PersistencyEventType.DataLoadedFromFileToMemory, "yourPersistencyID");
	/// </summary>
	public struct PersistenceEvent
	{
		public PersistenceEventType PersistenceEventType;
		public string PersistenceID;

		public PersistenceEvent(PersistenceEventType eventType, string persistenceID)
		{
			PersistenceEventType = eventType;
			PersistenceID = persistenceID;
		}

		static PersistenceEvent e;
		public static void Trigger(PersistenceEventType eventType, string persistencyID)
		{
			e.PersistenceEventType = eventType;
			e.PersistenceID = persistencyID;
			FeelEventManager.TriggerEvent(e);
		}
	}
}
