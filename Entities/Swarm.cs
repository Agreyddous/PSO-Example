using System;
using System.Collections;
using System.Collections.Generic;

namespace PSO.Entities
{
	public class Swarm : IList<Particle>
	{
		private List<Particle> _particles;

		public Swarm(Func<Particle, double> fitnessFunction,
			   Func<double, double, bool> compareFitnessFunction,
			   int size,
			   int dimentions,
			   double[] optimalFitnessRange,
			   double maximumValue,
			   double minimumValue,
			   int lifetime,
			   double weight,
			   double particleWeight,
			   double globalWeight)
		{
			_particles = new List<Particle>(size);

			CompareFitnessFunction = compareFitnessFunction;
			OptimalFitnessRange = optimalFitnessRange;
			MaximumValue = maximumValue;
			MinimumValue = minimumValue;
			Lifetime = lifetime;
			Weight = weight;
			GlobalWeight = globalWeight;

			BestFitness = double.MaxValue;
			BestPosition = new double[dimentions];

			for (int index = 0; index < size; index++)
			{
				_particles.Add(new Particle(fitnessFunction,
								compareFitnessFunction,
								dimentions,
								MaximumValue,
								MinimumValue,
								particleWeight));

				if (_particles[index].Fitness < BestFitness)
				{
					BestFitness = _particles[index].Fitness;
					_particles[index].Position.CopyTo(BestPosition, 0);
				}
			}
		}

		public Func<double, double, bool> CompareFitnessFunction { get; private set; }
		public double[] OptimalFitnessRange { get; private set; }
		public double BestFitness { get; private set; }
		public double[] BestPosition { get; private set; }
		public double MaximumValue { get; private set; }
		public double MinimumValue { get; private set; }
		public int Lifetime { get; private set; }
		public double Weight { get; private set; }
		public double GlobalWeight { get; private set; }

		public bool OptimalFitnessFound { get => BestFitness > OptimalFitnessRange[0] && BestFitness < OptimalFitnessRange[1]; }

		public int Process()
		{
			int iteration = 0;

			for (iteration = 0; iteration < Lifetime && !OptimalFitnessFound; iteration++)
				for (int index = 0; index < _particles.Count && !OptimalFitnessFound; index++)
					_particles[index].Run(this);

			return iteration;
		}

		public void UpdateBest(double[] position, double fitness)
		{
			if (CompareFitnessFunction(fitness, BestFitness))
			{
				position.CopyTo(BestPosition, 0);
				BestFitness = fitness;
			}
		}

		public Particle this[int index] { get => _particles[index]; set => _particles[index] = value; }

		public int Count => _particles.Count;

		public bool IsReadOnly => true;

		public void Add(Particle item) => _particles.Add(item);

		public void Clear() => _particles.Clear();

		public bool Contains(Particle item) => _particles.Contains(item);

		public void CopyTo(Particle[] array, int arrayIndex) => _particles.CopyTo(array, arrayIndex);

		public IEnumerator<Particle> GetEnumerator() => _particles.GetEnumerator();

		public int IndexOf(Particle item) => _particles.IndexOf(item);

		public void Insert(int index, Particle item) => _particles.Insert(index, item);

		public bool Remove(Particle item) => _particles.Remove(item);

		public void RemoveAt(int index) => _particles.RemoveAt(index);

		IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
	}
}