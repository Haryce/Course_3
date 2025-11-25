// ProductForm.cs
public partial class ProductForm : Form
{
    public Product Product { get; private set; }
    private DatabaseService _dbService;
    
    private TextBox txtName, txtDescription, txtPrice, txtStockQuantity;
    private ComboBox cmbCategory;
    private Button btnSave, btnCancel;

    public ProductForm(Product product = null)
    {
        _dbService = new DatabaseService();
        Product = product ?? new Product();
        InitializeComponent();
        LoadCategories();
        
        if (product != null)
            LoadProductData();
    }

    private void InitializeComponent()
    {
        this.Text = Product.ProductId == 0 ? "Добавить продукт" : "Редактировать продукт";
        this.Size = new System.Drawing.Size(400, 350);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;

        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };

        // Название
        panel.Controls.Add(new Label { Text = "Название:", Location = new System.Drawing.Point(10, 20) });
        txtName = new TextBox { Location = new System.Drawing.Point(120, 17), Width = 200 };
        panel.Controls.Add(txtName);

        // Описание
        panel.Controls.Add(new Label { Text = "Описание:", Location = new System.Drawing.Point(10, 60) });
        txtDescription = new TextBox { Location = new System.Drawing.Point(120, 57), Width = 200, Height = 60 };
        txtDescription.Multiline = true;
        txtDescription.Height = 60;
        panel.Controls.Add(txtDescription);

        // Цена
        panel.Controls.Add(new Label { Text = "Цена:", Location = new System.Drawing.Point(10, 140) });
        txtPrice = new TextBox { Location = new System.Drawing.Point(120, 137), Width = 100 };
        panel.Controls.Add(txtPrice);

        // Количество
        panel.Controls.Add(new Label { Text = "Количество:", Location = new System.Drawing.Point(10, 180) });
        txtStockQuantity = new TextBox { Location = new System.Drawing.Point(120, 177), Width = 100 };
        panel.Controls.Add(txtStockQuantity);

        // Категория
        panel.Controls.Add(new Label { Text = "Категория:", Location = new System.Drawing.Point(10, 220) });
        cmbCategory = new ComboBox { Location = new System.Drawing.Point(120, 217), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        panel.Controls.Add(cmbCategory);

        // Кнопки
        btnSave = new Button { Text = "Сохранить", Location = new System.Drawing.Point(120, 270), Width = 80 };
        btnCancel = new Button { Text = "Отмена", Location = new System.Drawing.Point(210, 270), Width = 80 };

        btnSave.Click += BtnSave_Click;
        btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

        panel.Controls.Add(btnSave);
        panel.Controls.Add(btnCancel);

        this.Controls.Add(panel);
    }

    private void LoadCategories()
    {
        var categories = _dbService.GetCategories();
        cmbCategory.DataSource = categories;
        cmbCategory.DisplayMember = "Name";
        cmbCategory.ValueMember = "CategoryId";
    }

    private void LoadProductData()
    {
        txtName.Text = Product.Name;
        txtDescription.Text = Product.Description;
        txtPrice.Text = Product.Price.ToString();
        txtStockQuantity.Text = Product.StockQuantity.ToString();
        cmbCategory.SelectedValue = Product.CategoryId;
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
        if (ValidateForm())
        {
            Product.Name = txtName.Text;
            Product.Description = txtDescription.Text;
            Product.Price = decimal.Parse(txtPrice.Text);
            Product.StockQuantity = int.Parse(txtStockQuantity.Text);
            Product.CategoryId = (int)cmbCategory.SelectedValue;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }

    private bool ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MessageBox.Show("Введите название продукта");
            return false;
        }

        if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
        {
            MessageBox.Show("Введите корректную цену");
            return false;
        }

        if (!int.TryParse(txtStockQuantity.Text, out int quantity) || quantity < 0)
        {
            MessageBox.Show("Введите корректное количество");
            return false;
        }

        return true;
    }
}