using OpenCvSharp;

namespace Aula3D.VisionCore
{
    /// <summary>Resultado de uma única mão detectada num frame.</summary>
    public class HandTrackingResult
    {
        public bool HandDetected { get; set; }
        public bool IsHandOpen { get; set; }
        public string? State { get; set; }
        public Point CenterOfMass { get; set; }
        public Rect BoundingRect { get; set; }
        public Point[]? Contour { get; set; }
        public Point[]? DefectPoints { get; set; }
    }
}
