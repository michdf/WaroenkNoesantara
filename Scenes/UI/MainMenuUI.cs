using Godot;

public partial class MainMenuUI : Control
{
    private Button playButton;
    private Button helpButton;
	private Button CreditButton;
    private Button quitButton;
    private Label titleLabel;
    
    public override void _Ready()
    {
        // Get node references
        playButton = GetNode<Button>("VBoxContainer/PlayButton");
        helpButton = GetNode<Button>("VBoxContainer/HelpButton");
		CreditButton = GetNode<Button>("VBoxContainer/CreditButton");
        quitButton = GetNode<Button>("VBoxContainer/QuitButton");
        titleLabel = GetNode<Label>("TitleLabel");
        
        // Connect signals
        playButton.Pressed += OnPlayPressed;
        helpButton.Pressed += OnHelpPressed;
		CreditButton.Pressed += OnHelpPressed;
        quitButton.Pressed += OnQuitPressed;
        
        // Setup UI
        titleLabel.Text = "Waroenk Noesantara";
    }
    
    private void OnPlayPressed()
    {
		GD.Print("Play button pressed");
        GetTree().ChangeSceneToFile("res://Scenes/Levels/prototype.tscn");
    }
    
    private void OnHelpPressed()
    {
		GD.Print("Help button pressed");
        GetTree().ChangeSceneToFile("res://scenes/UI/Help.tscn");
    }
	
	private void OnCreditPressed()
	{
		GD.Print("Credit button pressed");
		// TODO: Implement credit menu
		GetTree().ChangeSceneToFile("res://scenes/ui/Credit.tscn");
	}

    private void OnQuitPressed()
	{
		GD.Print("Quit button pressed");
		GetTree().Quit();
	}
}
