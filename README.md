# MergeFiles

[![Website](https://img.shields.io/badge/Website-mergefiles.yigisoft.com-blue)](https://mergefiles.yigisoft.com/)

**Quick Links:** [Website / Live Demo](https://mergefiles.yigisoft.com/) · [Download (Latest Release)](https://github.com/halil158/MergeFiles/releases/latest) · [Source Code](https://github.com/halil158/MergeFiles)

MergeFiles is a Windows application that allows you to merge multiple files or entire folders into a single text file. It's especially handy for developers who want to collect project files in one place, share code snippets, or prepare data for AI tools.

## Versions

| Version | Framework | Description |
|---------|-----------|-------------|
| **MergeFilesWPF** | .NET 10.0 (LTS) | Modern dark theme UI with animations |
| **MergeFilesWinForms** | .NET 9.0 | Classic Windows Forms UI |

## Features

- **Drag & Drop support**: Drop files or folders directly into the window
- **Add folder with subfolders**: Automatically includes all files in a directory tree
- **File extension filter (allow.txt)**: Control which file types are included
- **Ignore list (ignore.txt)**: Exclude unwanted folders (e.g., `bin`, `obj`, `build`, `node_modules`)
- **Include override (include.txt)**: Force include specific files from ignored folders
- **Automatic AI header**: Merged files include automatic header with file list for AI tools
- **Easy management**: Remove selected items, clear the entire list, and see the total file count
- **File merging**: Combines the contents of all selected files into a single output file
- **Encoding detection**: Supports UTF-8, UTF-16, UTF-32 with BOM detection
- **Quick access**: Opens the merged file's folder automatically after saving
- **Configurable**: Directly open and edit config files from the UI

## Quick Start (Windows)

1. Download the latest release from [GitHub Releases](https://github.com/halil158/MergeFiles/releases/latest) or [Website](https://mergefiles.yigisoft.com/)
2. Extract the ZIP file to any folder
3. Run `MergeFilesWPF.exe` (modern UI) or `MergeFilesWinForms.exe` (classic UI)
4. Drag & drop files/folders or use "Add Folder" button
5. Click "Merge" to combine files into a single text file
6. (Optional) Edit config files via the UI: `allow.txt`, `ignore.txt`, `include.txt`

## Configuration Files

### allow.txt
Controls which file extensions are included:
```
.c
.cpp
.h
.cs
.js
.ts
.json
.xml
.yaml
```

### ignore.txt
Folders/paths to exclude:
```
bin
obj
node_modules
build
.vs
```

### include.txt
Override ignore rules for specific files (relative paths):
```
# Include specific files from ignored folders
build/zephyr/zephyr.dts
build/zephyr/zephyr.map
```

## Merged File Format

The merged output file includes an automatic header for AI context:
```
===== MERGED FILE INFO =====

This file contains the merged contents of multiple source files.
Each file's content is separated by a header showing the file name.

Total files: 5
Generated: 2026-01-01 12:00:00

Files included:
  - App.xaml.cs
  - MainWindow.xaml
  - MainWindow.xaml.cs
  ...

===== FILE CONTENTS =====
```

## Use Cases

- Collect code or config files into one sharable text file
- Prepare project files for AI tools (e.g., ChatGPT, Claude, Copilot)
- Merge logs, configs, and scripts into a single file
- Create project snapshots for code review

## Screenshots

### WPF Version (Dark Theme)
Modern UI with dark theme, hover effects, and drag & drop animations.
![MergeFiles WPF Dark Theme - Modern UI with animations](screenshots/MergeFilesWPF.png)

### WinForms Version
Classic Windows Forms interface.
![MergeFiles WinForms - Classic Windows Forms UI](screenshots/MergeFilesWinForms.png)

## Build

### Requirements
- Visual Studio 2022 or later
- .NET 10.0 SDK (for WPF version)
- .NET 9.0 SDK (for WinForms version)

### Build Commands
```bash
# WPF Version
dotnet build MergeFilesWPF/MergeFilesWPF.csproj

# WinForms Version
dotnet build MergeFilesWinForms/MergeFilesWinForms.csproj
```

## Topics

`wpf` `winforms` `dotnet` `desktop-app` `file-merge` `developer-tools` `ai-tools`

## License

Released under the MIT License – free for personal and commercial use.
