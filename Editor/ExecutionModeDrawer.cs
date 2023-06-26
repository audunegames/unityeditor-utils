using System;
using UnityEditor;
using UnityEngine;

namespace Audune.Utils.Editor
{
  // Class that defines a property drawer for execution modes
  [CustomPropertyDrawer(typeof(ExecutionMode))]
  public sealed class ExecutionModeDrawer : PropertyDrawer
  {
    // Draw the property
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
      rect = EditorGUI.PrefixLabel(rect, label);
      if (EditorGUI.DropdownButton(rect, new GUIContent(((ExecutionMode)property.enumValueFlag).ToString()), FocusType.Keyboard))
        CreateEnumFlagMenu(property).DropDown(rect);
    }

    // Return the property height
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      return EditorGUIUtility.singleLineHeight;
    }


    // Create an enum flag menu for a property
    private GenericMenu CreateEnumFlagMenu(SerializedProperty property)
    {
      var menu = new GenericMenu();
      foreach (var flag in (ExecutionMode[])Enum.GetValues(typeof(ExecutionMode)))
        menu.AddItem(new GUIContent(ObjectNames.NicifyVariableName(Enum.GetName(typeof(ExecutionMode), flag))), HasEnumFlag(property, (int)flag), () => SetEnumFlag(property, (int)flag));
      return menu;
    }

    // Return if a property has an enum flag
    private bool HasEnumFlag(SerializedProperty property, int flag)
    {
      if (flag == 0)
        return property.enumValueFlag == 0;
      else
        return (property.enumValueFlag & flag) == flag;
    }

    // Set the enum flag of a property
    private void SetEnumFlag(SerializedProperty property, int flag)
    {
      property.serializedObject.Update();

      if (flag == 0)
        property.enumValueFlag = 0;
      if ((property.enumValueFlag & flag) == flag)
        property.enumValueFlag &= ~flag;
      else
        property.enumValueFlag |= flag;

      property.serializedObject.ApplyModifiedProperties();
    }
  }
}