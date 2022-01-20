using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworks;

namespace MedicalSystem
{
    public class SystemController
    {
        public NeuralNetwork DataNetwork { get; set; }
        public NeuralNetwork ImageNetwork { get; set; }
        public SystemController()
        {
            var datatopology = new Topology(14, 1, 0.1, 7);
            DataNetwork = new NeuralNetwork(datatopology);

            Topology imagetopology = new Topology(400,1,0.1,200);
            ImageNetwork = new NeuralNetwork(imagetopology);
        }
    }
}
