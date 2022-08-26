using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace AnimationDrawer.Ink
{
    public class StrokeProperty
    {
        public DrawingAttributes drawingAttributes = new();
        public List<StylusPoint> stylusPoints = new();

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
            Stroke stroke = new(GetStylusPointsCollection(strokeProperty.stylusPoints), strokeProperty.drawingAttributes);
            return stroke;
        }

        public static StylusPointCollection GetStylusPointsCollection(List<StylusPoint> stylusPoints)
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
        public List<StrokeProperty> strokeProperties = new();
        public ImageSource? backGround;

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
            foreach (StrokeProperty item in frame.strokeProperties)
            {
                strokes.Add(StrokeProperty.GetStroke(item));
            }
            return strokes;
        }
    }
    
    public class AnimationPiece
    {
        public List<SingleFrame> frames = new();
        // public int FPS

        public static List<SingleFrame> GetFrames(AnimationPiece piece)
        {
            return piece.frames;
        }

        public static List<StrokeCollection> GetCollectionList(AnimationPiece piece)
        {
            List<StrokeCollection> strokes = new();
            foreach (SingleFrame item in piece.frames)
            {
                strokes.Add(SingleFrame.GetStrokes(item));
            }
            return strokes;
        }

        public static AnimationPiece GetAnimationPiece(List<SingleFrame> frames)
        {
            AnimationPiece piece = new();
            piece.frames = frames;
            return piece;
        }

        public static AnimationPiece GetAnimationPiece(List<StrokeCollection> strokes)
        {
            AnimationPiece piece = new AnimationPiece();
            foreach (StrokeCollection stroke in strokes)
            {
                piece.frames.Add(SingleFrame.GetSingleFrame(stroke));
            }
            return piece;
        }

        public static AnimationPiece GetAnimationPiece(List<StrokeCollection> strokes, ImageSource source)
        {
            AnimationPiece piece = new AnimationPiece();
            foreach (StrokeCollection stroke in strokes)
            {
                piece.frames.Add(SingleFrame.GetSingleFrame(stroke, source));
            }
            return piece;
        }

        public static AnimationPiece MergeAnimationPieces(List<AnimationPiece> pieces)
        {
            AnimationPiece piece = new();
            foreach (AnimationPiece item in pieces)
            {
                piece.frames.AddRange(item.frames);
            }
            return piece;
        }
    }
}
