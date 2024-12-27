# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.6] - 2024-12-10

### Fixed

- Drawn item popups don't throuw exceptions anymore when an index is out of range.

## [2.0.5] - 2024-06-16

## Fixed

- Drawing item popups in editors now work properly with empty lists.

## [2.0.4] - 2024-06-07

### Fixed

- Fixed missing base class for partical classes in the IMGUI utilities.

## [2.0.3] - 2024-06-07

**Note: this release is not suitable to use in a production environment. Please use a higher version instead.**

### Changed

- Refactored IMGUI utilities into partial classes.
- Updated package information.

## [2.0.2] - 2023-05-28

### Fixed

- Selecting the selecting item in a selector tree view now works as intended.

## [2.0.1] - 2024-04-19

### Added

- Property search tree views can now search in values of properties.

### Fixed

- Fixed trimming keys that match exact strings in search tree views.

## [2.0.0] - 2024-04-12

### Added

- Methods to search for serialized properties in the project (prefabs, scriptable objects and scenes).
- Serialized property extensions can now optionally enter their children.

### Changed

- Improved searching in item tree views.

### Removed

- Moved serialized type utilities to their own package (com.audune.utils.types).

## [1.0.8] - 2024-04-07

### Added

- Customization for the search string highlighter in the items tree view.

## [1.0.7] - 2024-04-07

### Changed

- Made HighlightSearchString method in the items tree view virtual to be implemented by subclasses.

## [1.0.6] - 2024-04-03

### Added

- Editor icons now include colors for warnings and errors.

### Changed

- Textures for editor icons are made read-only.

## [1.0.5] - 2024-04-03

### Changed

- Improved layout of the search editor window.

## [1.0.4] - 2024-04-03

### Added

- Editor icons utility class.

### Changed

- Improved tree view code.

## [1.0.3] - 2024-03-20

### Changed

- Updated code for drawing reorderable lists.

## [1.0.2] - 2024-03-20

### Added

- IMGUI editor extension methods for coloring inspector fields.

## [1.0.1] - 2024-02-24

### Added

- Type display name attribute for serialized types.

### Fixed

- Callbacks of reorderable lists now update the serialized property.

## [1.0.0] - 2024-02-22

### Added

- IMGUI editor extension methods for drawing multiple property fields, inline objects, and item popups.
- Reorderable list builder classes for use in editor code.
- Search editor windows and tree views.
- Serialized C# type reference for use in the inspector.
- ExecutionMode enum to execute code based on the build and player state.

### Removed

- Moved input system utilities to their own package (com.audune.utils.inputsystem).
- Moved spritesheet utilities to their own package (com.audune.spritesheet).

## [0.2.0] - 2023-05-03

### Changed

- Moved RawTexture to editor scripts.

## [0.1.0] - 2023-04-27

### Added

- Input system utilities to abstract away bindings and sprites.
- Utilities for spritesheets defined in CSV format.