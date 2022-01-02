using Northwind.Business.Abstract;
using Northwind.Business.Concrete;
using Northwind.DataAccess.Concrete.EntityFramework;
using Northwind.DataAccess.Concrete.Nhibernate;
using Northwind.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Northwind.WebFormsUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
             _productService= new ProductManager(new EfProductDal());
            _categoryService = new CategoryManager(new EfCategoryDal());
        }
        
        private IProductService _productService;
        private ICategoryService _categoryService;

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadProducts();
            LoadCategories();
        }

        private void LoadCategories()
        {
            cbxCategory.DataSource = _categoryService.GetAll();
            cbxCategory.DisplayMember = "CategoryName";
            cbxCategory.ValueMember = "CategoryId";

            cbxCategoryId.DataSource = _categoryService.GetAll();
            cbxCategoryId.DisplayMember = "CategoryName";
            cbxCategoryId.ValueMember = "CategoryId";

            cbxProductNameUpdate.DataSource = _categoryService.GetAll();
            cbxProductNameUpdate.DisplayMember = "CategoryName";
            cbxProductNameUpdate.ValueMember = "CategoryId";

        }

        private void LoadProducts()
        {
            dgwProduct.DataSource = _productService.GetAll();
        }

        private void cbxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dgwProduct.DataSource = _productService.GetProductByCategory(Convert.ToInt32(cbxCategory.SelectedValue));

            }
            catch 
            {

              
            }
        }

        private void tbxProductName_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbxProductName.Text))
            {
                dgwProduct.DataSource = _productService.GetProductsByProductName(tbxProductName.Text);

            }
            else
            {
                LoadProducts();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _productService.Add(new Product
            {

                CategoryId = Convert.ToInt32(cbxCategoryId.SelectedValue),
                ProductName = tbxProductName2.Text,
                QuantityPerUnit =tbxQuantityPerUnit.Text,
                UnitPrice =Convert.ToDecimal(tbxUnitPrice.Text),
                UnitsInStock= Convert.ToInt16(tbxStockAmount.Text)
            });
            MessageBox.Show("Added!");
            LoadProducts();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            _productService.Update(new Product { 
                
                ProductId= Convert.ToInt32(dgwProduct.CurrentRow.Cells[0].Value),
                ProductName = tbxProductNameUpdate.Text,
                CategoryId = Convert.ToInt32(cbxProductNameUpdate.SelectedValue),
                QuantityPerUnit = tbxQuantityUpdate.Text,
                UnitPrice = Convert.ToDecimal(tbxUnitPriceUpdate.Text),
                UnitsInStock = Convert.ToInt16(tbxStockAmountUpdate.Text)

            });
            MessageBox.Show("Updated!");
            LoadProducts();
        }

        private void dgwProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = dgwProduct.CurrentRow;
            tbxProductNameUpdate.Text = dgwProduct.CurrentRow.Cells[1].Value.ToString();
            cbxProductNameUpdate.SelectedValue = row.Cells[2].Value;
            tbxUnitPriceUpdate.Text = row.Cells[3].Value.ToString();
            tbxQuantityUpdate.Text = row.Cells[4].Value.ToString();
            tbxStockAmountUpdate.Text = row.Cells[5].Value.ToString();

        
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgwProduct.CurrentRow != null)
                {
                    _productService.Delete(new Product { ProductId = Convert.ToInt32(dgwProduct.CurrentRow.Cells[0]) });

                    MessageBox.Show("Deleted!");
                    LoadProducts();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                
            }

        }

    }
}
