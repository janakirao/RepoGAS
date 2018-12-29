using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GASLibrary;
namespace WpfApp8
{
    /// <summary>
    /// Interaction logic for Products.xaml
    /// </summary>
    public partial class Products : Page
    {

        GASDBEntities db = new GASDBEntities("metadata=res://*/GASModel.csdl|res://*/GASModel.ssdl|res://*/GASModel.msl;provider=System.Data.SqlClient;provider connection string='data source=192.168.178.48;initial catalog=GASDB;user id=GASuser;password= winter2018;MultipleActiveResultSets=True;App=EntityFramework'");
        List<Purpose> Pro = new List<Purpose>();

        public Products()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PurposeList.ItemsSource = Pro;

            foreach (var item in db.Purposes)
            {
                Pro.Add(item);

            }

        }
    }
}
