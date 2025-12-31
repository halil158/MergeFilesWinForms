# Changelog

All notable changes to this project will be documented in this file.

## [2.0.0] - 2026-01-01

### Added
- **MergeFilesWPF**: New WPF version with modern dark theme UI
  - Dark title bar integration with Windows theme
  - Custom styled buttons with hover effects
  - Drag & drop overlay animation
  - Empty state placeholder with icon
  - Success animation when files are added
  - Compact file list view
- **include.txt**: New configuration file to override ignore rules
  - Allows including specific files from ignored folders
  - Uses relative path matching (suffix match)
  - Example: `build/zephyr/zephyr.dts`
- **Automatic AI header**: Merged files now include automatic context header
  - Lists all included files at the beginning
  - Shows total file count and generation timestamp
  - Helps AI tools understand the merged file structure
- Application icon for window title bar and taskbar

### Changed
- **Localized to English**: All UI text translated to English for global usage
  - Buttons, labels, messages, tooltips, and dialogs
  - Config file comments
- Updated README with new features and WPF version documentation
- Reorganized WinForms UI layout to two rows for better button placement
- Fixed Turkish character encoding issues in WinForms version

### Technical
- WPF version targets .NET 10.0 LTS
- WinForms version targets .NET 9.0
- Added Windows API integration for dark title bar (DwmSetWindowAttribute)

## [1.1.0] - 2025-12-XX

### Added
- Merged file name prefix textbox
- More default file extensions in allow.txt

### Changed
- Removed OK message after merge completion
- Opens explorer with file selected after merge

## [1.0.0] - 2025-XX-XX

### Added
- Initial release
- Drag & drop support for files and folders
- File extension filtering via allow.txt
- Folder exclusion via ignore.txt
- Multi-file merge with encoding detection (UTF-8, UTF-16, UTF-32)
- Quick config file editing from UI
