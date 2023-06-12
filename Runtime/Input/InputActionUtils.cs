using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

using RebindingOperation = UnityEngine.InputSystem.InputActionRebindingExtensions.RebindingOperation;

namespace Audune.Utils.Input
{
  // Class that defines utility methods for actions and bindings
  public static class InputActionUtils
  {
    #region Getting runtime bindings of UnityEngine.InputSystem.InputAction that match a control scheme
    // Return the bindings of an action that match the specified control scheme
    public static IEnumerable<Binding> GetBindingsForControlScheme(this InputAction action, InputControlScheme controlScheme)
    {
      return action.bindings
        .Select((binding, index) => new Binding(action, controlScheme, binding, index))
        .Where(binding => InputBinding.MaskByGroup(controlScheme.bindingGroup).Matches(binding.binding));
    }

    // Return the bindings of an action grouped by their composite name that match the specified control scheme
    public static IEnumerable<BindingGroup> GetGroupedBindingsForControlScheme(this InputAction action, InputControlScheme controlScheme)
    {
      return GetBindingsForControlScheme(action, controlScheme)
        .GroupBy(binding => binding.binding.name)
        .Select(bindingGroup => new BindingGroup(action, controlScheme, bindingGroup.Key, bindingGroup));
    }

    // Return the bindings of an action with the specified composite name that match the specified control scheme
    public static BindingGroup GetGroupedBindingsForControlScheme(this InputAction action, InputControlScheme controlScheme, string partOfCompositeName)
    {
      return GetGroupedBindingsForControlScheme(action, controlScheme)
        .FirstOrDefault(bindings => bindings.partOfCompositeName == partOfCompositeName);
    }
    #endregion

    #region Getting runtime bindings of an enumerable of UnityEngine.InputSystem.InputAction that match a control scheme
    // Return the bindings of an enumerable of actions that match the specified control scheme
    public static IEnumerable<Binding> GetBindingsForControlScheme(this IEnumerable<InputAction> actions, InputControlScheme controlScheme)
    {
      return actions.SelectMany(action => GetBindingsForControlScheme(action, controlScheme));
    }

    // Return the bindings of an enumerable of actions grouped by their composite name that match the specified control scheme
    public static IEnumerable<BindingGroup> GetGroupedBindingsForControlScheme(this IEnumerable<InputAction> actions, InputControlScheme controlScheme)
    {
      return actions.SelectMany(action => GetGroupedBindingsForControlScheme(action, controlScheme));
    }
    #endregion

    #region Upating runtime bindings
    // Return a binding group with updated bindings for the binding group
    public static BindingGroup WithUpdatedBindings(this BindingGroup bindings)
    {
      return GetGroupedBindingsForControlScheme(bindings.action, bindings.controlScheme, bindings.partOfCompositeName);
    }
    #endregion

    #region Rebinding runtime bindings
    // Perform interactive rebinding on a binding
    public static RebindingOperation PerformInteractiveRebinding(this Binding binding)
    {
      return binding.action.PerformInteractiveRebinding(binding.bindingIndex);
    }

    // Perform interactive rebinding on a binding group where the binding is selected by the specified predicate
    public static RebindingOperation PerformInteractiveRebinding(this BindingGroup bindings, Predicate<Binding> bindingPredicate)
    {
      var binding = bindings.bindings.Find(bindingPredicate);
      if (binding == null)
        throw new ArgumentException("Could not find a binding that matches the predicate", nameof(binding));

      return PerformInteractiveRebinding(binding);
    }


    // Remove overrides on a binding
    public static void RemoveBindingOverride(this Binding binding)
    {
      binding.action.RemoveBindingOverride(binding.bindingIndex);
    }

    // Remove overrides on a binding group where the binding is selected by the specified predicate
    public static void RemoveBindingOverride(this BindingGroup bindings, Predicate<Binding> bindingPredicate)
    {
      var binding = bindings.bindings.Find(bindingPredicate);
      if (binding == null)
        throw new ArgumentException("Could not find a binding that matches the predicate", nameof(binding));

      RemoveBindingOverride(binding);
    }
    #endregion
  }
}