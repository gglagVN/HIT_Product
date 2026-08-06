using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if GAMEFEEL_UI
using UnityEngine.UI;
#endif

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// A test object to store data to test the SaveLoadManager class
	/// </summary>
	[System.Serializable]
	public class SaveLoadTestObject
	{
		public string SavedText;
	}

	/// <summary>
	/// A simple class used in the SaveLoadTestScene to test the SaveLoadManager class
	/// </summary>
	public class SaveLoadTester : MonoBehaviour
	{
		[Header("Bindings")]
		#if GAMEFEEL_UI
		/// the text to save
		[Tooltip("the text to save")]
		public InputField TargetInputField;
		#endif

		[Header("Save settings")]
		/// the chosen save method (json, encrypted json, binary, encrypted binary)
		[Tooltip("the chosen save method (json, encrypted json, binary, encrypted binary)")]
		public SaveLoadManagerMethods SaveLoadMethod = SaveLoadManagerMethods.Binary;
		/// the name of the file to save
		[Tooltip("the name of the file to save")]
		public string FileName = "TestObject";
		/// the name of the destination folder
		[Tooltip("the name of the destination folder")]
		public string FolderName = "Test/";
		/// the extension to use
		[Tooltip("the extension to use")]
		public string SaveFileExtension = ".testObject";
		/// the key to use to encrypt the file (if needed)
		[Tooltip("the key to use to encrypt the file (if needed)")]
		public string EncryptionKey = "ThisIsTheKey";

		/// Test button
		[InspectorButton("Save")]
		public bool TestSaveButton;
		/// Test button
		[InspectorButton("Load")]
		public bool TestLoadButton;
		/// Test button
		[InspectorButton("Reset")]
		public bool TestResetButton;

		protected IMMSaveLoadManagerMethod _saveLoadManagerMethod;

		/// <summary>
		/// Saves the contents of the TestObject into a file
		/// </summary>
		public virtual void Save()
		{
			InitializeSaveLoadMethod();
			SaveLoadTestObject testObject = new SaveLoadTestObject();
			#if GAMEFEEL_UI
			testObject.SavedText = TargetInputField.text;
			#endif
			SaveLoadManager.Save(testObject, FileName+SaveFileExtension, FolderName);
		}

		/// <summary>
		/// Loads the saved data
		/// </summary>
		public virtual void Load()
		{
			InitializeSaveLoadMethod();
			SaveLoadTestObject testObject = (SaveLoadTestObject)SaveLoadManager.Load(typeof(SaveLoadTestObject), FileName + SaveFileExtension, FolderName);
			#if GAMEFEEL_UI
			TargetInputField.text = testObject.SavedText;
			#endif
		}

		/// <summary>
		/// Resets all saves by deleting the whole folder
		/// </summary>
		protected virtual void Reset()
		{
			SaveLoadManager.DeleteSaveFolder(FolderName);
		}

		/// <summary>
		/// Creates a new SaveLoadManagerMethod and passes it to the SaveLoadManager
		/// </summary>
		protected virtual void InitializeSaveLoadMethod()
		{
			switch(SaveLoadMethod)
			{
				case SaveLoadManagerMethods.Binary:
					_saveLoadManagerMethod = new SaveLoadManagerMethodBinary();
					break;
				case SaveLoadManagerMethods.BinaryEncrypted:
					_saveLoadManagerMethod = new SaveLoadManagerMethodBinaryEncrypted();
					(_saveLoadManagerMethod as SaveLoadManagerEncrypter).Key = EncryptionKey;
					break;
				case SaveLoadManagerMethods.Json:
					_saveLoadManagerMethod = new SaveLoadManagerMethodJson();
					break;
				case SaveLoadManagerMethods.JsonEncrypted:
					_saveLoadManagerMethod = new SaveLoadManagerMethodJsonEncrypted();
					(_saveLoadManagerMethod as SaveLoadManagerEncrypter).Key = EncryptionKey;
					break;
			}
			SaveLoadManager.SaveLoadMethod = _saveLoadManagerMethod;
		}
	}
}