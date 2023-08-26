using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using Unity.VisualScripting;

public static class InventorySaveSystem
{
    public static void SaveInventory(Dictionary<string, Inventory> inventoryManager)
    {
        BinaryFormatter formatter= new BinaryFormatter();
        string path = Application.persistentDataPath + "/Item";
        FileStream fileStream = new FileStream(path, FileMode.Create);
        InventoryData InventoryData = new InventoryData(inventoryManager);
        formatter.Serialize(fileStream, InventoryData);
        fileStream.Close();
    }
    public static InventoryData LoadItem()
    {
        string path = Application.persistentDataPath + "/Item";
        if(File.Exists(path))
        {
            FileStream fileStream = new FileStream(path, FileMode.Open);
            if (fileStream.Length == 0)
            {
                fileStream.Close();
                return null;
            }
            BinaryFormatter formatter = new BinaryFormatter();
            InventoryData InventoryData = formatter.Deserialize(fileStream) as InventoryData;
            fileStream.Close();
            return InventoryData;
        }
        else
        {
            Debug.LogError("Save File " + path + " does not exist");
            return null;
        }
    }
    public static void Create()
    {
        string path = Application.persistentDataPath + "/Item";
        if (!File.Exists(path))
        {
            FileStream fileStream = new FileStream(path, FileMode.Create);
            fileStream.Close();
        }
    }

    public static void Reset()
    {
        string path = Application.persistentDataPath + "/Item";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
