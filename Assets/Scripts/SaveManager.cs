using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour
{
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        Load();
    }
    public void Save()
    {
        FileStream file = new(Application.persistentDataPath + "/PlayerData.dat", FileMode.OpenOrCreate);

        try
        {
            BinaryFormatter formatter = new();
            formatter.Serialize(file, _gameManager.playerData);
        }
        catch (SerializationException ex)
        {
            Debug.LogError("There was an issue serializing this data: " + ex);
        }
        finally
        {
            file.Close();
        }
    }

    public void Load()
    {
        FileStream file = new(Application.persistentDataPath + "/PlayerData.dat", FileMode.Open);
        
        try
        {
            BinaryFormatter formatter = new();
            _gameManager.playerData = (PlayerData)formatter.Deserialize(file);
        }
        catch (SerializationException ex)
        {
            Debug.LogError("There was an issue deserializing this data: " + ex);
        }
        finally
        {
            file.Close();
        }
    }
}
