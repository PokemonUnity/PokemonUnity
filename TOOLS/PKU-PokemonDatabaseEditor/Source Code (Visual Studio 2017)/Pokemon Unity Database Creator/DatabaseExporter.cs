using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Drawing.Imaging;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Pokemon_Unity_Database_Creator
{
    public class DatabaseExporter
    {
        public DatabaseExporter(List<PokemonData> Pokemons)
        {
            string PokemonDatabase = "";
            foreach (PokemonData Pokemon in Pokemons)
            {
                PokemonDatabase = PokemonDatabase + Pokemon.ToString() + "\n\n";
                if (PokemonDatabase != "")
                {
                    PokemonDatabase = PokemonDatabase.Remove(PokemonDatabase.Length - 1);
                }
            }
            string Experience = Pokemon_Unity_Database_Creator.Properties.Resources.Experience;
            string Output =
                "//Original Scripts by IIColour (IIColour_Spectrum)\n" +
                "//Made using the Pokemon Unity Database Creator (By Velorexe#8403)\n\n" +
                "using UnityEngine;\n" +
                "using System.Collections;\n\n" +
                "public static class PokemonDatabase\n" +
                "{\n" +
                "private static PokemonData[] pokedex = new PokemonData[]\n" +
                "{\n" +
                "null,\n" +
                $"{PokemonDatabase}\n" +
                "};\n\n" +
                $"{Experience}" +
                "}\n" +
                "}";

            SaveFileDialog save = new SaveFileDialog();
            save.FileName = "PokemonDatabase.cs";
            save.Filter = "CS File|*.cs";
            if (save.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(save.FileName, Output);
            }
        }
    }
}
