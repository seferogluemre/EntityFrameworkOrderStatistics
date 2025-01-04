using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
            lblProductAveragePrice.Text = averageProductPrice.ToString() + " TL";

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

            //Türkiyeden yapılan siparişler
            var OrderCountFromTurkiye = istatisticEntities.Database.SqlQuery<int>("Select count(*) from TblOrder where CustomerId In (select CustomerId from TblCustomer where CustomerCountry='Türkiye')").FirstOrDefault();
            lblOrderCountFromTurkiye.Text = OrderCountFromTurkiye.ToString();

            //Türkiyeden yapılan siparişler EF METHODU İle
            var turkishCustomerIds = istatisticEntities.TblCustomer.Where(x => x.CustomerCountry == "Türkiye").
                Select(y => y.CustomerId).
                ToList();

            var orderCountFromTurkiyeWithEF = istatisticEntities.TblOrder.Count(z => turkishCustomerIds.Contains(z.CustomerId.Value));
            lblOrderCountFromTurkiyeEF.Text = orderCountFromTurkiyeWithEF.ToString();


            //Siparişler içinde kategorisi meyve olan ürünlerin toplam satış fiyatı SQL İLE
            try
            {
                using (var context = new DbIstatisticEntities())
                {
                    // SQL sorgusunu çalıştır ve sonucu al
                    var result = context.Database.SqlQuery<decimal?>(
                        "SELECT SUM(o.TotalPrice) as ToplamPrice " +
                        "FROM TblOrder o " +
                        "JOIN TblProduct p ON o.ProductId = p.ProductId " +
                        "JOIN TblCategory c ON p.CategoryId = c.CategoryId " +
                        "WHERE c.CategoryName = 'Meyve'").FirstOrDefault();

                    // Eğer sonuç null ise 0 olarak göster, değilse sonucu formatla
                    lblOrderTotalPriceByCategoryIsFruit.Text =
                        (result ?? 0).ToString("N2") + " TL";
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama veya kullanıcıya bilgi verme
                lblOrderTotalPriceByCategoryIsFruit.Text = "Hata: " + ex.Message;
            }

            // //Siparişler içinde kategorisi meyve olan ürünlerin toplam satış fiyatı EF İLE
            var OrderTotalPriceByCategoryIsFruitEf = (from o in istatisticEntities.TblOrder
                                                      join p in istatisticEntities.TblProduct on o.ProductId equals p.ProductId
                                                      join c in istatisticEntities.TblCategory on p.CategoryId equals c.CategoryId
                                                      where c.CategoryName == "Meyve"
                                                      select o.TotalPrice
                                                    ).Sum();

            lblOrderTotalPriceByCategoryIsFruitEF.Text = OrderTotalPriceByCategoryIsFruitEf.ToString() + " TL";

            //En Son eklenen ürünün adı
            var LastProductName = istatisticEntities.TblProduct.OrderByDescending(x => x.ProductId).Select(y => y.ProductName).FirstOrDefault();
            lblLastProductName.Text = LastProductName.ToString();


            //En son eklenen ürünün kategorisi
            var LastProductCategoryId = istatisticEntities.TblProduct.OrderByDescending(x => x.ProductId).Select(y => y.CategoryId).FirstOrDefault();
            var LastProductCategoryName = istatisticEntities.TblCategory.Where(x => x.CategoryId == LastProductCategoryId).Select(y => y.CategoryName).FirstOrDefault();
            lblLastProductCategoryName.Text = LastProductCategoryName.ToString();


            //Aktif ürün sayısı
            var activeProductCount = istatisticEntities.TblProduct.Where(x => x.ProductStatus == true).Count();
            lblActiveProductCount.Text = activeProductCount.ToString();


            //Toplam ayran stok satışlarından kazanılan miktar
            var ayranStock = istatisticEntities.TblProduct.Where(x => x.ProductName == "Ayran").Select(y2 => y2.ProductStock).FirstOrDefault();
            var ayranPrice = istatisticEntities.TblProduct.Where(x => x.ProductName == "Ayran").Select(y => y.ProductPrice).FirstOrDefault();
            var totalAyranStockPrice = ayranStock * ayranPrice;
            lblTotalPriceStockByAyran.Text = totalAyranStockPrice + " ₺";


            //Sistemde son sipariş veren  müşterinin adı
            var lastCustomerID = istatisticEntities.TblOrder.OrderByDescending(x => x.OrderId).Select(y => y.CustomerId).FirstOrDefault();
            var lastCustomerName = istatisticEntities.TblCustomer.Where(x => x.CustomerId == lastCustomerID).Select(y => y.CustomerName).FirstOrDefault();
            lblLastCustomerName.Text = lastCustomerName.ToString();


            //Ülke çeşitliligi sayısı
            var countryDifferentCount = istatisticEntities.TblCustomer.Select(x => x.CustomerCountry).Distinct().Count();
            lblCountryDifferent.Text = countryDifferentCount.ToString() +" Farklı ülke vardır.";

        }
    }
}
