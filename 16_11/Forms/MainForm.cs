using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SneakerShop.Services;
using SneakerShop.ViewModels;

namespace SneakerShop.Forms
{
    public partial class MainForm : Form
    {
        private DatabaseService _dbService;
        private TabControl tabControl;
        private DataGridView sneakersGrid, brandsGrid, categoriesGrid, customersGrid;

        public MainForm()
        {
            _dbService = new DatabaseService();
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Магазин Кроссовок - Управление";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            this.Controls.Add(tabControl);

            // Вкладка кроссовок
            var sneakersTab = new TabPage("Кроссовки");
            tabControl.TabPages.Add(sneakersTab);
            InitializeSneakersTab(sneakersTab);

            // Вкладка брендов
            var brandsTab = new TabPage("Бренды");
            tabControl.TabPages.Add(brandsTab);
            InitializeBrandsTab(brandsTab);

            // Вкладка категорий
            var categoriesTab = new TabPage("Категории");
            tabControl.TabPages.Add(categoriesTab);
            InitializeCategoriesTab(categoriesTab);

            // Вкладка клиентов
            var customersTab = new TabPage("Клиенты");
            tabControl.TabPages.Add(customersTab);
            InitializeCustomersTab(customersTab);
        }

        private void LoadData()
        {
            LoadSneakers();
            LoadBrands();
            LoadCategories();
            LoadCustomers();
        }

        #region Sneakers Tab

        private Button btnAddSneaker, btnEditSneaker, btnDeleteSneaker;

        private void InitializeSneakersTab(TabPage tab)
        {
            sneakersGrid = new DataGridView();
            sneakersGrid.Dock = DockStyle.Top;
            sneakersGrid.Height = 400;
            sneakersGrid.AutoGenerateColumns = false;
            sneakersGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            sneakersGrid.ReadOnly = true;
            sneakersGrid.AllowUserToAddRows = false;

            // Настройка колонок
            sneakersGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SneakerId", HeaderText = "ID", Width = 50 });
            sneakersGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Название", Width = 150 });
            sneakersGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "BrandName", HeaderText = "Бренд", Width = 100 });
            sneakersGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CategoryName", HeaderText = "Категория", Width = 100 });
            sneakersGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Price", HeaderText = "Цена", Width = 80 });
            sneakersGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Size", HeaderText = "Размер", Width = 60 });
            sneakersGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Color", HeaderText = "Цвет", Width = 80 });
            sneakersGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "StockQuantity", HeaderText = "В наличии", Width = 70 });
            sneakersGrid.Columns.Add(new DataGridViewCheckBoxColumn { DataPropertyName = "IsAvailable", HeaderText = "Доступен", Width = 70 });

            tab.Controls.Add(sneakersGrid);

            // Панель с кнопками
            var panel = new Panel();
            panel.Dock = DockStyle.Bottom;
            panel.Height = 50;

            btnAddSneaker = new Button { Text = "Добавить", Location = new Point(10, 10), Width = 100 };
            btnEditSneaker = new Button { Text = "Редактировать", Location = new Point(120, 10), Width = 100 };
            btnDeleteSneaker = new Button { Text = "Удалить", Location = new Point(230, 10), Width = 100 };

            btnAddSneaker.Click += BtnAddSneaker_Click;
            btnEditSneaker.Click += BtnEditSneaker_Click;
            btnDeleteSneaker.Click += BtnDeleteSneaker_Click;

            panel.Controls.AddRange(new Control[] { btnAddSneaker, btnEditSneaker, btnDeleteSneaker });
            tab.Controls.Add(panel);
        }

        private void LoadSneakers()
        {
            try
            {
                var sneakers = _dbService.GetAllSneakers();
                sneakersGrid.DataSource = sneakers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки кроссовок: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddSneaker_Click(object sender, EventArgs e)
        {
            var form = new SneakerForm(_dbService);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadSneakers();
            }
        }

        private void BtnEditSneaker_Click(object sender, EventArgs e)
        {
            if (sneakersGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите кроссовки для редактирования", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedSneaker = sneakersGrid.SelectedRows[0].DataBoundItem as SneakerViewModel;
            if (selectedSneaker != null)
            {
                var sneaker = _dbService.GetSneakerById(selectedSneaker.SneakerId);
                if (sneaker != null)
                {
                    var form = new SneakerForm(_dbService, sneaker);
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadSneakers();
                    }
                }
            }
        }

        private void BtnDeleteSneaker_Click(object sender, EventArgs e)
        {
            if (sneakersGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите кроссовки для удаления", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedSneaker = sneakersGrid.SelectedRows[0].DataBoundItem as SneakerViewModel;
            if (selectedSneaker != null)
            {
                var result = MessageBox.Show($"Удалить кроссовки '{selectedSneaker.Name}'?", "Подтверждение", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        if (_dbService.DeleteSneaker(selectedSneaker.SneakerId))
                        {
                            MessageBox.Show("Кроссовки удалены", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadSneakers();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        #endregion

        #region Brands Tab

        private Button btnAddBrand;

        private void InitializeBrandsTab(TabPage tab)
        {
            brandsGrid = new DataGridView();
            brandsGrid.Dock = DockStyle.Top;
            brandsGrid.Height = 400;
            brandsGrid.AutoGenerateColumns = false;
            brandsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            brandsGrid.ReadOnly = true;

            brandsGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "BrandId", HeaderText = "ID", Width = 50 });
            brandsGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Название", Width = 150 });
            brandsGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Country", HeaderText = "Страна", Width = 100 });
            brandsGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Description", HeaderText = "Описание", Width = 200 });

            tab.Controls.Add(brandsGrid);

            var panel = new Panel();
            panel.Dock = DockStyle.Bottom;
            panel.Height = 50;

            btnAddBrand = new Button { Text = "Добавить бренд", Location = new Point(10, 10), Width = 120 };
            btnAddBrand.Click += BtnAddBrand_Click;

            panel.Controls.Add(btnAddBrand);
            tab.Controls.Add(panel);
        }

        private void LoadBrands()
        {
            try
            {
                var brands = _dbService.GetAllBrands();
                brandsGrid.DataSource = brands;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки брендов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddBrand_Click(object sender, EventArgs e)
        {
            var form = new BrandForm(_dbService);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadBrands();
            }
        }

        #endregion

        #region Categories Tab

        private Button btnAddCategory;

        private void InitializeCategoriesTab(TabPage tab)
        {
            categoriesGrid = new DataGridView();
            categoriesGrid.Dock = DockStyle.Top;
            categoriesGrid.Height = 400;
            categoriesGrid.AutoGenerateColumns = false;
            categoriesGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            categoriesGrid.ReadOnly = true;

            categoriesGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CategoryId", HeaderText = "ID", Width = 50 });
            categoriesGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Название", Width = 150 });
            categoriesGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Description", HeaderText = "Описание", Width = 250 });

            tab.Controls.Add(categoriesGrid);

            var panel = new Panel();
            panel.Dock = DockStyle.Bottom;
            panel.Height = 50;

            btnAddCategory = new Button { Text = "Добавить категорию", Location = new Point(10, 10), Width = 140 };
            btnAddCategory.Click += BtnAddCategory_Click;

            panel.Controls.Add(btnAddCategory);
            tab.Controls.Add(panel);
        }

        private void LoadCategories()
        {
            try
            {
                var categories = _dbService.GetAllCategories();
                categoriesGrid.DataSource = categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddCategory_Click(object sender, EventArgs e)
        {
            var form = new CategoryForm(_dbService);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadCategories();
            }
        }

        #endregion

        #region Customers Tab

        private Button btnAddCustomer, btnEditCustomer, btnDeleteCustomer;

        private void InitializeCustomersTab(TabPage tab)
        {
            customersGrid = new DataGridView();
            customersGrid.Dock = DockStyle.Top;
            customersGrid.Height = 400;
            customersGrid.AutoGenerateColumns = false;
            customersGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            customersGrid.ReadOnly = true;

            customersGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CustomerId", HeaderText = "ID", Width = 50 });
            customersGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FirstName", HeaderText = "Имя", Width = 100 });
            customersGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "LastName", HeaderText = "Фамилия", Width = 100 });
            customersGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "Email", Width = 150 });
            customersGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Phone", HeaderText = "Телефон", Width = 120 });
            customersGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Address", HeaderText = "Адрес", Width = 200 });

            tab.Controls.Add(customersGrid);

            var panel = new Panel();
            panel.Dock = DockStyle.Bottom;
            panel.Height = 50;

            btnAddCustomer = new Button { Text = "Добавить", Location = new Point(10, 10), Width = 100 };
            btnEditCustomer = new Button { Text = "Редактировать", Location = new Point(120, 10), Width = 100 };
            btnDeleteCustomer = new Button { Text = "Удалить", Location = new Point(230, 10), Width = 100 };

            btnAddCustomer.Click += BtnAddCustomer_Click;
            btnEditCustomer.Click += BtnEditCustomer_Click;
            btnDeleteCustomer.Click += BtnDeleteCustomer_Click;

            panel.Controls.AddRange(new Control[] { btnAddCustomer, btnEditCustomer, btnDeleteCustomer });
            tab.Controls.Add(panel);
        }

        private void LoadCustomers()
        {
            try
            {
                var customers = _dbService.GetAllCustomers();
                customersGrid.DataSource = customers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки клиентов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddCustomer_Click(object sender, EventArgs e)
        {
            var form = new CustomerForm(_dbService);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadCustomers();
            }
        }

        private void BtnEditCustomer_Click(object sender, EventArgs e)
        {
            if (customersGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите клиента для редактирования", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var customer = customersGrid.SelectedRows[0].DataBoundItem as Models.Customer;
            if (customer != null)
            {
                var form = new CustomerForm(_dbService, customer);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadCustomers();
                }
            }
        }

        private void BtnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (customersGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите клиента для удаления", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var customer = customersGrid.SelectedRows[0].DataBoundItem as Models.Customer;
            if (customer != null)
            {
                var result = MessageBox.Show($"Удалить клиента '{customer.FirstName} {customer.LastName}'?", "Подтверждение", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        if (_dbService.DeleteCustomer(customer.CustomerId))
                        {
                            MessageBox.Show("Клиент удален", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadCustomers();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        #endregion
    }
}