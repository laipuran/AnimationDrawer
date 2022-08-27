using System.Collections.Generic;
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

        public static SingleFrame GetSingleFrame(StrokeCollection strokes, ImageSource? source)
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

        public static List<SingleFrame> GetFrames(AnimationPiece piece)
        {
            return piece.Frames;
        }

        public static List<StrokeCollection> GetStrokeCollections(AnimationPiece piece)
        {
            List<StrokeCollection> strokes = new();
            foreach (SingleFrame item in piece.Frames)
            {
                strokes.Add(SingleFrame.GetStrokes(item));
            }
            return strokes;
        }

        public static List<ImageSource?> GetImageSources(AnimationPiece piece)
        {
            List<ImageSource?> sources = new();
            foreach (SingleFrame frame in piece.Frames)
            {
                sources.Add(frame.Background);
            }
            return sources;
        }

        public static AnimationPiece GetAnimationPiece(List<SingleFrame> frames)
        {
            AnimationPiece piece = new()
            {
                Frames = frames
            };
            return piece;
        }

        public static AnimationPiece GetAnimationPiece(List<StrokeCollection> strokes, ImageSource? source)
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
        public static AnimationPiece MergeAnimationPieces(AnimationPiece piece1, AnimationPiece piece2)
        {
            AnimationPiece piece = new();
            piece.Frames.AddRange(piece1.Frames);
            piece.Frames.AddRange(piece2.Frames);
            return piece;
        }
    }
}
