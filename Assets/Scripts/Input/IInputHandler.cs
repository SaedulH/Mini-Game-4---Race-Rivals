public interface IInputHandler
{
    float Throttle { get; set; }
    float Steering { get; set; }
    bool HandBrake { get; set; }
}
