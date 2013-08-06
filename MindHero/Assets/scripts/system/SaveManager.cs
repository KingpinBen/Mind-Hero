using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class SaveManager 
{
	public static SaveManager instance = new SaveManager();
	
	private readonly Dictionary<int, LevelSavedData> _saveData = new Dictionary<int, LevelSavedData>();

	private const string DIRECTORY = "./Saves/";
	private const string FILE_NAME = "Save01";
	
	public int this[int index]
	{
		get
		{
			LevelSavedData data;
			if (_saveData.TryGetValue(index, out data))
				return data.highScore;
			
			return 0;
		}
	}
	
	public void Load()
	{
		const string path = DIRECTORY + FILE_NAME;
		
		if (!Directory.Exists(DIRECTORY)) 
		{
			Directory.CreateDirectory(DIRECTORY);
		}
		
		if (File.Exists(path + ".sav")) 
		{
			var reader = new BinaryReader(File.Open(path + ".sav", FileMode.Open));
		    XmlHandler.locale = ( XmlHandler.Locale ) reader.ReadInt16();

			for(var i = 0; i < Application.levelCount; i++)
			{
				var data = new LevelSavedData
				               {
				                   levelId = i,
				                   highScore = reader.ReadInt32()
				               };

			    _saveData.Add(i, data);
			}
		}
		else
		{
			SetupDefaultSaveFile();
			Save ();
		}
	}

	public void Save() 
	{
		if (!Directory.Exists(DIRECTORY))
			Directory.CreateDirectory(DIRECTORY);
		
		const string path = DIRECTORY + FILE_NAME;
		var writer 	= new BinaryWriter(File.Create(path + ".tmp"));
		
		try
		{
            writer.Write((short)XmlHandler.locale);

			for(var i = 0; i < Application.levelCount; i++)
			{
				writer.Write(this[i]);	
			}
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
		
		writer.Flush();
		writer.Close();
		
		if (File.Exists(path + ".sav"))
			File.Delete(path + ".sav");
				
		File.Move(path + ".tmp", path + ".sav");
	}
	
	private void SetupDefaultSaveFile()
	{
		for(var i = 0; i < Application.levelCount; i++)
		{
			var data = new LevelSavedData
			               {
			                   levelId = i,
			                   highScore = 0
			               };

		    _saveData.Add(i, data);
		}
	}
}

public struct LevelSavedData
{
    public int levelId;
    public int highScore;

    public LevelSavedData( int id, int hs )
    {
        levelId = id;
        highScore = hs;
    }
}