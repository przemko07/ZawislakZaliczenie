using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WpfFrontend.Model
{
    public class SettingsXml
    {
        [XmlElement("MatrixF1")]
        public string TxtF1 { get; set; }

        [XmlElement("MatrixF2")]
        public string TxtF2 { get; set; }

        [XmlIgnore]
        public Matrix F1
        {
            get { return ConvertMatrix(TxtF1); }
            set { TxtF1 = ConvertMatrix(value); }
        }

        [XmlIgnore]
        public Matrix F2
        {
            get { return ConvertMatrix(TxtF2); }
            set { TxtF2 = ConvertMatrix(value); }
        }

        public uint PopSize { get; set; }

        [XmlElement("Individuals")]
        public string[] TxtIndividuals { get; set; }

        [XmlIgnore]
        public Individual[] Individuals
        {
            get { return ConvertIndividuals(TxtIndividuals); }
            set { TxtIndividuals = ConvertIndividuals(value); }
        }


        private string ConvertMatrix(Matrix m)
        {
            if (m.Cols == 0 && m.Rows == 0) return string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            string firstdigitFormat = "{0:000}";
            string digitFormat = ", {0:000}";
            for (uint row = 0; row < m.Rows; row++)
            {
                sb.AppendFormat(firstdigitFormat, m[row, 0]);
                for (uint col = 1; col < m.Cols; col++)
                {
                    sb.AppendFormat(digitFormat, m[row, col]);
                }
                sb.AppendLine();
            }
            sb.AppendLine();

            return sb.ToString();
        }

        private Matrix ConvertMatrix(string matrixF1)
        {
            Regex rowsR = new Regex(".*(,).*\n");
            var rowC = rowsR.Matches(matrixF1);
            uint rows = (uint)rowC.Count;
            uint cols = rows;

            Matrix result = new Matrix(rows, cols);

            Regex colsR = new Regex("(\\d+)");
            for (uint row = 0; row < rowC.Count; row++)
            {
                var colC = colsR.Matches(rowC[(int)row].Value);
                if (colC.Count != cols) throw new Exception("Cannot use non square matricies.");
                for (uint col = 0; col < colC.Count; col++)
                {
                    result[row, col] = Int32.Parse(colC[(int)col].Value);
                }
            }

            return result;
        }

        private Individual[] ConvertIndividuals(string[] individuals)
        {
            Regex regex = new Regex(@"(\d+)");
            Individual[] result = new Individual[individuals.Length];
            for (int i = 0; i < individuals.Length; i++)
            {
                var g = regex.Matches(individuals[i]);
                result[i] = Individual.IndividualOfLength((uint)g.Count);
                for (uint j = 0; j < g.Count; j++)
                {
                    result[i][j] = UInt32.Parse(g[(int)j].Value);
                }
            }
            return result;
        }

        private string[] ConvertIndividuals(Individual[] individuals)
        {
            string[] result = new string[individuals.Length];
            for (int i = 0; i < individuals.Length; i++)
            {
                result[i] = individuals[i].ToString();
            }
            return result;
        }

        public static SettingsXml Load(string filepath)
        {
            XmlSerializer xml = new XmlSerializer(typeof(SettingsXml));
            using (Stream stream = File.OpenRead(filepath))
            {
                return xml.Deserialize(stream) as SettingsXml;
            }
        }

        public static void Write(SettingsXml o, string filepath)
        {
            XmlSerializer xml = new XmlSerializer(typeof(SettingsXml));
            using (Stream stream = File.Create(filepath))
            {
                xml.Serialize(stream, o);
            }
        }
    }
}
