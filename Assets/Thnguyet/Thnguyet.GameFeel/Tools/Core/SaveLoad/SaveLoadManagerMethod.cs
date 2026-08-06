using UnityEngine;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// This component, on Awake or on demand, will force a SaveLoadMethod on the SaveLoadManager, changing the way it saves data to file.
	/// This will impact all classes that use the SaveLoadManager (unless they change that method before saving or loading).
	/// If you change the method, your previously existing data files won't be compatible, you'll need to delete them and start with new ones.
	/// </summary>
	public class SaveLoadManagerMethod : MonoBehaviour
	{
		[Header("Save and load method")]
		[Information("This component, on Awake or on demand, will force a SaveLoadMethod on the SaveLoadManager, changing the way it saves data to file. " +
		               "This will impact all classes that use the SaveLoadManager (unless they change that method before saving or loading)." +
		               "If you change the method, your previously existing data files won't be compatible, you'll need to delete them and start with new ones.", 
						InformationAttribute.InformationType.Info,false)]

		/// the method to use to save to file
		[Tooltip("the method to use to save to file")]
		public SaveLoadManagerMethods SaveLoadMethod = SaveLoadManagerMethods.Binary;
		/// the key to use to encrypt the file (if using an encryption method)
		[Tooltip("the key to use to encrypt the file (if using an encryption method)")]
		public string EncryptionKey = "ThisIsTheKey";

		protected IMMSaveLoadManagerMethod _saveLoadManagerMethod;

		/// <summary>
		/// On Awake, we set the SaveLoadManager's method to the chosen one
		/// </summary>
		protected virtual void Awake()
		{
			SetSaveLoadMethod();
		}
		
		/// <summary>
		/// Creates a new SaveLoadManagerMethod and passes it to the SaveLoadManager
		/// </summary>
		public virtual void SetSaveLoadMethod()
		{
			switch(SaveLoadMethod)
			{
				case SaveLoadManagerMethods.Binary:
					_saveLoadManagerMethod = new SaveLoadManagerMethodBinary();
					break;
				case SaveLoadManagerMethods.BinaryEncrypted:
					_saveLoadManagerMethod = new SaveLoadManagerMethodBinaryEncrypted();
					((SaveLoadManagerEncrypter)_saveLoadManagerMethod).Key = EncryptionKey;
					break;
				case SaveLoadManagerMethods.Json:
					_saveLoadManagerMethod = new SaveLoadManagerMethodJson();
					break;
				case SaveLoadManagerMethods.JsonEncrypted:
					_saveLoadManagerMethod = new SaveLoadManagerMethodJsonEncrypted();
					((SaveLoadManagerEncrypter)_saveLoadManagerMethod).Key = EncryptionKey;
					break;
			}
			SaveLoadManager.SaveLoadMethod = _saveLoadManagerMethod;
		}
	}    
}

