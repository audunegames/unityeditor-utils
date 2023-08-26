using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Audune.Utils.Editor
{
  // Class that defines a builder for a reorderable list
  public class ReorderableListBuilder
  {
    // Delegate for drawing the header of a list
    public delegate void HeaderDrawer(ReorderableList list, Rect rect);

    // Delegate for drawing an element of a list
    public delegate void ElementDrawer(ReorderableList list, Rect rect, SerializedProperty element, int index);

    // Delegate for returning the height of an element of a list
    public delegate float ElementDrawerHeight(ReorderableList list, SerializedProperty element, int index);

    // Delegate for selecting the a label based on a serialized property
    public delegate GUIContent LabelSelector(SerializedProperty property);

    // Delegate for selecting the a label of an element based on a serialized property
    public delegate GUIContent ElementLabelSelector(SerializedProperty element, int index);

    // Delegate for an element callback
    public delegate void ElementCallback(SerializedProperty element, int index);

    // Delegate for an element reorder callback
    public delegate void ElementReorderedCallback(SerializedProperty element, int oldIndex, int newIndex);


    // Toggle to draw the header of the list
    protected bool _drawHeader;

    // Function to draw the header of the list
    protected HeaderDrawer _headerDrawer;

    // Function to draw an element of the list
    protected ElementDrawer _elementDrawer;

    // Function to return the height of an element of the list
    protected ElementDrawerHeight _elementDrawerHeight;


    // Event that is invoked after adding an element
    protected event ElementCallback _onAfterAdded;

    // Event that is invoked before removing an element
    protected event ElementCallback _onBeforeRemoved;

    // Event that is invoked when reordering an element
    protected event ElementReorderedCallback _onReordered;


    // Create a reorderable list from the builder options
    public virtual ReorderableList Create(SerializedObject serializedObject, SerializedProperty elements)
    {
      var list = new ReorderableList(serializedObject, elements);

      if (!_drawHeader)
        list.headerHeight = 0.0f;

      list.onAddCallback = CreateAddCallback();
      list.onRemoveCallback = CreateRemoveCallback();
      list.onReorderCallbackWithDetails = CreateReorderCallback();
      list.drawHeaderCallback = CreateHeaderCallback(list);
      list.drawElementCallback = CreateElementCallback(list);
      list.elementHeightCallback = CreateElementHeightCallback(list);

      return list;
    }


    #region Header drawer building methods
    // Unset the header drawer
    public ReorderableListBuilder UnsetHeaderDrawer()
    {
      _drawHeader = false;
      _headerDrawer = null;
      return this;
    }

    // Set the header drawer
    public ReorderableListBuilder SetHeaderDrawer(HeaderDrawer headerDrawer)
    {
      _drawHeader = true;
      _headerDrawer = headerDrawer;
      return this;
    }

    // Set the header drawer to select the label from the serialized property of the list
    public ReorderableListBuilder SetHeaderDrawer(LabelSelector headerLabelSelector)
    {
      return SetHeaderDrawer((list, rect) => EditorGUI.LabelField(rect, headerLabelSelector(list.serializedProperty)));
    }

    // Set the header drawer to use the specified label
    public ReorderableListBuilder SetHeaderDrawer(GUIContent headerLabel)
    {
      return SetHeaderDrawer((list, rect) => EditorGUI.LabelField(rect, headerLabel));
    }

    // Set the header drawer to use the label from the serialized property of the list
    public ReorderableListBuilder UsePropertyFieldForHeaderDrawer()
    {
      return SetHeaderDrawer((property) => new GUIContent(property.displayName, property.tooltip));
    }
    #endregion

    #region Element drawer building methods
    // Set the element drawer
    public ReorderableListBuilder SetElementDrawer(ElementDrawer elementDrawer, ElementDrawerHeight elementDrawerHeight)
    {
      _elementDrawer = elementDrawer;
      _elementDrawerHeight = elementDrawerHeight;
      return this;
    }

    // Set the element drawer which draws a header above the element
    public ReorderableListBuilder SetElementDrawerWithHeader(ElementDrawer elementDrawer, ElementDrawerHeight elementDrawerHeight, ElementLabelSelector elementHeaderLabelSelector)
    {
      return SetElementDrawer(WrapElementHeader(elementDrawer, elementHeaderLabelSelector), WrapElementHeaderHeight(elementDrawerHeight));
    }

    // Set the element GUI to use the property field of the element with the specified label selector
    public ReorderableListBuilder UsePropertyFieldForElementDrawer(ElementLabelSelector elementLabelSelector)
    {
      return SetElementDrawer(ElementPropertyField(elementLabelSelector), ElementPropertyFieldHeight(elementLabelSelector));
    }

    // Set the element GUI to use the property field of the element with the specified label
    public ReorderableListBuilder UsePropertyFieldForElementDrawer(GUIContent elementLabel)
    {
      return UsePropertyFieldForElementDrawer((element, index) => elementLabel);
    }

    // Set the element GUI to use the property field of the element which draws a header above the element
    public ReorderableListBuilder UsePropertyFieldForElementDrawerWithHeader(ElementLabelSelector elementHeaderLabelSelector)
    {
      return SetElementDrawer(WrapElementHeader(ElementPropertyField(GUIContent.none), elementHeaderLabelSelector), WrapElementHeaderHeight(ElementPropertyFieldHeight(GUIContent.none)));
    }


    // Draw the property field of an element
    protected ElementDrawer ElementPropertyField(ElementLabelSelector labelSelector)
    {
      return (list, rect, element, index) => {
        EditorGUI.PropertyField(rect, element, labelSelector(element, index));
      };
    }

    // Draw the property field of an element with the specified label
    protected ElementDrawer ElementPropertyField(GUIContent label)
    {
      return ElementPropertyField((element, index) => label);
    }

    // Return the height of the property field of an element
    protected ElementDrawerHeight ElementPropertyFieldHeight(ElementLabelSelector labelSelector)
    {
      return (list, element, index) => {
        return EditorGUI.GetPropertyHeight(element, labelSelector(element, index));
      };
    }

    // Return the height of the property field of an element
    protected ElementDrawerHeight ElementPropertyFieldHeight(GUIContent label)
    {
      return ElementPropertyFieldHeight((element, index) => label);
    }

    // Wrap an element header to an element drawer function
    protected ElementDrawer WrapElementHeader(ElementDrawer elementDrawer, ElementLabelSelector labelSelector)
    {
      return (list, rect, element, index) => {
        var label = labelSelector != null ? labelSelector(element, index) : new GUIContent(ObjectNames.NicifyVariableName(element.type));
        EditorGUI.LabelField(rect.AlignTop(EditorGUIUtility.singleLineHeight, EditorGUIUtility.standardVerticalSpacing, out rect), label, EditorStyles.boldLabel);

        EditorGUI.indentLevel++;
        elementDrawer(list, rect, element, index);
        EditorGUI.indentLevel--;
      };
    }

    // Wrap an element header to an element drawer height function
    protected ElementDrawerHeight WrapElementHeaderHeight(ElementDrawerHeight elementDrawerHeight)
    {
      return (list, element, index) => {
        return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + elementDrawerHeight(list, element, index);
      };
    }
    #endregion

    #region Callback building methods
    // Add an after added callback
    public ReorderableListBuilder AddAfterAddedCallback(ElementCallback onAfterAdded)
    {
      _onAfterAdded += onAfterAdded;
      return this;
    }

    // Add a before removed callback
    public ReorderableListBuilder AddBeforeRemovedCallback(ElementCallback onBeforeRemoved)
    {
      _onBeforeRemoved += onBeforeRemoved;
      return this;
    }

    // Add a reordered callback
    public ReorderableListBuilder AddReorderedCallback(ElementReorderedCallback onReordered)
    {
      _onReordered += onReordered;
      return this;
    }
    #endregion

    #region Callback methods for modifying elements
    // Create a callback that handles adding an element from a list
    protected ReorderableList.AddCallbackDelegate CreateAddCallback(ElementCallback onAfterAddedOnce = null)
    {
      return (list) => {
        var index = list.serializedProperty.arraySize;

        // Add the element
        list.serializedProperty.InsertArrayElementAtIndex(index);

        // Invoke the callback on the element
        onAfterAddedOnce?.Invoke(list.serializedProperty.GetArrayElementAtIndex(index), index);
        _onAfterAdded?.Invoke(list.serializedProperty.GetArrayElementAtIndex(index), index);

        // Apply the changes
        list.serializedProperty.serializedObject.ApplyModifiedProperties();
      };
    }

    // Create a callback that handles removing an element from a list
    protected ReorderableList.RemoveCallbackDelegate CreateRemoveCallback(ElementCallback onBeforeRemovedOnce = null)
    {
      return (list) => {
        // Invoke the callback on all elements to remove
        foreach (var index in list.selectedIndices)
        {
          onBeforeRemovedOnce?.Invoke(list.serializedProperty.GetArrayElementAtIndex(index), index);
          _onBeforeRemoved?.Invoke(list.serializedProperty.GetArrayElementAtIndex(index), index);
        }

        // Remove all elements to remove
        foreach (var index in list.selectedIndices)
          list.serializedProperty.MoveArrayElement(index, list.serializedProperty.arraySize - 1);
        list.serializedProperty.arraySize -= list.selectedIndices.Count;

        // Apply the changes
        list.serializedProperty.serializedObject.ApplyModifiedProperties();
      };
    }

    // Create a callback that handles reordering an element in a list
    protected ReorderableList.ReorderCallbackDelegateWithDetails CreateReorderCallback(ElementReorderedCallback onReorderedOnce = null)
    {
      return (list, oldIndex, newIndex) => {
        // Move the element
        list.serializedProperty.MoveArrayElement(oldIndex, newIndex);

        // Invoke the callback on the element
        _onReordered?.Invoke(list.serializedProperty.GetArrayElementAtIndex(newIndex), oldIndex, newIndex);
        onReorderedOnce?.Invoke(list.serializedProperty.GetArrayElementAtIndex(newIndex), oldIndex, newIndex);

        // Apply the changes
        list.serializedProperty.serializedObject.ApplyModifiedProperties();
      };
    }
    #endregion

    #region Callback methods for drawing elements
    // Return a callback that handles drawing the header of the list
    protected ReorderableList.HeaderCallbackDelegate CreateHeaderCallback(ReorderableList list)
    {
      return (rect) => {
        if (_drawHeader && _headerDrawer != null)
          _headerDrawer(list, rect);
      };
    }

    // Return a callback that handles drawing an element of the list
    protected ReorderableList.ElementCallbackDelegate CreateElementCallback(ReorderableList list)
    {
      return (rect, index, isActive, isFocused) => _elementDrawer?.Invoke(list, rect.ContractVertical(EditorGUIUtility.standardVerticalSpacing * 0.5f), list.serializedProperty.GetArrayElementAtIndex(index), index);
    }

    // Return a callback that handles returning the heght of an element of the list
    protected ReorderableList.ElementHeightCallbackDelegate CreateElementHeightCallback(ReorderableList list)
    {
      return (index) => _elementDrawerHeight(list, list.serializedProperty.GetArrayElementAtIndex(index), index);
    }
    #endregion
  }
}
