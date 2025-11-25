using System;
using System.Drawing;
using System.Windows.Forms;
using SneakerShop.Models;
using SneakerShop.Services;

namespace SneakerShop.Forms
{
    public partial class BrandForm : Form
    {
        private DatabaseService _dbService;
        public Brand Brand { get; private set; }

        private TextBox txtName, txtCountry, txtDescription;
        private Button btnSave, btnCancel;

        public BrandForm(DatabaseService dbService)
        {
            _dbService = dbService;
            Brand = new Brand();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Добавить бренд";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;

            var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };
            int y = 20;

            panel.Controls.Add(new Label { Text = "Название:*", Location = new Point(10, y), AutoSize = true });
            txtName = new TextBox { Location = new Point(120, y - 3), Width = 200 };
            panel.Controls.Add(txtName);
            y += 40;

            panel.Controls.Add(new Label { Text = "Страна:", Location = new Point(10, y), AutoSize = true });
            txtCountry = new TextBox { Location = new Point(120, y - 3), Width = 200 };
            panel.Controls.Add(txtCountry);
            y += 40;

            panel.Controls.Add(new Label { Text = "Описание:", Location = new Point(10, y), AutoSize = true });
            txtDescription = new TextBox { Location = new Point(120, y - 3), Width = 200, Height = 60 };
            txtDescription.Multiline = true;
            txtDescription.Height = 60;
            panel.Controls.Add(txtDescription);
            y += 80;

            btnSave = new Button { Text = "Сохранить", Location = new Point(120, y), Width = 80 };
            btnCancel = new Button { Text = "Отмена", Location = new Point(210, y), Width = 80 };

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            panel.Controls.Add(btnSave);
            panel.Controls.Add(btnCancel);
            this.Controls.Add(panel);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название бренда", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Brand.Name = txtName.Text;
            Brand.Country = txtCountry.Text;
            Brand.Description = txtDescription.Text;

            try
            {
                if (_dbService.AddBrand(Brand))
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

// Forms/CategoryForm.cs (аналогично BrandForm)
// Forms/CustomerForm.cs (аналогично другим формам)