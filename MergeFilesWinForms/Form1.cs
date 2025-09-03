using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MergeFilesWinForms
{
    public partial class Form1 : Form
    {
        // UI
        private readonly ListBox lstFiles = new ListBox();
        private readonly Button btnMerge = new Button();
        private readonly Button btnAddFolder = new Button();
        private readonly Button btnRemoveSelected = new Button();
        private readonly Button btnClear = new Button();
        private readonly Button btnEditAllow = new Button();
        private readonly Button btnEditIgnore = new Button();
        private readonly Label lblCount = new Label();

        // Durum
        private readonly List<string> _files = new List<string>();

        // Config dosyalarý
        private readonly string allowFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "allow.txt");
        private readonly string ignoreFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ignore.txt");

        public Form1()
        {
            InitializeComponent();
            EnsureConfigFiles();
            BuildUi();
            HookDnD();
        }

        private void EnsureConfigFiles()
        {
            if (!File.Exists(allowFile))
            {
                File.WriteAllLines(allowFile, new[]
                {
                    ".c",".cpp",".h",".hpp",".cs",".js",".ts",".tsx",".css",".xaml",".xml",".json",
                    ".html",".md",".ini",".cfg",".py",".sql",".shader"
                });
            }

            if (!File.Exists(ignoreFile))
            {
                File.WriteAllLines(ignoreFile, new[]
                {
                    "bin","obj","node_modules","wwwroot/vendor"
                });
            }
        }

        private void BuildUi()
        {
            Text = "Merge Files (Drag Drop)";
            Width = 1000;
            Height = 650;
            StartPosition = FormStartPosition.CenterScreen;

            // Liste
            lstFiles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstFiles.HorizontalScrollbar = true;
            lstFiles.IntegralHeight = false;
            lstFiles.Left = 10;
            lstFiles.Top = 10;
            lstFiles.Width = ClientSize.Width - 20;
            lstFiles.Height = ClientSize.Height - 160;
            lstFiles.BorderStyle = BorderStyle.FixedSingle;

            // Butonlar
            btnMerge.Text = "Birleþtir (MergedFiles.txt)";
            btnMerge.Width = 220;
            btnMerge.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnMerge.Left = ClientSize.Width - btnMerge.Width - 10;
            btnMerge.Top = ClientSize.Height - 50;
            btnMerge.Click += (s, e) => SaveCombined();

            btnAddFolder.Text = "Klasör Ekle";
            btnAddFolder.Width = 120;
            btnAddFolder.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAddFolder.Left = 10;
            btnAddFolder.Top = ClientSize.Height - 50;
            btnAddFolder.Click += (s, e) => AddFolder();

            btnRemoveSelected.Text = "Seçileni Kaldýr";
            btnRemoveSelected.Width = 140;
            btnRemoveSelected.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnRemoveSelected.Left = btnAddFolder.Right + 10;
            btnRemoveSelected.Top = btnAddFolder.Top;
            btnRemoveSelected.Click += (s, e) => RemoveSelected();

            btnClear.Text = "Temizle";
            btnClear.Width = 100;
            btnClear.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClear.Left = btnRemoveSelected.Right + 10;
            btnClear.Top = btnAddFolder.Top;
            btnClear.Click += (s, e) => { _files.Clear(); RefreshList(); };

            btnEditAllow.Text = "Allow.txt Düzenle";
            btnEditAllow.Width = 150;
            btnEditAllow.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnEditAllow.Left = btnClear.Right + 10;
            btnEditAllow.Top = btnAddFolder.Top;
            btnEditAllow.Click += (s, e) => OpenFile(allowFile);

            btnEditIgnore.Text = "Ignore.txt Düzenle";
            btnEditIgnore.Width = 150;
            btnEditIgnore.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnEditIgnore.Left = btnEditAllow.Right + 10;
            btnEditIgnore.Top = btnAddFolder.Top;
            btnEditIgnore.Click += (s, e) => OpenFile(ignoreFile);

            // Sayaç
            lblCount.AutoSize = true;
            lblCount.Left = 10;
            lblCount.Top = lstFiles.Bottom + 10;
            lblCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            UpdateCount();

            Controls.AddRange(new Control[] {
                lstFiles, btnMerge, btnAddFolder, btnRemoveSelected, btnClear,
                btnEditAllow, btnEditIgnore, lblCount
            });

            // Resize
            Resize += (s, e) =>
            {
                lstFiles.Width = ClientSize.Width - 20;
                lstFiles.Height = ClientSize.Height - 160;
                btnMerge.Left = ClientSize.Width - btnMerge.Width - 10;
                btnMerge.Top = ClientSize.Height - 50;
                btnAddFolder.Top = ClientSize.Height - 50;
                btnRemoveSelected.Top = ClientSize.Height - 50;
                btnClear.Top = ClientSize.Height - 50;
                btnEditAllow.Top = ClientSize.Height - 50;
                btnEditIgnore.Top = ClientSize.Height - 50;
                lblCount.Top = lstFiles.Bottom + 10;
            };
        }

        private void HookDnD()
        {
            AllowDrop = true;

            DragEnter += (s, e) =>
            {
                if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.None;
            };

            DragDrop += (s, e) =>
            {
                if (e.Data == null) return;
                var paths = (string[])e.Data.GetData(DataFormats.FileDrop);
                AddPaths(paths);
            };
        }

        private void AddFolder()
        {
            using var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog(this) == DialogResult.OK)
                AddPaths(new[] { fbd.SelectedPath });
        }

        private void RemoveSelected()
        {
            var toRemove = lstFiles.SelectedItems.Cast<string>().ToList();
            if (toRemove.Count == 0) return;

            foreach (var p in toRemove)
                _files.RemoveAll(x => string.Equals(x, p, StringComparison.OrdinalIgnoreCase));

            RefreshList();
        }

        private void AddPaths(IEnumerable<string> paths)
        {
            if (paths == null) return;

            foreach (var p in paths)
            {
                if (Directory.Exists(p))
                {
                    try
                    {
                        foreach (var f in Directory.EnumerateFiles(p, "*.*", SearchOption.AllDirectories))
                        {
                            if (IsAllowed(f) && !IsIgnored(f))
                                AddIfNew(f);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, $"Klasör okunamadý: {p}\n{ex.Message}", "Hata",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (File.Exists(p) && IsAllowed(p) && !IsIgnored(p))
                {
                    AddIfNew(p);
                }
            }

            RefreshList();
        }

        private HashSet<string> GetAllowedExt()
        {
            try
            {
                return new HashSet<string>(
                    File.ReadAllLines(allowFile)
                        .Where(l => !string.IsNullOrWhiteSpace(l))
                        .Select(l => l.Trim())
                        .Where(l => l.StartsWith(".")),
                    StringComparer.OrdinalIgnoreCase);
            }
            catch
            {
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }
        }

        private List<string> GetIgnoredFolders()
        {
            try
            {
                return File.ReadAllLines(ignoreFile)
                    .Where(l => !string.IsNullOrWhiteSpace(l))
                    .Select(l => l.Trim())
                    .ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        private bool IsAllowed(string path)
            => GetAllowedExt().Contains(Path.GetExtension(path) ?? string.Empty);

        private bool IsIgnored(string path)
        {
            var lower = path.ToLowerInvariant();
            return GetIgnoredFolders().Any(ig => lower.Contains(ig.ToLowerInvariant()));
        }

        private void AddIfNew(string path)
        {
            if (!_files.Contains(path, StringComparer.OrdinalIgnoreCase))
                _files.Add(path);
        }

        private void RefreshList()
        {
            lstFiles.BeginUpdate();
            try
            {
                lstFiles.Items.Clear();
                foreach (var f in _files.OrderBy(Path.GetFileName, StringComparer.OrdinalIgnoreCase))
                    lstFiles.Items.Add(f);
            }
            finally { lstFiles.EndUpdate(); }

            UpdateCount();
        }

        private void UpdateCount()
            => lblCount.Text = $"Toplam dosya: {lstFiles.Items.Count}";

        private void SaveCombined()
        {
            if (_files.Count == 0)
            {
                MessageBox.Show(this, "Birleþtirilecek dosya yok.", "Uyarý",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Title = "Birleþtirilmiþ dosyayý kaydet",
                Filter = "Metin Dosyasý|*.txt|Tümü|*.*",
                FileName = "MergedFiles.txt",
                OverwritePrompt = true
            };
            if (sfd.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                using var fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write, FileShare.Read);
                using var writer = new StreamWriter(fs, new UTF8Encoding(false));

                foreach (var file in _files)
                {
                    writer.WriteLine($"===== {Path.GetFileName(file)} =====");
                    writer.WriteLine();

                    string content;
                    try { content = File.ReadAllText(file, DetectEncoding(file)); }
                    catch { content = File.ReadAllText(file, new UTF8Encoding(false)); }

                    writer.WriteLine(content);
                    writer.WriteLine();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Kaydetme hatasý: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{sfd.FileName}\"");
            }
            catch { }            
        }

        private static Encoding DetectEncoding(string filePath)
        {
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            if (fs.Length >= 4)
            {
                var bom = new byte[4];
                _ = fs.Read(bom, 0, 4);
                if (bom[0] == 0xEF && bom[1] == 0xBB && bom[2] == 0xBF) return new UTF8Encoding(true);
                if (bom[0] == 0xFF && bom[1] == 0xFE) return Encoding.Unicode;
                if (bom[0] == 0xFE && bom[1] == 0xFF) return Encoding.BigEndianUnicode;
                if (bom[0] == 0xFF && bom[1] == 0xFE && bom[2] == 0x00 && bom[3] == 0x00) return Encoding.UTF32;
                if (bom[0] == 0x00 && bom[1] == 0x00 && bom[2] == 0xFE && bom[3] == 0xFF) return new UTF32Encoding(true, true);
            }
            return new UTF8Encoding(false);
        }

        private void OpenFile(string path)
        {
            try
            {
                System.Diagnostics.Process.Start("notepad.exe", $"\"{path}\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Dosya açýlamadý: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
