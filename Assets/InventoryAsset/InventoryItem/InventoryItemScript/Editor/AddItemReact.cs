using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
namespace InventorySystem
{
    //Author: Jaxon Schauer
    /// <summary>
    /// Creates the dropdown menu on the AddItem.
    /// </summary>
    [CustomEditor(typeof(AddItem))]
    public class AddItemReact : Editor
    {
        public override void OnInspectorGUI()
        {

            // Draw the default inspector
            DrawDefaultInspector();
            InventoryDropDown();
            ItemDropDown();


        }
        private void InventoryDropDown()
        {
            AddItem script = (AddItem)target;

            script.GetInventoryList();

            if (script.inventories != null && script.inventories.Count > 0)
            {
                string[] inventoryNames = new string[script.inventories.Count];
                for (int i = 0; i < script.inventories.Count; i++)
                {
                    inventoryNames[i] = script.inventories[i].GetInventoryName();
                }
                script.SelectedInventoryIndex = EditorGUILayout.Popup("Select Inventory", script.SelectedInventoryIndex, inventoryNames);
                script.SetInventory(script.inventories[script.SelectedInventoryIndex]);
                EditorUtility.SetDirty(script);
            }
            else
            {
                EditorGUILayout.LabelField("No Inventories found.");
            }
        }
        private void ItemDropDown()
        {
            AddItem script = (AddItem)target;
            script.GetItemList();

            // If there are items in the list, show the dropdown
            if (script.items != null && script.items.Count > 0)
            {
                string[] itemNames = new string[script.items.Count];
                for (int i = 0; i < script.items.Count; i++)
                {
                    itemNames[i] = script.items[i].GetItemType();
                }

                script.selectedItemIndex = EditorGUILayout.Popup("Select Item", script.selectedItemIndex, itemNames);

                script.SetItem(script.items[script.selectedItemIndex]);
                EditorUtility.SetDirty(script);


            }
            else
            {
                EditorGUILayout.LabelField("No items found.");
            }
        }
    }
}
