using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace AnimationDrawer
{
    public class StrokeProperty
    {
        DrawingAttributes drawingAttributes = new();
        List<StylusPoint> stylusPoints = new();

        public static StrokeProperty GetStrokeProperty(Stroke stroke)
        {
            StrokeProperty strokeProperty = new();
            strokeProperty.drawingAttributes = stroke.DrawingAttributes;
            foreach (StylusPoint item in stroke.StylusPoints)
            {
                strokeProperty.stylusPoints.Add(item);
            }
            return strokeProperty;
        }

        public static Stroke GetStroke(StrokeProperty strokeProperty)
        {
            Stroke stroke = new(GetStylusPoints(strokeProperty.stylusPoints), strokeProperty.drawingAttributes);
            return stroke;
        }

        public static StylusPointCollection GetStylusPoints(List<StylusPoint> stylusPoints)
        {
            StylusPointCollection points = new();
            foreach (StylusPoint item in stylusPoints)
            {
                points.Append(item);
            }
            return points;
        }
    }

    public class SingleFrame
    {
        List<StrokeProperty> strokeProperties = new();
        ImageSource? backGround;

        public static SingleFrame GetSingleFrame(StrokeCollection strokes)
        {
            SingleFrame frame = new();
            foreach (Stroke item in strokes)
            {
                frame.strokeProperties.Add(StrokeProperty.GetStrokeProperty(item));
            }
            return frame;
        }

        public static SingleFrame GetSingleFrame(StrokeCollection strokes, ImageSource source)
        {
            SingleFrame frame = new();
            foreach (Stroke item in strokes)
            {
                frame.strokeProperties.Add(StrokeProperty.GetStrokeProperty(item));
            }
            frame.backGround = source;
            return frame;
        }

        public static StrokeCollection GetStrokes(SingleFrame frame)
        {
            StrokeCollection strokes = new();
            foreach (var item in frame.strokeProperties)
            {
                strokes.Add(StrokeProperty.GetStroke(item));
            }
            return strokes;
        }
    }
    
    public class AnimationPiece
    {
        List<SingleFrame> frames = new();
    }
}
