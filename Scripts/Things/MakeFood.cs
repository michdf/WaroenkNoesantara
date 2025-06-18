using Godot;
using System;

public partial class MakeFood : Area3D
{
	private AnimationPlayer animationPlayer;
	private bool playerInRange = false;
	private Node player;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		
		// Connect area signals
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (playerInRange && Input.IsActionJustPressed("interact")) 
		{
			GD.Print("Interact key pressed");
			OpenFoodPurchaseScene();
		}
	}
	
	private void OnBodyEntered(Node3D body)
	{
		GD.Print("Body entered: " + body.Name);
		if (body.Name == "Player") // Adjust player node name as needed
		{
			playerInRange = true;
			player = body;
		}

		// 	// Play wave animation
			// 	if (animationPlayer != null && animationPlayer.HasAnimation("wave"))
			// 	{
			// 		animationPlayer.Play("wave");
			// 	}
			// }
		}

	private void OnBodyExited(Node3D body)
	{
		GD.Print("Body exited: " + body.Name);
		if (body.Name == "Player")
		{
			playerInRange = false;
			player = null;
		}
	}
	
	private void OpenFoodPurchaseScene()
	{
		GD.Print("Opening food processing scene...");
		// Change to food purchase scene
		// GetTree().ChangeSceneToFile("res://scenes/FoodPurchase.tscn"); // Adjust path as needed
	}
}
