using Microsoft.Extensions.DependencyInjection;
using SneakerShop.Models;
using SneakerShop.Services;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SneakerShop.Forms
{
    public partial class MainForm : Form
    {
        private readonly ISneakerService _sneakerService;
        private DataGridView sneakersGrid;
        private ComboBox cmbBrandFilter, cmbCategoryFilter;
        private Button btnAdd, btnEdit, btnDelete, btnRefresh;
        private TextBox txtSearch;

        public MainForm(ISneakerService sneakerService)
        {
            _sneakerService = sneakerService;
            InitializeComponent();
            LoadDataAsync();
        }

        private void InitializeComponent()
        {
            this.Text = "Магазин Кроссовок";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Панель фильтров
            var filterPanel = new Panel { Dock = DockStyle.Top, Height = 80, BackColor = Color.LightGray };

            txtSearch = new TextBox { Location = new Point(10, 10), Width = 200, PlaceholderText = "Поиск..." };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            cmbBrandFilter = new ComboBox { Location = new Point(220, 10), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbBrandFilter.SelectedIndexChanged += CmbFilter_SelectedIndexChanged;

            cmbCategoryFilter = new ComboBox { Location = new Point(380, 10), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCategoryFilter.SelectedIndexChanged += CmbFilter_SelectedIndexChanged;

            btnRefresh = new Button { Text = "Обновить", Location = new Point(540, 10), Width = 80 };
            btnRefresh.Click += BtnRefresh_Click;

            filterPanel.Controls.AddRange(new Control[] { 
                new Label { Text = "Поиск:", Location = new Point(10, 12), AutoSize = true },
                txtSearch,
                new Label { Text = "Бренд:", Location = new Point(220, 12), AutoSize = true },
                cmbBrandFilter,
                new Label { Text = "Категория:", Location = new Point(380, 12), AutoSize = true },
                cmbCategoryFilter,
                btnRefresh
            });

            this.Controls.Add(filterPanel);

            // DataGridView
            sneakersGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false
            };

            ConfigureDataGridView();
            this.Controls.Add(sneakersGrid);

            // Панель кнопок
            var buttonPanel = new Panel { Dock = DockStyle.Bottom, Height = 50, BackColor = Color.LightGray };

            btnAdd = new Button { Text = "Добавить", Location = new Point(10, 10), Width = 100 };
            btnEdit = new Button { Text = "Редактировать", Location = new Point(120, 10), Width = 100 };
            btnDelete = new Button { Text = "Удалить", Location = new Point(230, 10), Width = 100 };

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;

            buttonPanel.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete });
            this.Controls.Add(buttonPanel);
        }

        private void ConfigureDataGridView()
        {
            sneakersGrid.Columns.Clear();

            sneakersGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SneakerId",
                HeaderText = "ID",
                Width = 50
            });

            sneakersGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Name",
                HeaderText = "Название",
                Width = 150
            });

            sneakersGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Brand.Name",
                HeaderText = "Бренд",
                Width = 100
            });

            sneakersGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Category.Name",
                HeaderText = "Категория",
                Width = 100
            });

            sneakersGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Price",
                HeaderText = "Цена",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            sneakersGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Size",
                HeaderText = "Размер",
                Width = 60
            });

            sneakersGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Color",
                HeaderText = "Цвет",
                Width = 80
            });

            sneakersGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "StockQuantity",
                HeaderText = "В наличии",
                Width = 70
            });
        }

        private async void LoadDataAsync()
        {
            try
            {
                var sneakers = await _sneakerService.GetAllSneakersAsync();
                var brands = await _sneakerService.GetAllBrandsAsync();
                var categories = await _sneakerService.GetAllCategoriesAsync();

                sneakersGrid.DataSource = sneakers;

                cmbBrandFilter.DataSource = brands;
                cmbBrandFilter.DisplayMember = "Name";
                cmbBrandFilter.ValueMember = "BrandId";
                cmbBrandFilter.Items.Insert(0, "Все бренды");

                cmbCategoryFilter.DataSource = categories;
                cmbCategoryFilter.DisplayMember = "Name";
                cmbCategoryFilter.ValueMember = "CategoryId";
                cmbCategoryFilter.Items.Insert(0, "Все категории");

                cmbBrandFilter.SelectedIndex = 0;
                cmbCategoryFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnAdd_Click(object sender, EventArgs e)
        {
            var form = new SneakerForm(_sneakerService);
            if (form.ShowDialog() == DialogResult.OK)
            {
                await LoadDataAsync();
            }
        }

        private async void BtnEdit_Click(object sender, EventArgs e)
        {
            if (sneakersGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите кроссовки для редактирования", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedSneaker = sneakersGrid.SelectedRows[0].DataBoundItem as Sneaker;
            if (selectedSneaker != null)
            {
                var form = new SneakerForm(_sneakerService, selectedSneaker);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    await LoadDataAsync();
                }
            }
        }

        private async void BtnDelete_Click(object sender, EventArgs e)
        {
            if (sneakersGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите кроссовки для удаления", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedSneaker = sneakersGrid.SelectedRows[0].DataBoundItem as Sneaker;
            if (selectedSneaker != null)
            {
                var result = MessageBox.Show($"Удалить кроссовки '{selectedSneaker.Name}'?", "Подтверждение", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        await _sneakerService.DeleteSneakerAsync(selectedSneaker.SneakerId);
                        await LoadDataAsync();
                        MessageBox.Show("Кроссовки удалены", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            // Реализация поиска
            if (sneakersGrid.DataSource is List<Sneaker> sneakers)
            {
                var filtered = sneakers.Where(s => 
                    s.Name.ToLower().Contains(txtSearch.Text.ToLower()) ||
                    s.Brand.Name.ToLower().Contains(txtSearch.Text.ToLower()) ||
                    s.Color.ToLower().Contains(txtSearch.Text.ToLower())).ToList();

                sneakersGrid.DataSource = filtered;
            }
        }

        private void CmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Реализация фильтрации
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (sneakersGrid.DataSource is List<Sneaker> sneakers)
            {
                var filtered = sneakers.AsEnumerable();

                if (cmbBrandFilter.SelectedIndex > 0 && cmbBrandFilter.SelectedValue is int brandId)
                {
                    filtered = filtered.Where(s => s.BrandId == brandId);
                }

                if (cmbCategoryFilter.SelectedIndex > 0 && cmbCategoryFilter.SelectedValue is int categoryId)
                {
                    filtered = filtered.Where(s => s.CategoryId == categoryId);
                }

                sneakersGrid.DataSource = filtered.ToList();
            }
        }

        private async void BtnRefresh_Click(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }
    }
}