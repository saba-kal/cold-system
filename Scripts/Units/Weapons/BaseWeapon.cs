using Godot;

public abstract partial class BaseWeapon : Node3D
{
    [Export(PropertyHint.Enum, Constants.UNIT_GROUPS)] protected string _targetGroup = Constants.ENEMY_UNIT_GROUP;

    private AudioStreamPlayer3D _soundEffectPlayer;

    public bool Enabled { get; set; }

    public override void _Ready()
    {
        _soundEffectPlayer = this.GetChildNode<AudioStreamPlayer3D>();
    }

    public void PlaySoundEffect()
    {
        _soundEffectPlayer?.Play();
    }
}
