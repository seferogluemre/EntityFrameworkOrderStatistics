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



        }
    }
}
