using Godot;
using System;
using System.ComponentModel.Design;

namespace Game.Component
{

	public partial class HealthComponent : Node
	{
		[Signal] public delegate void HealthChangedEventHandler(int diff);
		[Signal]public delegate void MaxHealthChangedEventHandler(int diff);
		[Signal] public delegate void HealthDepletedEventHandler();
		[Export] private int MaxHealth = 3;
		[Export] private bool Immortal = false;
		private Timer immortalityTimer;
		private int Health;

		
		public bool IsImmortal()
		{
			return Immortal;
		}
		public void SetTemporaryImmortalityTimer(float time)
		{
			if (immortalityTimer == null)
			{
				immortalityTimer = new Timer();
				immortalityTimer.OneShot = true;
				AddChild(immortalityTimer);
			}
			if (immortalityTimer.IsConnected(nameof(immortalityTimer.Timeout), Callable.From(() => SetImortality(false))))
			{
				immortalityTimer.Timeout -= () => SetImortality(false);
			}
			{

			}
			immortalityTimer.Timeout += () => SetImortality(false) ;
			immortalityTimer.WaitTime = time;
			Immortal = true;
			immortalityTimer.Start();

		}

		public  void SetImortality(bool isImmortal)
		{
			Immortal = isImmortal;

        }

        public void SetMaxHealth(int maxHealth)
		{
			int clamped;
			if (maxHealth <= 0)
			{
				clamped = 1;
			}
			else
			{
				clamped = maxHealth;
			}
			if (clamped != MaxHealth)
			{
				EmitSignal(SignalName.MaxHealthChanged, clamped - MaxHealth);
			}
			MaxHealth = clamped;

			if (Health > MaxHealth)
			{
				Health = MaxHealth;
			}
		}
		
		public void SetHealth(int health)
		{
			if (health < Health && Immortal)
			{
				return;
			}

			int clamped = Mathf.Clamp(health,0,MaxHealth);
			Health = clamped;
			if (clamped != health)
			{
				EmitSignal(SignalName.HealthChanged, clamped - health);
			}
			if (Health <= 0)
			{
				EmitSignal(SignalName.HealthDepleted);
			}
			
		}
		public int GetHealth()
		{
			return Health;
		}

		public int GetMaxHealth()
		{
			return MaxHealth;
		}
		public override void _Ready()
		{
			Health = MaxHealth;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		
	}
}
