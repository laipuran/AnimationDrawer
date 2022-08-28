using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace AnimationDrawer.Ink
{
    public static class ExtensionMethods
    {
        public static StrokeProperty GetStrokeProperty(this Stroke stroke)
        {
            StrokeProperty strokeProperty = new()
            {
                Attributes = stroke.DrawingAttributes
            };

            foreach (StylusPoint item in stroke.StylusPoints)
            {
                strokeProperty.StylusPoints.Add(item);
            }
            return strokeProperty;
        }
        public static SingleFrame GetSingleFrame(this InkCanvas canvas)
        {
            return SingleFrame.GetSingleFrame(canvas.Strokes, ((ImageBrush)canvas.Background).ImageSource);
        }
    }

    interface IStrokeProperty
    {
        StylusPointCollection GetPointCollection();
        
    }

    public class StrokeProperty : IStrokeProperty
    {
        public DrawingAttributes Attributes = new();
        public List<StylusPoint> StylusPoints = new();

        public Stroke Stroke
        {
            get
            {
                return new(GetPointCollection(), Attributes);
            }
        }

        public StylusPointCollection GetPointCollection()
        {
            StylusPointCollection points = new();
            foreach (StylusPoint item in StylusPoints)
            {
                points.Insert(points.Count, item);
            }
            return points;
        }

    }

    interface ISingleFrame
    {
        StrokeCollection GetStrokes();
        ImageBrush GetBrush();
    }

    public class SingleFrame : ISingleFrame
    {
        public List<StrokeProperty> StrokeProperties = new();
        public ImageSource? Background;

        public StrokeCollection GetStrokes()
        {
            StrokeCollection strokes = new();
            foreach (StrokeProperty item in StrokeProperties)
            {
                strokes.Add(item.Stroke);
            }
            return strokes;
        }

        public ImageBrush GetBrush()
        {
            return new ImageBrush(Background);
        }

        public static SingleFrame GetSingleFrame(StrokeCollection strokes, ImageSource? source)
        {
            SingleFrame frame = new();
            foreach (Stroke item in strokes)
            {
                frame.StrokeProperties.Add(item.GetStrokeProperty());
            }
            frame.Background = source;
            return frame;
        }


    }

    public class AnimationPiece
    {
        public List<SingleFrame> Frames = new();

        public int Count
        {
            get
            {
                return Frames.Count;
            }
        }

        public static List<StrokeCollection> GetStrokeCollections(AnimationPiece piece)
        {
            List<StrokeCollection> strokes = new();
            foreach (SingleFrame item in piece.Frames)
            {
                strokes.Add(item.GetStrokes());
            }
            return strokes;
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

    }
}
