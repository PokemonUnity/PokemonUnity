using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pokemon_Unity_Database_Creator
{
    public partial class LoadingScreen : Form
    {
        public LoadingScreen()
        {
            InitializeComponent();
        }

        private void LoadingScreen_Load(object sender, EventArgs e)
        {

        }

        public List<PokemonData> ImportDatabase()
        {
            DatabaseImporter Import = new DatabaseImporter
            {
                Progress = loadingBar
            };
            this.Show();
            return Import.ImportDatabase();
        }
    }
}
