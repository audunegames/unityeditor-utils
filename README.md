# Audune Utilities

This repository provides utility scripts for Unity 2021.3 and highter, used in Audune's own projects.

## Input

The input utility scripts provide extension methods and extra classes for the [Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.5/manual/index.html) package:

* The `Binding` class is a wrapper around `Unity.InputSystem.InputBinding` and also contains the associated `Unity.InputSystem.InputAction`, `Unity.InputSystem.InputControlScheme` and binding index.
* The `BindingGroup` class is a wrapper around `IEnumerable<Unity.InputSystem.InputBinding>` and also contains the associated `Unity.InputSystem.InputAction`, `Unity.InputSystem.InputControlScheme` and part of composite name.
* The `InputActionUtils` class contains extension methods for actions in the input system.
* The `InputControlSchemeUtils` class contains extension methods for control schemes in the input system:
* The `InputDisplayUtils` class contains extension methods for displaying bindings as strings:
* The `RuntimeControlScheme` class is a wrapper around `Unity.InputSystem.InputControlScheme` and contains the devices that are currently used by the control scheme.

## Sprites

The sprites utility scripts provide classes to load sprites from a sprite sheet CSV file.

## Text

The text utility scripts provide extra classes for the [TextMeshPro](https://docs.unity3d.com/Packages/com.unity.textmeshpro@3.2/manual/index.html) package:

* The `TextMeshProSprite` class is a convenient wrapper around a string containing rich text for a TextMeshPro sprite.
