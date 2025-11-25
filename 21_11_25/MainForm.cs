using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


public partial class MainForm : Form
{
    private DatabaseService _dbService;
    private TabControl tabControl;

    public MainForm()
    {
        _dbService = new DatabaseService();
        InitializeComponent();
        LoadData();
    }

    private void InitializeComponent()
    {
        this.Text = "Магазин - Управление";
        this.Size = new System.Drawing.Size(1000, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        tabControl = new TabControl();
        tabControl.Dock = DockStyle.Fill;
        this.Controls.Add(tabControl);

        // Вкладка продуктов
        var productsTab = new TabPage("Продукты");
        tabControl.TabPages.Add(productsTab);
        InitializeProductsTab(productsTab);

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
        LoadProducts();
        LoadCategories();
        LoadCustomers();
    }
}