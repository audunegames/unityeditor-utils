# Audune UnityEditor Utilities

[![openupm](https://img.shields.io/npm/v/com.audune.utils.unityeditor?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.audune.utils.unityeditor/)

Enhance your IMGUI-based editors and property drawers with extension methods and utilities.

## Features

* Extension methods for `EditorGUI` and `EditorGUILayout` to draw multiple properties, an array of properties, children of properties, and inline object references. Draw popups and dropdowns for items, generic menus, enum flags and customized search tree views. Easily position a `Rect` in the editor with its extension methods.
* Build a `ReorderableList` with a useful factory class, optionally with an add dropdown button.
* Serialize types and draw them in the inspector as a dropdown from available child types.

## Installation

### Installing from the OpenUPM registry

To install this package as a package from the OpenUPM registry in the Unity Editor, use the following steps:

* In the Unity editor, navigate to **Edit › Project Settings... › Package Manager**.
* Add the following Scoped Registry, or edit the existing OpenUPM entry to include the new Scope:

```
Name:     package.openupm.com
URL:      https://package.openupm.com
Scope(s): com.audune.utils.unityeditor
```

* Navigate to **Window › Package Manager**.
* Click the **+** icon and click **Add package by name...**
* Enter the following name in the corresponding field and click **Add**:

```
com.audune.utils.unityeditor
```

### Installing as a Git package

To install this package as a Git package in the Unity Editor, use the following steps:

* In the Unity editor, navigate to **Window › Package Manager**.
* Click the **+** icon and click **Add package from git URL...**
* Enter the following URL in the URL field and click **Add**:

```
https://github.com/audunegames/unityeditor-utils.git
```

## License

This package is licensed under the GNU GPL 3.0 license. See `LICENSE.txt` for more information.
