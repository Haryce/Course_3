using System;
using System.Drawing;
using System.Windows.Forms;
using SneakerShop.Models;
using SneakerShop.Services;

namespace SneakerShop.Forms
{
    public partial class SneakerForm : Form
    {
        private DatabaseService _dbService;
        public Sneaker Sneaker { get; private set; }

        private TextBox txtName, txtDescription, txtPrice, txtSize, txtColor, txtStockQuantity, txtImageUrl;
        private ComboBox cmbBrand, cmbCategory;
        private CheckBox chkAvailable;
        private Button btnSave, btnCancel;

        public SneakerForm(DatabaseService dbService, Sneaker sneaker = null)
        {
            _dbService = dbService;
            Sneaker = sneaker ?? new Sneaker();
            InitializeComponent();
            LoadComboBoxData();
            
            if (sneaker != null)
                LoadSneakerData();
        }

        private void InitializeComponent()
        {
            this.Text = Sneaker.SneakerId == 0 ? "Добавить кроссовки" : "Редактировать кроссовки";
            this.Size = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };
            int y = 20;

            // Название
            panel.Controls.Add(new Label { Text = "Название:*", Location = new Point(10, y), AutoSize = true });
            txtName = new TextBox { Location = new Point(120, y - 3), Width = 300 };
            panel.Controls.Add(txtName);
            y += 40;

            // Описание
            panel.Controls.Add(new Label { Text = "Описание:", Location = new Point(10, y), AutoSize = true });
            txtDescription = new TextBox { Location = new Point(120, y - 3), Width = 300, Height = 60 };
            txtDescription.Multiline = true;
            txtDescription.Height = 60;
            panel.Controls.Add(txtDescription);
            y += 80;

            // Бренд
            panel.Controls.Add(new Label { Text = "Бренд:*", Location = new Point(10, y), AutoSize = true });
            cmbBrand = new ComboBox { Location = new Point(120, y - 3), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            panel.Controls.Add(cmbBrand);
            y += 40;

            // Категория
            panel.Controls.Add(new Label { Text = "Категория:*", Location = new Point(10, y), AutoSize = true });
            cmbCategory = new ComboBox { Location = new Point(120, y - 3), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            panel.Controls.Add(cmbCategory);
            y += 40;

            // Цена
            panel.Controls.Add(new Label { Text = "Цена:*", Location = new Point(10, y), AutoSize = true });
            txtPrice = new TextBox { Location = new Point(120, y - 3), Width = 100 };
            panel.Controls.Add(txtPrice);
            y += 40;

            // Размер
            panel.Controls.Add(new Label { Text = "Размер:*", Location = new Point(10, y), AutoSize = true });
            txtSize = new TextBox { Location = new Point(120, y - 3), Width = 100 };
            panel.Controls.Add(txtSize);
            y += 40;

            // Цвет
            panel.Controls.Add(new Label { Text = "Цвет:*", Location = new Point(10, y), AutoSize = true });
            txtColor = new TextBox { Location = new Point(120, y - 3), Width = 200 };
            panel.Controls.Add(txtColor);
            y += 40;

            // Количество
            panel.Controls.Add(new Label { Text = "Количество:*", Location = new Point(10, y), AutoSize = true });
            txtStockQuantity = new TextBox { Location = new Point(120, y - 3), Width = 100 };
            panel.Controls.Add(txtStockQuantity);
            y += 40;

            // Изображение
            panel.Controls.Add(new Label { Text = "Изображение:", Location = new Point(10, y), AutoSize = true });
            txtImageUrl = new TextBox { Location = new Point(120, y - 3), Width = 300 };
            panel.Controls.Add(txtImageUrl);
            y += 40;

            // Доступность
            chkAvailable = new CheckBox { Text = "Доступен", Location = new Point(120, y), Checked = true };
            panel.Controls.Add(chkAvailable);
            y += 40;

            // Кнопки
            btnSave = new Button { Text = "Сохранить", Location = new Point(120, y), Width = 80 };
            btnCancel = new Button { Text = "Отмена", Location = new Point(210, y), Width = 80 };

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            panel.Controls.Add(btnSave);
            panel.Controls.Add(btnCancel);

            this.Controls.Add(panel);
        }

        private void LoadComboBoxData()
        {
            var brands = _dbService.GetAllBrands();
            var categories = _dbService.GetAllCategories();

            cmbBrand.DataSource = brands;
            cmbBrand.DisplayMember = "Name";
            cmbBrand.ValueMember = "BrandId";

            cmbCategory.DataSource = categories;
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "CategoryId";
        }

        private void LoadSneakerData()
        {
            txtName.Text = Sneaker.Name;
            txtDescription.Text = Sneaker.Description;
            txtPrice.Text = Sneaker.Price.ToString();
            txtSize.Text = Sneaker.Size.ToString();
            txtColor.Text = Sneaker.Color;
            txtStockQuantity.Text = Sneaker.StockQuantity.ToString();
            txtImageUrl.Text = Sneaker.ImageUrl;
            chkAvailable.Checked = Sneaker.IsAvailable;

            cmbBrand.SelectedValue = Sneaker.BrandId;
            cmbCategory.SelectedValue = Sneaker.CategoryId;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                try
                {
                    Sneaker.Name = txtName.Text;
                    Sneaker.Description = txtDescription.Text;
                    Sneaker.Price = decimal.Parse(txtPrice.Text);
                    Sneaker.Size = int.Parse(txtSize.Text);
                    Sneaker.Color = txtColor.Text;
                    Sneaker.StockQuantity = int.Parse(txtStockQuantity.Text);
                    Sneaker.ImageUrl = txtImageUrl.Text;
                    Sneaker.IsAvailable = chkAvailable.Checked;
                    Sneaker.BrandId = (int)cmbBrand.SelectedValue;
                    Sneaker.CategoryId = (int)cmbCategory.SelectedValue;

                    bool success;
                    if (Sneaker.SneakerId == 0)
                    {
                        success = _dbService.AddSneaker(Sneaker);
                    }
                    else
                    {
                        success = _dbService.UpdateSneaker(Sneaker);
                    }

                    if (success)
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

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название кроссовок", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!int.TryParse(txtSize.Text, out int size) || size < 35 || size > 50)
            {
                MessageBox.Show("Введите корректный размер (35-50)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtColor.Text))
            {
                MessageBox.Show("Введите цвет", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!int.TryParse(txtStockQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Введите корректное количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
    }
}