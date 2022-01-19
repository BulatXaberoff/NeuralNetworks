using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Tests
{
    [TestClass()]
    public class NeuronTests
    {
        [TestMethod()]
        public void FeedForwardTest()
        {
            //var dataset = new List<Tuple<double, double[]>>
            //{
            //    // Результат - Пациент болен - 1
            //    //             Пациент Здоров - 0

            //    // Неправильная температура T
            //    // Хороший возраст A
            //    // Курит S
            //    // Правильно питается F
            //    //                                             T  A  S  F
            //    new Tuple<double, double[]> (0, new double[] { 0, 0, 0, 0 }),
            //    new Tuple<double, double[]> (0, new double[] { 0, 0, 0, 1 }),
            //    new Tuple<double, double[]> (1, new double[] { 0, 0, 1, 0 }),
            //    new Tuple<double, double[]> (0, new double[] { 0, 0, 1, 1 }),
            //    new Tuple<double, double[]> (0, new double[] { 0, 1, 0, 0 }),
            //    new Tuple<double, double[]> (0, new double[] { 0, 1, 0, 1 }),
            //    new Tuple<double, double[]> (1, new double[] { 0, 1, 1, 0 }),
            //    new Tuple<double, double[]> (0, new double[] { 0, 1, 1, 1 }),
            //    new Tuple<double, double[]> (1, new double[] { 1, 0, 0, 0 }),
            //    new Tuple<double, double[]> (1, new double[] { 1, 0, 0, 1 }),
            //    new Tuple<double, double[]> (1, new double[] { 1, 0, 1, 0 }),
            //    new Tuple<double, double[]> (0, new double[] { 1, 0, 1, 1 }),
            //    new Tuple<double, double[]> (1, new double[] { 1, 1, 0, 0 }),
            //    new Tuple<double, double[]> (0, new double[] { 1, 1, 0, 1 }),
            //    new Tuple<double, double[]> (1, new double[] { 1, 1, 1, 0 }),
            //    new Tuple<double, double[]> (1, new double[] { 1, 1, 1, 1 })
            //};
            var outputs = new double[] { 0, 0, 1, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1 };
            var inputs = new double[,]
            {
                // Результат - Пациент болен - 1
                //             Пациент Здоров - 0
                
                // Неправильная температура T
                // Хороший возраст A
                // Курит S
                // Правильно питается F
            // T  A  S  F
             { 0, 0, 0, 0 },
             { 0, 0, 0, 1 },
             { 0, 0, 1, 0 },
             { 0, 0, 1, 1 },
             { 0, 1, 0, 0 },
             { 0, 1, 0, 1 },
             { 0, 1, 1, 0 },
             { 0, 1, 1, 1 },
             { 1, 0, 0, 0 },
             { 1, 0, 0, 1 },
             { 1, 0, 1, 0 },
             { 1, 0, 1, 1 },
             { 1, 1, 0, 0 },
             { 1, 1, 0, 1 },
             { 1, 1, 1, 0 },
             { 1, 1, 1, 1 }
            };
            var topology = new Topology(4, 1,0.1,12);
            var neuralNetwork = new NeuralNetwork(topology);
            var difference = neuralNetwork.Learn(outputs,inputs, 100000);

            var results = new List<double>();
            for (int i = 0; i < outputs.Length; i++)
            {
                var row = NeuralNetwork.GetRow(inputs, i);
                var res = neuralNetwork.Predict(row).Output;
                results.Add(res);
            }
            for (int i = 0; i < results.Count; i++)
            {
                var expected = Math.Round(outputs[i], 2);
                var actual = Math.Round(results[i], 2);
                Assert.AreEqual(expected, actual);
            }
        }
        [TestMethod()]
        public void RecognizeImages()
        {
            var parasitizedPath = @"C:\Users\bulyn\Downloads\5\cell_images\Parasitized\";
            var unparasitizedPath = @"C:\Users\bulyn\Downloads\5\cell_images\Uninfected\";

            var converter = new PictureConverter();
            var testParasitizedImageInput = converter.Convert(@"C:\Users\bulyn\Source\Repos\BulatXaberoff\NeuralNetworks\NeuralNetworksTests\image\P1.png");
            var testUnparasitizedImageInput = converter.Convert(@"C:\Users\bulyn\Source\Repos\BulatXaberoff\NeuralNetworks\NeuralNetworksTests\image\UnP.png");

            var topology = new Topology(testParasitizedImageInput.Count, 1, 0.1, testParasitizedImageInput.Count / 2);
            var neuralNetwork = new NeuralNetwork(topology);
            
            double[,] parasitizedInputs = GetData(parasitizedPath, converter, testParasitizedImageInput,100);
            neuralNetwork.Learn(new double[] { 1 }, parasitizedInputs, 10);

            double[,] unparasitizedInputs = GetData(unparasitizedPath, converter, testParasitizedImageInput,100);
            neuralNetwork.Learn(new double[] { 0 }, unparasitizedInputs, 10);

            var par = neuralNetwork.Predict(testParasitizedImageInput.Select(t=>(double)t).ToArray());
            var unpar = neuralNetwork.Predict(testUnparasitizedImageInput.Select(t=>(double)t).ToArray());

            Assert.AreEqual(1, Math.Round(par.Output, 2));
            Assert.AreEqual(0, Math.Round(unpar.Output, 2));
        }

        private static double[,] GetData(string parasitizedPath, PictureConverter converter, List<int> testImageInput,int size)
        {
            var parasitizedImages = Directory.GetFiles(parasitizedPath);
            var parasitizedInputs = new double[size,testImageInput.Count];
            for (int i = 0; i < size; i++)
            {
                var image = converter.Convert(parasitizedImages[i]);
                for (int j = 0; j < image.Count; j++)
                {
                    parasitizedInputs[i,j] = image[j];
                }
            }

            return parasitizedInputs;
        }

        [TestMethod()]
        public void DatasetTest()
        {
            var outputs = new List<double>();
            var inputs = new List<double[]>();
            using (var sr = new StreamReader(@"C:\Users\bulyn\Source\Repos\BulatXaberoff\NeuralNetworks\NeuralNetworksTests\heart.csv"))
            {
                var header = sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    var row = sr.ReadLine();
                    var values = row.Split(',').Select(v=>Convert.ToDouble(v.Replace(".",","))).ToList();
                    var output = values.Last();
                    var input = values.Take(values.Count - 1).ToArray();

                    outputs.Add(output);
                    inputs.Add(input);
                }
            }

            var inputSignals = new double[inputs.Count, inputs[0].Length];
            for (int i = 0; i < inputSignals.GetLength(0); i++)
            {
                for (int j = 0; j < inputSignals.GetLength(1); j++)
                {
                    inputSignals[i, j] = inputs[i][j];
                }
            }

            var topology = new Topology(outputs.Count, 1, 1, outputs.Count/2);
            var neuralNetwork = new NeuralNetwork(topology);
            var difference = neuralNetwork.Learn(outputs.ToArray(), inputSignals, 100);

            var results = new List<double>();
            for (int i = 0; i < outputs.Count; i++)
            {
                var res = neuralNetwork.Predict(inputs[i]).Output;
                results.Add(res);
            }
            for (int i = 0; i < results.Count; i++)
            {
                var expected = Math.Round(outputs[i], 2);
                var actual = Math.Round(results[i], 2);
                Assert.AreEqual(expected, actual);
            }
        }

    }
}