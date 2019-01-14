using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Pokemon_Unity_Database_Creator
{
    public class PokemonData
    {
        //Pokemon Info
        public string Sprite { get; set; }
        public string Name { get; set; }
        public string PokedexID { get; set; }
        public string Type1 { get; set; }
        public string Type2 { get; set; }
        public string Ability1 { get; set; }
        public string Ability2 { get; set; }
        public string HiddenAbility { get; set; }

        //Egg Group
        public string EggGroup1 { get; set; }
        public string EggGroup2 { get; set; }

        //Ratio's And Value's
        public float MaleRatio { get; set; }
        public float CatchRate { get; set; }
        public int HatchTime { get; set; }
        public string LevelingRate { get; set; }

        //Pokedex Info
        public float Height { get; set; }
        public float Weight { get; set; }
        public string PokedexColor { get; set; }
        public string Species { get; set; }
        public string BaseFriendship { get; set; }
        public string PokedexEntry { get; set; }
        public string EvolutionID { get; set; }
        public string EvolutionLevel { get; set; }
        public string EvolutionMethod { get; set; }


        //Pokemon Unity Extra's
        public float Luminance { get; set; }
        public string LightColor { get; set; }

        //Base Stats
        public int BaseHP { get; set; }
        public int BaseAttack { get; set; }
        public int BaseDefense { get; set; }
        public int BaseSpecialAttack { get; set; }
        public int BaseSpecialDefense { get; set; }
        public int BaseSpeed { get; set; }

        //EV Stats
        public int EvHP { get; set; }
        public int EvAttack { get; set; }
        public int EvDefense { get; set; }
        public int EvSpecialAttack { get; set; }
        public int EvSpecialDefense { get; set; }
        public int EvSpeed { get; set; }
        public int EvExp { get; set; }

        //Level Moves
        public List<LevelMove> LevelMoves = new List<LevelMove>();

        //TM's and HM's
        public List<string> hmAndTM = new List<string>();

        public void CreateEmtpy()
        {
            Name = "";
            PokedexID = "0";
            Type1 = "";
            Type2 = "";
            Ability1 = "";
            Ability2 = "";
            HiddenAbility = "";
            EggGroup1 = "";
            EggGroup2 = "";
            LevelingRate = "";
            PokedexColor = "";
            PokedexEntry = "";
            Species = "";
            BaseFriendship = "0";
            EvolutionID = "";
            EvolutionLevel = "";
            EvolutionMethod = "";
            LightColor = "";
        }

        string ToUpperRemoveSpace(string Target)
        {
            Target = Target.ToUpper();
            Target = Target.Replace(" ", "");
            return Target;
        }

        string MoveLevels(List<LevelMove> Levels)
        {
            string Result = "";
            foreach(LevelMove Move in Levels)
            {
                Result = Result + Move.Level + ", ";
            }

            if(Result != "")
            {
                Result = Result.Remove(Result.Length - 1);
                Result = Result.Remove(Result.Length - 1);
            }

            return Result;
        }

        string MoveNames(List<LevelMove> Moves)
        {
            string Result = "";
            foreach(LevelMove Move in Moves)
            {
                Result = Result + "\"" + Move.Move + "\"" + ", ";
            }

            if (Result != "")
            {
                Result = Result.Remove(Result.Length - 1);
                Result = Result.Remove(Result.Length - 1);
            }

            return Result;
        }

        string HMandTM(List<string> Moves)
        {
            string Result = "";
            foreach(string Move in Moves)
            {
                if (Move != "")
                {
                    Result = Result + "\"" + Move + "\"" + ", ";
                }
            }

            if (Result != "")
            {
                Result = Result.Remove(Result.Length - 1);
                Result = Result.Remove(Result.Length - 1);
            }

            return Result;
        }

        public string AbilityCheck(string Ability)
        {
            string Result = Ability;
            if(Ability == "None")
            {
                Result = "null";
            }
            return Result;
        }

        public override string ToString()
        {
            return ($"new PokemonData({PokedexID}, \"{Name}\", PokemonData.Type.{Type1.ToUpper()}, PokemonData.Type.{Type2.ToUpper()}, \"{AbilityCheck(Ability1)}\", {AbilityCheck(Ability2)}, \"{AbilityCheck(HiddenAbility)}\",\n" +
                   $"{MaleRatio}f, {CatchRate}, PokemonData.EggGroup.{EggGroup1.ToUpper()}, PokemonData.EggGroup.{EggGroup2.ToUpper()}, {HatchTime}, {Height}f, {Weight}f,\n" +
                   $"{EvExp.ToString()}, PokemonData.LevelingRate.{ToUpperRemoveSpace(LevelingRate)}, {EvHP.ToString()}, {EvAttack.ToString()}, {EvDefense.ToString()}, {EvSpecialAttack.ToString()}, {EvSpecialDefense.ToString()}, {EvSpeed.ToString()}, PokemonData.PokedexColor.{ToUpperRemoveSpace(PokedexColor)}, {BaseFriendship.ToString()},\n" +
                   $"\"{Species}\", \"{PokedexEntry}\",\n" +
                   $"{BaseHP.ToString()}, {BaseAttack.ToString()}, {BaseDefense.ToString()}, {BaseSpecialAttack.ToString()}, {BaseSpecialDefense.ToString()}, {BaseSpeed.ToString()}, {Luminance.ToString()}f, Color.{LightColor.ToLower()}," +
                   "new int[] {" + $"{MoveLevels(LevelMoves)}" + "},\n" +
                   "new string[]\n" +
                   "{\n" +
                   $"{MoveNames(LevelMoves)}\n" +
                   "},\n" +
                   "new string[] { " + HMandTM(hmAndTM) + " },\n" +
                   "new int[] {" + EvolutionID + "}, new string[] {" + $"\"Level,{EvolutionLevel}\"" + "}),\n");
        }
    }
}
