// Products Tab
private DataGridView productsGrid;
private Button btnAddProduct, btnEditProduct, btnDeleteProduct;

private void InitializeProductsTab(TabPage tab)
{
    // DataGridView для отображения продуктов
    productsGrid = new DataGridView();
    productsGrid.Dock = DockStyle.Top;
    productsGrid.Height = 400;
    productsGrid.AutoGenerateColumns = false;
    productsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
    productsGrid.ReadOnly = true;

    // Колонки для DataGridView
    productsGrid.Columns.Add(new DataGridViewTextBoxColumn 
    { 
        DataPropertyName = "ProductId", 
        HeaderText = "ID", 
        Width = 50 
    });
    productsGrid.Columns.Add(new DataGridViewTextBoxColumn 
    { 
        DataPropertyName = "Name", 
        HeaderText = "Название", 
        Width = 150 
    });
    productsGrid.Columns.Add(new DataGridViewTextBoxColumn 
    { 
        DataPropertyName = "Price", 
        HeaderText = "Цена", 
        Width = 80 
    });
    productsGrid.Columns.Add(new DataGridViewTextBoxColumn 
    { 
        DataPropertyName = "StockQuantity", 
        HeaderText = "Количество", 
        Width = 80 
    });
    productsGrid.Columns.Add(new DataGridViewTextBoxColumn 
    { 
        DataPropertyName = "CategoryName", 
        HeaderText = "Категория", 
        Width = 120 
    });

    tab.Controls.Add(productsGrid);

    // Панель с кнопками
    var panel = new Panel();
    panel.Dock = DockStyle.Bottom;
    panel.Height = 50;

    btnAddProduct = new Button { Text = "Добавить", Location = new System.Drawing.Point(10, 10), Width = 80 };
    btnEditProduct = new Button { Text = "Редактировать", Location = new System.Drawing.Point(100, 10), Width = 100 };
    btnDeleteProduct = new Button { Text = "Удалить", Location = new System.Drawing.Point(210, 10), Width = 80 };

    btnAddProduct.Click += BtnAddProduct_Click;
    btnEditProduct.Click += BtnEditProduct_Click;
    btnDeleteProduct.Click += BtnDeleteProduct_Click;

    panel.Controls.AddRange(new Control[] { btnAddProduct, btnEditProduct, btnDeleteProduct });
    tab.Controls.Add(panel);
}

private void LoadProducts()
{
    var products = _dbService.GetProducts();
    productsGrid.DataSource = products;
}

private void BtnAddProduct_Click(object sender, EventArgs e)
{
    var form = new ProductForm();
    if (form.ShowDialog() == DialogResult.OK)
    {
        _dbService.AddProduct(form.Product);
        LoadProducts();
    }
}

private void BtnEditProduct_Click(object sender, EventArgs e)
{
    if (productsGrid.SelectedRows.Count == 0)
    {
        MessageBox.Show("Выберите продукт для редактирования");
        return;
    }

    var selectedProduct = productsGrid.SelectedRows[0].DataBoundItem as ProductViewModel;
    var product = new Product
    {
        ProductId = selectedProduct.ProductId,
        Name = selectedProduct.Name,
        Description = selectedProduct.Description,
        Price = selectedProduct.Price,
        StockQuantity = selectedProduct.StockQuantity,
        CategoryId = _dbService.GetCategories()
            .First(c => c.Name == selectedProduct.CategoryName).CategoryId
    };

    var form = new ProductForm(product);
    if (form.ShowDialog() == DialogResult.OK)
    {
        _dbService.UpdateProduct(form.Product);
        LoadProducts();
    }
}

private void BtnDeleteProduct_Click(object sender, EventArgs e)
{
    if (productsGrid.SelectedRows.Count == 0)
    {
        MessageBox.Show("Выберите продукт для удаления");
        return;
    }

    var selectedProduct = productsGrid.SelectedRows[0].DataBoundItem as ProductViewModel;
    
    if (MessageBox.Show($"Удалить продукт '{selectedProduct.Name}'?", "Подтверждение", 
        MessageBoxButtons.YesNo) == DialogResult.Yes)
    {
        _dbService.DeleteProduct(selectedProduct.ProductId);
        LoadProducts();
    }
}