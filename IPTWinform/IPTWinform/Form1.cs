using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace IPTWinform
{
    public partial class Form1 : Form
    {
        private List<TodoItem> todoItems = new List<TodoItem>();
        private ListView todoListView;
        private TextBox taskTextBox;
        private ComboBox priorityComboBox;
        private Button addButton;
        private Button deleteButton;
        private Button completeButton;
        private Panel mainPanel;
        private Label titleLabel;
        private Label taskLabel;
        private Label priorityLabel;

        public Form1()
        {
            InitializeComponent();
            InitializeTodoList();
        }

        private void InitializeTodoList()
        {
            this.Text = "Todo List Manager";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240);

            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            titleLabel = new Label
            {
                Text = "Todo List Manager",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(64, 64, 64),
                Location = new Point(20, 20),
                AutoSize = true
            };

            taskLabel = new Label
            {
                Text = "New Task:",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.FromArgb(64, 64, 64),
                Location = new Point(20, 80),
                AutoSize = true
            };

            taskTextBox = new TextBox
            {
                Location = new Point(20, 110),
                Size = new Size(350, 30),
                Font = new Font("Segoe UI", 12),
                BorderStyle = BorderStyle.FixedSingle
            };
            taskTextBox.KeyPress += TaskTextBox_KeyPress;

            priorityLabel = new Label
            {
                Text = "Priority:",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.FromArgb(64, 64, 64),
                Location = new Point(390, 80),
                AutoSize = true
            };

            priorityComboBox = new ComboBox
            {
                Location = new Point(390, 110),
                Size = new Size(150, 30),
                Font = new Font("Segoe UI", 12),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            priorityComboBox.Items.AddRange(new string[] { "High", "Medium", "Low" });
            priorityComboBox.SelectedIndex = 1;

            addButton = new Button
            {
                Text = "Add Task",
                Location = new Point(560, 110),
                Size = new Size(170, 30),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            addButton.FlatAppearance.BorderSize = 0;
            addButton.Click += AddButton_Click;

            todoListView = new ListView
            {
                Location = new Point(20, 160),
                Size = new Size(710, 300),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Font = new Font("Segoe UI", 10)
            };
            todoListView.Columns.Add("Task", 400);
            todoListView.Columns.Add("Priority", 100);
            todoListView.Columns.Add("Status", 200);

            completeButton = new Button
            {
                Text = "Mark Complete",
                Location = new Point(20, 480),
                Size = new Size(150, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(0, 150, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            completeButton.FlatAppearance.BorderSize = 0;
            completeButton.Click += CompleteButton_Click;

            deleteButton = new Button
            {
                Text = "Delete Task",
                Location = new Point(190, 480),
                Size = new Size(150, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(215, 0, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            deleteButton.FlatAppearance.BorderSize = 0;
            deleteButton.Click += DeleteButton_Click;

            mainPanel.Controls.AddRange(new Control[] {
                titleLabel, taskLabel, taskTextBox, priorityLabel,
                priorityComboBox, addButton, todoListView,
                completeButton, deleteButton
            });

            this.Controls.Add(mainPanel);
        }

        private void TaskTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                AddTask();
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            AddTask();
        }

        private void AddTask()
        {
            string taskText = taskTextBox.Text.Trim();
            
            if (string.IsNullOrWhiteSpace(taskText))
            {
                MessageBox.Show("Please enter a task!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                taskTextBox.Focus();
                return;
            }

            if (taskText.Length > 100)
            {
                MessageBox.Show("Task description is too long. Maximum 100 characters allowed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                taskTextBox.Focus();
                return;
            }

            if (todoItems.Any(x => x.Task.Equals(taskText, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("This task already exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                taskTextBox.Focus();
                return;
            }

            var item = new TodoItem
            {
                Task = taskText,
                Priority = priorityComboBox.SelectedItem.ToString(),
                Status = "Pending"
            };

            todoItems.Add(item);
            UpdateListView();
            taskTextBox.Clear();
            taskTextBox.Focus();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (todoListView.SelectedItems.Count > 0)
            {
                var selectedItem = todoListView.SelectedItems[0];
                todoItems.RemoveAt(selectedItem.Index);
                UpdateListView();
            }
            else
            {
                MessageBox.Show("Please select a task to delete!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CompleteButton_Click(object sender, EventArgs e)
        {
            if (todoListView.SelectedItems.Count > 0)
            {
                var selectedItem = todoListView.SelectedItems[0];
                todoItems[selectedItem.Index].Status = "Completed";
                UpdateListView();
            }
            else
            {
                MessageBox.Show("Please select a task to mark as complete!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateListView()
        {
            todoListView.Items.Clear();
            foreach (var item in todoItems)
            {
                var listItem = new ListViewItem(item.Task);
                listItem.SubItems.Add(item.Priority);
                listItem.SubItems.Add(item.Status);
                todoListView.Items.Add(listItem);
            }
        }
    }

    public class TodoItem
    {
        public string Task { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
    }
}
