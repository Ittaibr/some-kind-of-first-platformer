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
		[Signal] public delegate void ReceivedDamageEventHandler(int damage);

		[Export] private int MaxHealth = 3;
		[Export] private bool Immortal = false;
		[Export] private CollisionShape2D collision;
		private Timer immortalityTimer;
		private int Health;

		
		public bool IsImmortal()
		{
			
			return Immortal;
		}

		private void SetImortalityTimeout()
		{
			SetImortality(false);
		}
		public void SetTemporaryImmortalityTimer(double time)
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

			immortalityTimer.WaitTime = time;
			immortalityTimer.Timeout += () => SetImortality(false);
			Immortal = true;
			immortalityTimer.Start();

		}

		public  void SetImortality(bool isImmortal)
		{
			Tween tween2 = CreateTween();
       	    tween2.TweenCallback(Callable.From(() => collision.SetDeferred("disabled", true))).SetDelay(0.01);
       	    tween2.TweenCallback(Callable.From(() => collision.SetDeferred("disabled", false))).SetDelay(0.01);
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
			{

				//EmitSignal(SignalName.HealthChanged, clamped - health);
				if (clamped < Health)
				{
					
					EmitSignal(SignalName.ReceivedDamage,  clamped - health);
				}

			}
			Health = clamped;

			
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
