using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EntityFrameworkIstatistics
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DbIstatisticEntities istatisticEntities = new DbIstatisticEntities();
        

        private void Form1_Load(object sender, EventArgs e)
        {
            //Toplam kategori sayısı
            int categoryCount = istatisticEntities.TblCategory.Count();
            lblCategoryCount.Text = categoryCount.ToString();

            //Toplam ürün sayısı
            int productCount = istatisticEntities.TblProduct.Count();
            lblProductCount.Text = productCount.ToString();

            //Toplam Müşteri sayısı
            int customerCount = istatisticEntities.TblCustomer.Count();
            lblCustomerCount.Text = customerCount.ToString();

            //Toplam sipariş sayısı
            int orderCount = istatisticEntities.TblOrder.Count();
            lblOrderCount.Text = orderCount.ToString();

            //Toplam Stok Sayısı
            var totalProductStockCount = istatisticEntities.TblProduct.Sum(x => x.ProductStock);
            lblProductTotalStock.Text = totalProductStockCount.ToString();

            //Ortalama ürün fiyatı
            var averageProductPrice = istatisticEntities.TblProduct.Average(x => x.ProductPrice);
            lblProductAveragePrice.Text = averageProductPrice.ToString()+" TL";

            //Toplam meyve stok sayısı
            var totalProductCountByCategoryIsFruit = istatisticEntities.TblProduct.Where(category => category.CategoryId == 1).Sum(stock => stock.ProductStock);
            lblProductCountByCategoryIsFruit.Text = totalProductCountByCategoryIsFruit.ToString();

            //Gazoz isimli ürünün toplam işlem hacmi
            var totalPriceByProductNameIsGazozGetStock = istatisticEntities.TblProduct.Where(x => x.ProductName == "Soda").Select(y => y.ProductStock).FirstOrDefault();
            var totalPriceByProductNameIsGazozGetUnitPrice = istatisticEntities.TblProduct.Where(x => x.ProductName == "Soda").Select(y => y.ProductPrice).FirstOrDefault();
            var totalPriceByProductNameIsGazoz = totalPriceByProductNameIsGazozGetStock * totalPriceByProductNameIsGazozGetUnitPrice;
            lblTotalPriceByProductNameIsGazoz.Text = totalPriceByProductNameIsGazoz.ToString() + " TL";

            //Stok Sayısı 100'den Az Ürünler
            var stockCountLessThanOneHundred = istatisticEntities.TblProduct.Where(x => x.ProductStock < 100).Count();
            lblStockCountLessThanOneHundred.Text = stockCountLessThanOneHundred.ToString();

            //Kategorisi sebze ve durumu aktif olan ürün stok toplamı
            int id = istatisticEntities.TblCategory.Where(x => x.CategoryName == "Sebze").Select(y => y.CategoryId).FirstOrDefault();
            var productStockCountByCategoryNameIsSebzeAndStatusIsTrue = istatisticEntities.TblProduct.Where(x => x.CategoryId == (istatisticEntities.TblCategory.Where(z => z.CategoryName == "Sebze").Select(y => y.CategoryId).FirstOrDefault()) && x.ProductStatus == true).Sum(y => y.ProductStock);
            lblProductCategorySebzeAndStatusTrue.Text = productStockCountByCategoryNameIsSebzeAndStatusIsTrue.ToString();


        }
    }
}
