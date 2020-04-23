using System;

namespace PSO.Entities
{
	public class Particle
	{
		public Particle(Func<Particle, double> fitnessFunction,
				  int dimentions,
				  double maximumValue,
				  double minimumValue,
				  double weight)
		{
			Random random = new Random();

			FitnessFunction = fitnessFunction;
			Weight = weight;

			Position = new double[dimentions];
			Velocity = new double[dimentions];
			BestFitness = double.MaxValue;
			BestPosition = new double[dimentions];

			for (int index = 0; index < dimentions; index++)
			{
				Position[index] = (maximumValue - minimumValue) * random.NextDouble() + minimumValue;

				double absolute = Math.Abs(maximumValue - minimumValue);
				Velocity[index] = (absolute * 2) * random.NextDouble() - absolute;
			}

			_updateFitness();
		}

		public Func<Particle, double> FitnessFunction { get; private set; }
		public double[] Position { get; private set; }
		public double[] Velocity { get; private set; }
		public double Fitness { get; private set; }
		public double[] BestPosition { get; private set; }
		public double BestFitness { get; private set; }
		public double Weight { get; private set; }

		public void Run(Swarm swarm)
		{
			_updateVelocity(swarm);
			_updatePosition(swarm);

			if (_updateFitness())
				swarm.UpdateBest(Position, Fitness);
		}

		private void _updateVelocity(Swarm swarm)
		{
			Random random = new Random(0);

			for (int index = 0; index < Velocity.Length; index++)
			{
				double localRandomization = random.NextDouble();
				double globalRandomization = random.NextDouble();

				Velocity[index] = (swarm.Weight * Velocity[index]) +
				  (Weight * localRandomization * (BestPosition[index] - Position[index])) +
				  (swarm.GlobalWeight * globalRandomization * (swarm.BestPosition[index] - Position[index]));

				if (Velocity[index] < -swarm.MaximumValue)
					Velocity[index] = -swarm.MaximumValue;

				else if (Velocity[index] > swarm.MaximumValue)
					Velocity[index] = swarm.MaximumValue;
			}
		}

		private void _updatePosition(Swarm swarm)
		{
			for (int index = 0; index < Position.Length; ++index)
			{
				Position[index] = Position[index] + Velocity[index];

				if (Position[index] < swarm.MinimumValue)
					Position[index] = swarm.MinimumValue;

				else if (Position[index] > swarm.MaximumValue)
					Position[index] = swarm.MaximumValue;
			}
		}

		private bool _updateFitness()
		{
			Fitness = FitnessFunction(this);

			bool updateBest = Fitness < BestFitness;

			if (updateBest)
			{
				BestFitness = Fitness;
				Position.CopyTo(BestPosition, 0);
			}

			return updateBest;
		}
	}
}