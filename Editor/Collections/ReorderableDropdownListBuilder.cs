using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Audune.Utils.Editor
{
  // Class that defines a builder for a reorderable list with a dropdown menu for the add button
  public class ReorderableDropdownListBuilder<TItem> : ReorderableListBuilder
  {
    // Delegate for creating the dropdown menu
    public delegate void DropdownCreator(ReorderableList list, Rect buttonRect, Action<TItem> addCallback);

    // Delegate for a callback that is invoked when the dropdown menu should add an item
    public delegate void DropdownAddCallback(SerializedProperty element, TItem item);


    // Function to create the dropdown menu
    protected DropdownCreator _dropdownCreator;

    // Callback that is invoked when the dropdown menu should add an item
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
        _dropdownCreator(list, buttonRect, (item) => CreateAddCallback((element, index) => _dropdownAddCallback(element, item)).Invoke(list));
      };
    }
    #endregion
  }


  // Class that defines utility functions for the reorderable dropdown list builder
  public static class ReorderableDropdownListBuilder
  {
    // Create a builder for the child types of the specified object type 
    public static ReorderableDropdownListBuilder<Type> CreateForObjectTypes<TType>(TypeDisplayOptions typeDisplayOptions, Func<TType, object> itemSelector = null)
    {
      itemSelector ??= obj => obj;

      return new ReorderableDropdownListBuilder<Type>()
        .SetDropdownCreator((list, buttonRect, addCallback) => typeof(TType).CreateGenericMenuForChildTypes(typeDisplayOptions, null, addCallback).DropDown(buttonRect))
        .SetDropdownAddCallback((element, type) => element.boxedValue = itemSelector((TType)Activator.CreateInstance(type)));
    }

    // Create a builder for the child types of the specified scriptable object type 
    public static ReorderableDropdownListBuilder<Type> CreateForScriptableObjectTypes<TType>(TypeDisplayOptions typeDisplayOptions, Func<TType, object> itemSelector = null) where TType : ScriptableObject
    {
      itemSelector ??= obj => obj;

      return new ReorderableDropdownListBuilder<Type>()
        .SetDropdownCreator((list, buttonRect, addCallback) => typeof(TType).CreateGenericMenuForChildTypes(typeDisplayOptions, null, addCallback).DropDown(buttonRect))
        .SetDropdownAddCallback((element, type) => element.boxedValue = itemSelector((TType)EditorScriptableObjectUtils.CreateInstanceWithNiceName(type)));
    }
  }
}
