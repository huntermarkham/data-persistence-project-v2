using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class MenuManager : MonoBehaviour
{
    //Super helpful stuff to allow access to this class from any script

    //Note the keyword static after the keyword public.
    //This keyword means that the values stored in this class member will be shared by all the instances of that class.
    public static MenuManager Instance;

    //passes name variable
    public TMP_InputField nameInput;

    public string playerName;


    private void Awake()
    {
        //code to prevent duplicate MenuManagers from being created
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        //this = You don’t need to have a reference to it, like you do when you assign GameObjects to script properties in the Inspector.
        Instance = this;
        //marks the MainManager GameObject attached to this script not to be destroyed when the scene changes.
        DontDestroyOnLoad(gameObject);

        LoadName();
    }

    //save function that saves serializable data
    [System.Serializable]
    class SaveData
    {
        public string playerName;
    }

    public void SetName()
    {
        playerName = nameInput.text;
    }

    //method to save color
    public void SaveName()
    {
        //created a new instance of the save data
        SaveData data = new SaveData();
        //filled team color class member with the teamColor variable saved in the MainManager
        data.playerName = playerName;

        //transformed that instance to JSON with JsonUtility.ToJson
        string json = JsonUtility.ToJson(data);

        //used the special method File.WriteAllText to write a string to a file
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }


    public void LoadName()
    {
        //This method is a reversal of the SaveColor method
        string path = Application.persistentDataPath + "/savefile.json";

        //uses the method File.Exists to check if a .json file exists
        if (File.Exists(path))
        {
            //if the file does exist, then the method will read its content with File.ReadAllText
            string json = File.ReadAllText(path);
            //give the resulting text to JsonUtility.FromJson to transform it back into a SaveData instance: 
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            //set the TeamColor to the color saved in that SaveData
            playerName = data.playerName;
        }
    }
}
