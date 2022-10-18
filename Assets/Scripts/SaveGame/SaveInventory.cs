using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class SaveInventory : MonoBehaviour
{
    public Inventory bag;
    public Inventory saveBag;
    public Inventory ability;
    public Inventory saveAbility;
    public Inventory equipment;
    public Inventory saveEquipment;

    public void SaveAllInventory()
    {
        saveBag = bag;
        saveAbility = ability;
        saveEquipment = equipment;
    }

    public void LoadAllInventory()
    {
        bag = saveBag;
        ability = saveAbility;
        equipment = saveEquipment;
    }
    public void SaveGame()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/game_SaveData"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_SaveData");
        }

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream file = File.Create(Application.persistentDataPath + "/game_SaveData/inventory.txt");

        var json = JsonUtility.ToJson(bag);

        formatter.Serialize(file, json);

        file.Close();
    }
}
