using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SaveSystemTutorial
{ public class DataManager : Singleton<DataManager>
    {
        [SerializeField]
        int blood = 0;
        [SerializeField]
        Transform playerTransform;

        public Item[] items;
        [SerializeField]
        Inventory bag;
        [SerializeField]
        Inventory ability;
        [SerializeField]
        Inventory equipment;

        Vector3 playerPosition;
        
        [System.Serializable]
        class SaveData
        {
            public int Blood;
            public Vector3 playerPosition;
        }

        #region Properties

        public int Blood => blood;


        #endregion

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        SaveData SavingData()
        {
            var saveData = new SaveData();

            saveData.Blood = blood;
            saveData.playerPosition = playerTransform.position;

            return saveData;
        }

        void LoadData(SaveData saveData)
        {
            blood = saveData.Blood;
            playerTransform.position = saveData.playerPosition;
        }
        
        #region JSON

        public void SaveByJson(string saveName)
        {
            SaveSystem.SaveByJson(saveName,SavingData());
            SaveGame("bag.dt",SaveInventoryData(new InventoryData(),bag));
            SaveGame("ability.dt", SaveInventoryData(new InventoryData(), ability));
            SaveGame("equipment.dt", SaveInventoryData(new InventoryData(), equipment));

        }
        public void LoadFromJson(string saveName)
        {
            
            string[] file = Directory.GetFiles(Application.persistentDataPath, "*", SearchOption.AllDirectories);
            foreach (var fileName in file)
            {
                if (fileName == Application.persistentDataPath + "\\"  + saveName)
                {
                    var saveData = SaveSystem.LoadFromJson<SaveData>(saveName);
                    LoadData(saveData);
                    var bagData = LoadGame<InventoryData>("bag.dt");
                    LoadInventoryData(bag, bagData);
                    var abilityData = LoadGame<InventoryData>("ability.dt");
                    LoadInventoryData(ability, abilityData);
                    var equipmentData = LoadGame<InventoryData>("equipment.dt");
                    LoadInventoryData(equipment, equipmentData);
                }
            }
        }
        public void DeleteData(string saveName)
        {
            string[] file = Directory.GetFiles(Application.persistentDataPath, "*", SearchOption.AllDirectories);
            foreach (var fileName in file)
            {
                if (fileName == Application.persistentDataPath + "\\" + saveName)
                {
                    SaveSystem.DeleteSaveFile(saveName);
                }
            }
            
        }

        #endregion



         class InventoryData
        {
            public int count;
            public string[] type;
            public int[] amount;
        }

        InventoryData SaveInventoryData(InventoryData inventoryData,Inventory inventory)
        {
            inventoryData.count = inventory.items.Count;
            inventoryData.type = new string[inventory.items.Count];
            inventoryData.amount = new int[inventory.items.Count];
            Debug.Log(inventory.items.Count);
            for (int i = 0;i < inventory.items.Count; i++)
            {
                if (inventory.items[i].itemData != null && inventory.items[i].amount != 0)
                {
                    inventoryData.type[i] = inventory.items[i].itemData.data.itemName;
                    inventoryData.amount[i] = inventory.items[i].amount;
                }
                else
                {
                    inventoryData.type[i] = "0";
                    inventoryData.amount[i] = 0;
                }
            }

            return inventoryData;
        }

        void LoadInventoryData(Inventory inventory,InventoryData inventoryData)
        {
            for (int i = 0; i < inventoryData.count; i++)
            {
                if (inventoryData.type[i] != "0" && inventoryData.amount[i] != 0)
                {
                    inventory.items[i].amount = inventoryData.amount[i] ;
                    foreach (var ite in items)
                    {
                        if (inventoryData.type[i] == ite.data.itemName)
                        {
                            inventory.items[i].itemData = ite;
                        }
                    }
                }
                else
                {
                    inventory.items[i].itemData = null;
                    inventory.items[i].amount = 0;
                }
            }
        }

        public void SaveGame(string saveName,object data)
        {
            
            if (!Directory.Exists(Application.persistentDataPath + "/game_SaveData"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/game_SaveData");
            }

            BinaryFormatter formatter = new BinaryFormatter();

            FileStream file = File.Create(Application.persistentDataPath + "/game_SaveData/" + saveName);

            var json = JsonUtility.ToJson(data,true);

            formatter.Serialize(file, json);

            file.Close();
        }

        public T LoadGame<T>(string saveName)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            if (File.Exists(Application.persistentDataPath + "/game_SaveData/" + saveName))
            {
                FileStream file = File.Open(Application.persistentDataPath + "/game_SaveData/" + saveName, FileMode.Open);

                var data = formatter.Deserialize(file);

                var json = JsonUtility.FromJson<T>((string)data);

                file.Close();

                return json;
            }
            return default;
        }
    }
}

