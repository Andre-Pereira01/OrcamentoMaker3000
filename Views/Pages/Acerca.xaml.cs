using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace OrcamentoMaker3000.Views.Pages
{
    public partial class Acerca : Page
    {
        public Acerca()
        {
            InitializeComponent();
        }
        private void AbrirFacebook(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo("https://www.facebook.com/moncaobrass") { UseShellExecute = true });
        }

        private void AbrirInstagram(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo("https://www.instagram.com/moncaobrass") { UseShellExecute = true });
        }

        private void AbrirTwitter(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo("https://www.x.com/moncaobrass") { UseShellExecute = true });
        }

        private void AbrirYoutube(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo("https://www.youtube.com/@moncaobrass") { UseShellExecute = true });
        }

        private void AbrirGmail(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo("mailto:moncaobrass@gmail.com") { UseShellExecute = true });
        }

        private void AbrirTikTok(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo("https://www.tiktok.com/@moncaobrassoficial") { UseShellExecute = true });
        }

        private void AbrirSpotify(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo("https://open.spotify.com/intl-pt/artist/1At7nWiQx5USSxBuKEGCoM?si=WIY4GZjvT_q6u8beY4xTGA") { UseShellExecute = true });
        }

    }
}
