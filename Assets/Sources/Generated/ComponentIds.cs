public static class ComponentIds {
    public const int Background = 0;
    public const int Bounds = 1;
    public const int Bullet = 2;
    public const int ColorAnimation = 3;
    public const int Destroy = 4;
    public const int Enemy = 5;
    public const int Expires = 6;
    public const int Firing = 7;
    public const int Health = 8;
    public const int Input = 9;
    public const int Interactive = 10;
    public const int Movable = 11;
    public const int ParallaxStar = 12;
    public const int Player = 13;
    public const int Position = 14;
    public const int Resource = 15;
    public const int ScaleAnimation = 16;
    public const int Scale = 17;
    public const int Score = 18;
    public const int SoundEffect = 19;
    public const int Velocity = 20;
    public const int View = 21;

    public const int TotalComponents = 22;

    static readonly string[] _components = {
        "Background",
        "Bounds",
        "Bullet",
        "ColorAnimation",
        "Destroy",
        "Enemy",
        "Expires",
        "Firing",
        "Health",
        "Input",
        "Interactive",
        "Movable",
        "ParallaxStar",
        "Player",
        "Position",
        "Resource",
        "ScaleAnimation",
        "Scale",
        "Score",
        "SoundEffect",
        "Velocity",
        "View"
    };

    public static string IdToString(int componentId) {
        return _components[componentId];
    }
}