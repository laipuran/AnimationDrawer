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
        public DrawingAttributes DrawingAttributes = new();
        public List<StylusPoint> StylusPoints = new();

        public static StrokeProperty GetStrokeProperty(Stroke stroke)
        {
            StrokeProperty strokeProperty = new()
            {
                DrawingAttributes = stroke.DrawingAttributes
            };
            foreach (StylusPoint item in stroke.StylusPoints)
            {
                strokeProperty.StylusPoints.Add(item);
            }
            return strokeProperty;
        }

        public static Stroke GetStroke(StrokeProperty strokeProperty)
        {
            Stroke stroke = new(GetStylusPointsCollection(strokeProperty.StylusPoints), strokeProperty.DrawingAttributes);
            return stroke;
        }

        public static StylusPointCollection GetStylusPointsCollection(List<StylusPoint> stylusPoints)
        {
            StylusPointCollection points = new();
            foreach (StylusPoint item in stylusPoints)
            {
                points.Insert(points.Count, item);
            }
            return points;
        }
    }

    public class SingleFrame
    {
        public List<StrokeProperty> StrokeProperties = new();
        public ImageSource? Background;

        public static SingleFrame GetSingleFrame(StrokeCollection strokes)
        {
            SingleFrame frame = new();
            foreach (Stroke item in strokes)
            {
                frame.StrokeProperties.Add(StrokeProperty.GetStrokeProperty(item));
            }
            return frame;
        }

        public static SingleFrame GetSingleFrame(StrokeCollection strokes, ImageSource source)
        {
            SingleFrame frame = new();
            foreach (Stroke item in strokes)
            {
                frame.StrokeProperties.Add(StrokeProperty.GetStrokeProperty(item));
            }
            frame.Background = source;
            return frame;
        }

        public static StrokeCollection GetStrokes(SingleFrame frame)
        {
            StrokeCollection strokes = new();
            foreach (StrokeProperty item in frame.StrokeProperties)
            {
                strokes.Add(StrokeProperty.GetStroke(item));
            }
            return strokes;
        }
    }
    
    public class AnimationPiece
    {
        public List<SingleFrame> Frames = new();
        // public int FPS

        public static List<SingleFrame> GetFrames(AnimationPiece piece)
        {
            return piece.Frames;
        }

        public static List<StrokeCollection> GetCollectionList(AnimationPiece piece)
        {
            List<StrokeCollection> strokes = new();
            foreach (SingleFrame item in piece.Frames)
            {
                strokes.Add(SingleFrame.GetStrokes(item));
            }
            return strokes;
        }

        public static AnimationPiece GetAnimationPiece(List<SingleFrame> frames)
        {
            AnimationPiece piece = new()
            {
                Frames = frames
            };
            return piece;
        }

        public static AnimationPiece GetAnimationPiece(List<StrokeCollection> strokes)
        {
            AnimationPiece piece = new();
            foreach (StrokeCollection stroke in strokes)
            {
                piece.Frames.Add(SingleFrame.GetSingleFrame(stroke));
            }
            return piece;
        }

        public static AnimationPiece GetAnimationPiece(List<StrokeCollection> strokes, ImageSource source)
        {
            AnimationPiece piece = new();
            foreach (StrokeCollection stroke in strokes)
            {
                piece.Frames.Add(SingleFrame.GetSingleFrame(stroke, source));
            }
            return piece;
        }

        public static AnimationPiece MergeAnimationPieces(List<AnimationPiece> pieces)
        {
            AnimationPiece piece = new();
            foreach (AnimationPiece item in pieces)
            {
                piece.Frames.AddRange(item.Frames);
            }
            return piece;
        }
    }
}
