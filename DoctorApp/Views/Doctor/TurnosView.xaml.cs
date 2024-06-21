using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DoctorApp.Properties;
using DoctorApp.Services;
using DoctorApp.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DoctorApp.Views.Doctor
{
    /// <summary>
    /// Interaction logic for TurnosView.xaml
    /// </summary>
    public partial class TurnosView : Window
    {
        public TurnosView()
        {
            InitializeComponent();
            TurnoViewModel viewModel = new TurnoViewModel();
            this.DataContext = new TurnoViewModel() ;
        }

   
    }
}
