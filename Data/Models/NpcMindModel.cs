using Godot;
using Godot.Collections;

namespace GoldenHand.Data.Models;

[GlobalClass, Tool]
public partial class NpcMindModel : GameStateModel
{
    
    [Export] public EmotionModel PlayerAttitude { get; set; }
    
    [Export] public Dictionary<string, EmotionModel> TopicAttitudes { get; set; }
    
    [Export] public Dictionary<string, EmotionModel> BelievedPlayerAttitudes { get; set; }
    [Export] public float Trust { get; set; } = 0.5f;

}