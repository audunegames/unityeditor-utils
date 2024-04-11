using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines a builder for a reorderable list with a dropdown for the add button
  public class ReorderableDropdownListBuilder<TItem> : ReorderableListBuilder
  {
    // Delegate for creating the menu
    public delegate void DropdownCreator(ReorderableList list, Rect buttonRect, Action<TItem> addCallback);

    // Delegate for a callback that is invoked when the menu should add an item
    public delegate void DropdownAddCallback(SerializedProperty element, int index, TItem item);


    // Function to create the dropdown
    protected DropdownCreator _dropdownCreator;

    // Callback that is invoked when the dropdown should add an item
    protected DropdownAddCallback _dropdownAddCallback;


    // Create a reorderable list from the builder options
    public override ReorderableList Create(SerializedObject serializedObject, SerializedProperty elements)
    {
      var list = base.Create(serializedObject, elements);

      list.onAddCallback = null;
      list.onAddDropdownCallback = CreateAddDropdownCallback(list);

      return list;
    }


    #region Dropdown building methods
    // Set the dropdown creator
    public ReorderableDropdownListBuilder<TItem> SetDropdownCreator(DropdownCreator dropdownCreator)
    {
      _dropdownCreator = dropdownCreator;
      return this;
    }

    // Set the dropdown add callback
    public ReorderableDropdownListBuilder<TItem> SetDropdownAddCallback(DropdownAddCallback dropdownAddCallback)
    {
      _dropdownAddCallback = dropdownAddCallback;
      return this;
    }
    #endregion

    #region Callback methods for modifying elements
    // Return a callback that handles adding an element to the list using a dropdown menu
    protected ReorderableList.AddDropdownCallbackDelegate CreateAddDropdownCallback(ReorderableList list)
    {
      return (buttonRect, _) => {
        _dropdownCreator(list, buttonRect, (item) => CreateAddCallback((element, index) => _dropdownAddCallback(element, index, item)).Invoke(list));
      };
    }
    #endregion
  }
}
