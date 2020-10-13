using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;

namespace MaxTemp
{
    enum ReturnValues
    {
        MaxTemperatur = 0,
        Date = 1,
        Time = 2,
        HighestTempLinePosition = 3,
        Messfühler = 4
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private string FormatDateToGermanTime(string OriginalDate)
        {
            DateTime parsedDate;
            if (DateTime.TryParseExact(OriginalDate, "yyyy-MM-dd", null, DateTimeStyles.None, out parsedDate))
                return parsedDate.ToShortDateString();
            else
                return OriginalDate;
        }
        private List<string> GetHighestTemperatur(string[] lines)
        {
            List<string> ReturnValuesLst = new List<string>();
            decimal MaxTemp = 0;
            string DateAndTimeString = String.Empty;
            int IterrationCounter = 1;
            int HighestTempLinePosition = 0;
            string Messfühler = String.Empty;
            foreach (string line in lines)
            {
                string[] csvData = line.Split(',');
                decimal temp = decimal.Parse(csvData[2], CultureInfo.InvariantCulture);
                if (temp > MaxTemp)
                {
                    MaxTemp = temp;
                    DateAndTimeString = csvData[1];
                    HighestTempLinePosition = IterrationCounter;
                    Messfühler = csvData[0];
                }
                IterrationCounter++;
            }
            string[] DateAndTimeArraySplit = DateAndTimeString.Split(' ');

            ReturnValuesLst.Add(MaxTemp.ToString());
            ReturnValuesLst.Add(DateAndTimeArraySplit[0]);
            ReturnValuesLst.Add(DateAndTimeArraySplit[1]);
            ReturnValuesLst.Add(HighestTempLinePosition.ToString());
            ReturnValuesLst.Add(Messfühler);
            return ReturnValuesLst;
        }
        private void BtnAuswerten_Click(object sender, RoutedEventArgs e)
        {
            string[] lines = File.ReadAllLines("temps.csv");
            List<string> MaxTempResult = GetHighestTemperatur(lines);
            lblAusgabe.Content = $"{MaxTempResult[(int)ReturnValues.MaxTemperatur]} Grad ist die höchste Temperatur am Messfühler: {MaxTempResult[(int)ReturnValues.Messfühler]}.{Environment.NewLine}Die am {FormatDateToGermanTime(MaxTempResult[(int)ReturnValues.Date])} um {MaxTempResult[(int)ReturnValues.Time]} Uhr in der Zeile: {MaxTempResult[(int)ReturnValues.HighestTempLinePosition]} gefunden wurde.";
        }
    }
}
