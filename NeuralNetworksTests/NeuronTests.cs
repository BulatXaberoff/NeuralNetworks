using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworks;
using System;
using System.Collections.Generic;
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
                var res = neuralNetwork.FeedForward(row).Output;
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