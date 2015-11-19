public static class ComponentIds {
    public const int Background = 0;
    public const int Bounds = 1;
    public const int Bullet = 2;
    public const int ColorAnimation = 3;
    public const int Destroy = 4;
    public const int Enemy = 5;
    public const int Expires = 6;
    public const int Firing = 7;
    public const int GameBoardCache = 8;
    public const int GameBoard = 9;
    public const int GameBoardElement = 10;
    public const int Health = 11;
    public const int Input = 12;
    public const int Interactive = 13;
    public const int Movable = 14;
    public const int Player = 15;
    public const int Position = 16;
    public const int Resource = 17;
    public const int ScaleAnimation = 18;
    public const int Scale = 19;
    public const int Score = 20;
    public const int SoundEffect = 21;
    public const int Velocity = 22;
    public const int View = 23;

    public const int TotalComponents = 24;

    static readonly string[] _components = {
        "Background",
        "Bounds",
        "Bullet",
        "ColorAnimation",
        "Destroy",
        "Enemy",
        "Expires",
        "Firing",
        "GameBoardCache",
        "GameBoard",
        "GameBoardElement",
        "Health",
        "Input",
        "Interactive",
        "Movable",
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