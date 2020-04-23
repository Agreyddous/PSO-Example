using System;
using System.Text.Json;
using PSO.Entities;

namespace PSO
{
	class Program
	{
		static void Main(string[] args)
		{
			int swarmSize = 10;
			int lifetime = 1000;
			int dimentions = 10;
			double minimumValue = -10000.0;
			double maximumValue = 10000.0;
			double[] optimalFitnessRange = new double[] { -2.999999999999999, 3.000000000000001 };

			double weight = 0.729;
			double particleWeight = 1.49445;
			double globalWeight = 1.49445;

			Console.Clear();
			Console.WriteLine("Creating Swarm...");

			Swarm swarm = new Swarm(particle => 3.0 +
					particle.Position[0] * particle.Position[0] +
					particle.Position[1] * particle.Position[1] +
					particle.Position[2] * particle.Position[2] +
					particle.Position[3] * particle.Position[3] +
					particle.Position[4] * particle.Position[4] +
					particle.Position[5] * particle.Position[5] +
					particle.Position[6] * particle.Position[6] +
					particle.Position[7] * particle.Position[7] +
					particle.Position[8] * particle.Position[8] +
					particle.Position[9] * particle.Position[9],
						(fitness, bestFitness) => fitness < bestFitness,
						swarmSize,
						dimentions,
						optimalFitnessRange,
						maximumValue,
						minimumValue,
						lifetime,
						weight,
						particleWeight,
						globalWeight);

			Console.WriteLine("Swarm created\n");

			Console.WriteLine("Starting process...");

			DateTime startedProcess = DateTime.Now;

			int iterations = swarm.Process();

			Console.WriteLine("Process finished\n");

			Console.WriteLine($"Executed {iterations} iterations in {DateTime.Now.Subtract(startedProcess).TotalMilliseconds}ms");
			Console.WriteLine($"Optimal fitness found: {swarm.BestFitness}");

			Console.WriteLine("\nPosition:");
			Console.WriteLine($"{JsonSerializer.Serialize(swarm.BestPosition)}");
		}

	}
}