using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace TextQuestGame
{
    public partial class MainForm : Form, IGameView
    {
        private FlowLayoutPanel choicePanel;
        private RichTextBox sceneText;
        private MenuStrip menu;
        private PictureBox scenePictureBox;
        private ListBox inventoryListBox;
        private Label inventoryLabel;
        private Panel inventoryPanel;
        private ToolStripStatusLabel statusLabel;
        private ToolStrip toolStrip;
        private ToolTip buttonToolTip;
        private StatusStrip statusStrip;
        public MainForm()
        {
            InitializeComponent();
            buttonToolTip = new ToolTip
            {
                AutomaticDelay = 500,
                AutoPopDelay = 5000,
                InitialDelay = 500,
                ReshowDelay = 100   
            };

            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Font = new Font("Segoe UI", 9);
            this.Text = "Текстовый квест - Приключение начинается!";
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            CreateControls();
            InitializeContextMenu();

        }

        private void CreateControls()
        {
            menu = new MenuStrip
            {
                BackColor = Color.SteelBlue,
                ForeColor = Color.White
            };

            var fileMenu = new ToolStripMenuItem("Файл");
            fileMenu.ForeColor = Color.White;

            var newGameItem = new ToolStripMenuItem("Новая игра")
            {
                ShortcutKeys = Keys.Control | Keys.N,
                ToolTipText = "Начать новую игру (Ctrl+N)"
            };
            newGameItem.Click += (s, e) => NewGameRequested?.Invoke();

            var saveItem = new ToolStripMenuItem("Сохранить игру")
            {
                ShortcutKeys = Keys.Control | Keys.S,
                ToolTipText = "Сохранить текущую игру (Ctrl+S)"
            };
            saveItem.Click += (s, e) => SaveRequested?.Invoke();

            var loadItem = new ToolStripMenuItem("Загрузить игру")
            {
                ShortcutKeys = Keys.Control | Keys.L,
                ToolTipText = "Загрузить сохранённую игру (Ctrl+L)"
            };
            loadItem.Click += (s, e) => LoadRequested?.Invoke();

            var exitItem = new ToolStripMenuItem("Выход")
            {
                ShortcutKeys = Keys.Alt | Keys.F4
            };
            exitItem.Click += (s, e) => Application.Exit();

            fileMenu.DropDownItems.AddRange(new ToolStripItem[] {
                newGameItem, saveItem, loadItem, new ToolStripSeparator(), exitItem
            });
            menu.Items.Add(fileMenu);

            toolStrip = new ToolStrip
            {
                BackColor = Color.LightSteelBlue,
                GripStyle = ToolStripGripStyle.Hidden
            };

            var newButton = new ToolStripButton("Новая")
            {
                ToolTipText = "Новая игра (Ctrl+N)",
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };
            newButton.Click += (s, e) => NewGameRequested?.Invoke();

            var saveButton = new ToolStripButton("Сохранить")
            {
                ToolTipText = "Сохранить игру (Ctrl+S)",
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };
            saveButton.Click += (s, e) => SaveRequested?.Invoke();

            var loadButton = new ToolStripButton("Загрузить")
            {
                ToolTipText = "Загрузить игру (Ctrl+L)",
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };
            loadButton.Click += (s, e) => LoadRequested?.Invoke();

            toolStrip.Items.Add(newButton);
            toolStrip.Items.Add(new ToolStripSeparator());
            toolStrip.Items.Add(saveButton);
            toolStrip.Items.Add(loadButton);

            scenePictureBox = new PictureBox
            {
                Dock = DockStyle.Top,
                Height = 250,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Black,
                BorderStyle = BorderStyle.Fixed3D,
                Padding = new Padding(5)
            };

            sceneText = new RichTextBox
            {
                Dock = DockStyle.Top,
                Height = 150,
                Font = new Font("Georgia", 11),
                BackColor = Color.Beige,
                ForeColor = Color.DarkSlateGray,
                BorderStyle = BorderStyle.FixedSingle,
                ReadOnly = true,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                Padding = new Padding(10),
                Margin = new Padding(5, 5, 5, 10)
            };

            inventoryPanel = new Panel
            {
                Dock = DockStyle.Right,
                Width = 220,
                BackColor = Color.LightSteelBlue,
                Padding = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle
            };

            inventoryLabel = new Label
            {
                Text = "ИНВЕНТАРЬ",
                Dock = DockStyle.Top,
                Font = new Font("Arial", 11, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 30,
                ForeColor = Color.DarkBlue,
                BackColor = Color.LightBlue
            };

            inventoryListBox = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 10),
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.DarkGreen,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 5, 0, 0)
            };

            inventoryPanel.Controls.Add(inventoryListBox);
            inventoryPanel.Controls.Add(inventoryLabel);


            choicePanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                BackColor = Color.FromArgb(230, 240, 255),
                Padding = new Padding(15),
                WrapContents = false
            };


            statusStrip = new StatusStrip
            {
                BackColor = Color.SteelBlue,
                ForeColor = Color.White
            };

            statusLabel = new ToolStripStatusLabel
            {
                Text = "Готов к игре",
                Spring = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };

            statusStrip.Items.Add(statusLabel);


            Controls.AddRange(new Control[]
            {
                choicePanel,
                inventoryPanel,
                sceneText,
                scenePictureBox,
                toolStrip,
                menu,
                statusStrip
            });
        }

        private void InitializeContextMenu()
        {
            var contextMenu = new ContextMenuStrip();

            var copyItem = new ToolStripMenuItem("Копировать текст");
            copyItem.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(sceneText.SelectedText))
                {
                    Clipboard.SetText(sceneText.SelectedText);
                }
            };

            contextMenu.Items.Add(copyItem);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add("Очистить", null, (s, e) => sceneText.Clear());

            sceneText.ContextMenuStrip = contextMenu;
        }

        private Image CreateDefaultImage()
        {
            var bitmap = new Bitmap(800, 250);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.DarkSlateGray);

                var gradientBrush = new LinearGradientBrush(
                    new Point(0, 0),
                    new Point(800, 250),
                    Color.DarkSlateBlue,
                    Color.DarkSlateGray);
                g.FillRectangle(gradientBrush, 0, 0, 800, 250);

                g.DrawString("Текстовый квест",
                    new Font("Arial", 24, FontStyle.Bold),
                    Brushes.White,
                    new PointF(280, 80));

                g.DrawString("Изображение сцены",
                    new Font("Arial", 16, FontStyle.Regular),
                    Brushes.LightGray,
                    new PointF(310, 120));

                g.DrawString("День 2: Улучшенный интерфейс",
                    new Font("Arial", 10, FontStyle.Italic),
                    Brushes.Silver,
                    new PointF(300, 160));

                g.DrawRectangle(new Pen(Color.Silver, 2), 10, 10, 780, 230);
            }
            return bitmap;
        }

        private void DisplayChoices(List<Choice> choices)
        {
            choicePanel.Controls.Clear();

            if (choices == null || choices.Count == 0)
            {
                var label = new Label
                {
                    Text = "Конец игры или нет вариантов выбора. Начните новую игру!",
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    ForeColor = Color.DarkRed,
                    AutoSize = true,
                    Margin = new Padding(10)
                };
                choicePanel.Controls.Add(label);
                return;
            }

            for (int i = 0; i < choices.Count; i++)
            {
                var choice = choices[i];

                var button = new Button
                {
                    Text = $"{i + 1}. {choice.Text}",
                    Tag = i,
                    Width = choicePanel.ClientSize.Width - 40,
                    Height = 50,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    BackColor = Color.CornflowerBlue,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Margin = new Padding(5),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(15, 0, 0, 0)
                };

                button.FlatAppearance.BorderColor = Color.DarkBlue;
                button.FlatAppearance.BorderSize = 2;

                if (!string.IsNullOrEmpty(choice.Condition))
                {
                    buttonToolTip.SetToolTip(button, $"Требуется: {choice.Condition}");
                }

                button.Click += (s, e) => ChoiceSelected?.Invoke((int)button.Tag);

                button.MouseEnter += (s, e) =>
                {
                    button.BackColor = Color.RoyalBlue;
                    button.Cursor = Cursors.Hand;
                };

                button.MouseLeave += (s, e) => button.BackColor = Color.CornflowerBlue;

                choicePanel.Controls.Add(button);
            }
        }

        public void UpdateInventory(List<string> inventory)
        {
            if (inventoryListBox.InvokeRequired)
            {
                inventoryListBox.Invoke(new Action(() => UpdateInventory(inventory)));
                return;
            }

            inventoryListBox.Items.Clear();

            if (inventory == null || inventory.Count == 0)
            {
                inventoryListBox.Items.Add("Инвентарь пуст");
                inventoryListBox.Items.Add("");
                inventoryListBox.Items.Add("Находите предметы");
                inventoryListBox.Items.Add("во время игры!");
                inventoryListBox.ForeColor = Color.Gray;
                inventoryListBox.Font = new Font("Arial", 9, FontStyle.Italic);
            }
            else
            {
                inventoryListBox.Font = new Font("Arial", 10, FontStyle.Regular);
                inventoryListBox.ForeColor = Color.DarkGreen;

                foreach (var item in inventory)
                {
                    inventoryListBox.Items.Add($"✅ {item}");
                }

                inventoryLabel.Text = $"🎒 ИНВЕНТАРЬ ({inventory.Count})";
            }
        }
        public void DisplayScene(string text, List<string> choices)
        {
            sceneText.Text = text;
            choicePanel.Controls.Clear();

            for (int i = 0; i < choices.Count; i++)
            {
                var button = new Button { Text = choices[i], Tag = i, Width = 200 };
                button.Click += (s, e) => ChoiceSelected?.Invoke((int)button.Tag);
                choicePanel.Controls.Add(button);
            }
        
        }
        public event Action<int> ChoiceSelected;
        public event Action SaveRequested;
        public event Action LoadRequested;
        public event Action NewGameRequested;
    }

}
