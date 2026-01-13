using System.Collections.ObjectModel;
using System.Numerics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using RectangleControl.Enums;
using RectangleControl.Interfaces;
using RectangleControl.Selectors;
using Vector = System.Windows.Vector;

namespace RectangleControl.ViewModels
{
    public class MarkRectangleViewModel : BindableBase, IDataTemplateMatcher<CanvasItemsTemplateSelector.InnerSelector>
    {
        class StrechResult
        {
            public double NewX { get; set; }
            public double NewY { get; set; }
            public double NewWidth { get; set; }
            public double NewHeight { get; set; }
            public Point NewOperationPoint { get; set; }
        }

        private double _x;
        private double _y;
        private double _angle;
        private double _width;
        private double _height;
        private Point _dragStartPoint;
        private Point _dragStartCenter;
        private bool _isDragging = false;
        private static int _minValue = 15;
        private ObservableCollection<object> _marks = [];

        public double X
        {
            get { return _x; }
            set
            {
                SetProperty(ref _x, value);
                UpdateMarkPositions();
            }
        }
        public double Y
        {
            get { return _y; }
            set
            {
                SetProperty(ref _y, value);
                UpdateMarkPositions();
            }
        }

        public double Angle
        {
            get { return _angle; }
            set
            {
                SetProperty(ref _angle, value);
                UpdateMarkPositions();
            }
        }

        public double Width
        {
            get { return _width; }
            set
            {
                SetProperty(ref _width, value);
                UpdateMarkPositions();
            }
        }

        public double Height
        {
            get { return _height; }
            set
            {
                SetProperty(ref _height, value);
                UpdateMarkPositions();
            }
        }

        public Point Center => new Point(Width / 2, Height / 2);

        public Matrix TransformMatrix
        {
            get
            {
                Matrix matrix = Matrix.Identity;
                matrix.RotateAt(Angle, Width / 2, Height / 2);
                return matrix;
            }
        }

        public Matrix Matrix
        {
            get
            {
                Matrix matrix = TransformMatrix;
                matrix.Translate(X, Y); // 添加平移
                return matrix;
            }
        }

        public ObservableCollection<object> Marks
        {
            get { return _marks; }
            set
            {
                SetProperty(ref _marks, value);
            }
        }


        public MarkRectangleViewModel()
        {
            Marks.Add(new ResizeMarkViewModel { Direction = ResizeDirection.TopLeft });
            Marks.Add(new ResizeMarkViewModel { Direction = ResizeDirection.Top });
            Marks.Add(new ResizeMarkViewModel { Direction = ResizeDirection.TopRight });
            Marks.Add(new ResizeMarkViewModel { Direction = ResizeDirection.Right });
            Marks.Add(new ResizeMarkViewModel { Direction = ResizeDirection.BottomRight });
            Marks.Add(new ResizeMarkViewModel { Direction = ResizeDirection.Bottom });
            Marks.Add(new ResizeMarkViewModel { Direction = ResizeDirection.BottomLeft });
            Marks.Add(new ResizeMarkViewModel { Direction = ResizeDirection.Left });
            Marks.Add(new ResizeMarkViewModel { Direction = ResizeDirection.Center });

            var roh = new RotateHandle();
            Marks.Add(roh);
            Marks.Add(new RotateMarkViewModel(roh.Length));

            foreach (var mark in Marks)
            {
                if (mark is IDraggableMark dm)
                {
                    dm.DragStarted += OnMarkDragStarted;
                    dm.DragDelta += OnMarkDragDelta;
                    dm.DragCompleted += OnMarkDragCompleted;
                }
            }
        }

        private void OnMarkDragStarted(object sender, DragStartedArgs e)
        {
            _isDragging = true;
            _dragStartPoint = e.DragPoint;
            _dragStartCenter = Center;
        }

        private void OnMarkDragDelta(object sender, DragDeltaArgs e)
        {
            Point currentPoint = e.DragPoint;

            double deltaH = e.EventArgs.HorizontalChange;
            double deltaV = e.EventArgs.VerticalChange;

            Vector deltaVec = TransformMatrix.Transform(new Vector(deltaH, deltaV));
            var tDeltaH = deltaVec.X;
            var tDeltaV = deltaVec.Y;


            if (sender is ResizeMarkViewModel cm && cm.Direction == ResizeDirection.Center)
            {
                X += deltaH;
                Y += deltaV;
                RaisePropertyChanged(nameof(Matrix));
                return;
            }
            else if (sender is ResizeMarkViewModel rm)
            {
                double newWidth = Width;
                double newHeight = Height;
                double newX = X;
                double newY = Y;

                var oldCenter = new Point(X + Width / 2, Y + Height / 2);
                bool invalidOperation = false;

                if (rm.Direction.HasFlag(ResizeDirection.Left))
                {
                    if (rm.Direction.HasFlag(ResizeDirection.Top))
                    {
                        var re = HanldeRectCornerTransform(
                            new Point(X + Width, Y + Height),
                            currentPoint,
                            oldCenter,
                            Angle);

                        newX = re.NewX - re.NewWidth;
                        newY = re.NewY - re.NewHeight;
                        newWidth = re.NewWidth;
                        newHeight = re.NewHeight;
                        invalidOperation =
                            re.NewOperationPoint.X > newX + newWidth ||
                            re.NewOperationPoint.Y > newY + newHeight;
                    }
                    else if (rm.Direction.HasFlag(ResizeDirection.Bottom))
                    {
                        var re = HanldeRectCornerTransform(
                            new Point(X + Width, Y),
                            currentPoint,
                            oldCenter,
                            Angle);

                        newX = re.NewX - re.NewWidth;
                        newY = re.NewY;
                        newWidth = re.NewWidth;
                        newHeight = re.NewHeight;

                        invalidOperation =
                            re.NewOperationPoint.X > newX + newWidth ||
                            re.NewOperationPoint.Y < newY;
                    }
                    else
                    {
                        var re = HandleRectSideTransform(
                            new Point(X + Width, Y + Height / 2),
                            new Point(X, Y + Height / 2),
                            currentPoint,
                            oldCenter,
                            Angle);

                        newHeight = Height;
                        newWidth = re.NewWidth;
                        newX = re.NewX - re.NewWidth;
                        newY = re.NewY - newHeight / 2;

                        invalidOperation = re.NewOperationPoint.X > newX + newWidth;
                    }
                }
                else if (rm.Direction.HasFlag(ResizeDirection.Right))
                {
                    if (rm.Direction.HasFlag(ResizeDirection.Top))
                    {
                        var re = HanldeRectCornerTransform(
                            new Point(X, Y + Height),
                            currentPoint,
                            oldCenter,
                            Angle);

                        newX = re.NewX;
                        newY = re.NewY - re.NewHeight;
                        newWidth = re.NewWidth;
                        newHeight = re.NewHeight;

                        invalidOperation =
                            re.NewOperationPoint.X < newX ||
                            re.NewOperationPoint.Y > newY + newHeight;
                    }
                    else if (rm.Direction.HasFlag(ResizeDirection.Bottom))
                    {
                        var re = HanldeRectCornerTransform(
                            new Point(X, Y),
                            currentPoint,
                            oldCenter,
                            Angle);

                        newX = re.NewX;
                        newY = re.NewY;
                        newWidth = re.NewWidth;
                        newHeight = re.NewHeight;

                        invalidOperation = 
                            re.NewOperationPoint.X < newX ||
                            re.NewOperationPoint.Y < newY;
                    }
                    else
                    {
                        var re = HandleRectSideTransform(
                            new Point(X, Y + Height / 2),
                            new Point(X + Width, Y + Height / 2),
                            currentPoint,
                            oldCenter,
                            Angle);

                        newHeight = Height;
                        newWidth = re.NewWidth;
                        newX = re.NewX;
                        newY = re.NewY - newHeight / 2;

                        invalidOperation = re.NewOperationPoint.X < newX;
                    }
                }
                else if (rm.Direction.HasFlag(ResizeDirection.Top))
                {
                    var re = HandleRectSideTransform(
                        new Point(X + Width / 2, Y + Height),
                        new Point(X + Width / 2, Y),
                        currentPoint,
                        oldCenter,
                        Angle);

                    newWidth = Width;
                    newHeight = re.NewHeight;
                    newX = re.NewX - newWidth / 2;
                    newY = re.NewY - newHeight;

                    invalidOperation = re.NewOperationPoint.Y > newY + newHeight;

                }
                else if (rm.Direction.HasFlag(ResizeDirection.Bottom))
                {
                    var re = HandleRectSideTransform(
                        new Point(X + Width / 2, Y),
                        new Point(X + Width / 2, Y + Height),
                        currentPoint,
                        oldCenter,
                        Angle);

                    newWidth = Width;
                    newHeight = re.NewHeight;
                    newX = re.NewX - newWidth / 2;
                    newY = re.NewY;

                    invalidOperation = re.NewOperationPoint.Y < newY;
                }

                if (newWidth < _minValue || newHeight < _minValue || invalidOperation)
                {
                    return;
                }

                _x = newX;
                _y = newY;
                _width = Math.Abs(newWidth);
                _height = Math.Abs(newHeight);

                RaisePropertyChanged(nameof(Width));
                RaisePropertyChanged(nameof(Height));
                RaisePropertyChanged(nameof(Angle));
                RaisePropertyChanged(nameof(Matrix));
                UpdateMarkPositions();

                return;
            }

            if (sender is RotateMarkViewModel rot)
            {
                var cCenter = new Point(X + Center.X, Y + Center.Y);
                // 计算相对于中心点的向量
                Vector originRay = _dragStartPoint - cCenter;
                Vector currentRay = currentPoint - cCenter;

                // 计算角度变化并累加
                double angleDelta = Vector.AngleBetween(originRay, currentRay);
                Angle = (_angle + angleDelta) % 360;

                _dragStartPoint = currentPoint;

                RaisePropertyChanged(nameof(Angle));
                RaisePropertyChanged(nameof(Matrix));
                UpdateMarkPositions();

            }
        }

        /// <summary>
        /// Project point（Vector2）to the vector「start at A and end at B」
        /// </summary>
        /// <param name="p">p to project</param>
        /// <param name="a">vector start point</param>
        /// <param name="b">vector end point</param>
        /// <returns>the project of p on AB</returns>
        public static Vector2 ProjectPointToVector(Vector2 p, Vector2 a, Vector2 b)
        {
            Vector2 vectorV = b - a;
            Vector2 vectorU = p - a;

            float dotProduct = Vector2.Dot(vectorU, vectorV);

            float vectorVLengthSquared = vectorV.X * vectorV.X + vectorV.Y * vectorV.Y;

            if (Math.Abs(vectorVLengthSquared) < 1e-9f)
            {
                return a;
            }

            float t = dotProduct / vectorVLengthSquared;

            return a + vectorV * t;
        }

        /// <summary>
        /// Calculate the fixed point(the opposite of the operation point, e.g. If operating on top left, the fixed one is bottom right)
        /// </summary>
        /// <param name="originPt">origin fixed point</param>
        /// <param name="rotatedOppositePt">operating point</param>
        /// <param name="originCenter">origion rect center</param>
        /// <param name="angle">the rotation angle</param>
        /// <returns></returns>
        private StrechResult HanldeRectCornerTransform(Point originPt, Point rotatedOppositePt, Point originCenter, double angle)
        {
            Matrix m1 = Matrix.Identity;
            m1.RotateAt(angle, originCenter.X, originCenter.Y);

            var rotPt = m1.Transform(originPt);
            var rotOpPt = rotatedOppositePt;

            var rotNewCenter = new Point((rotPt.X + rotOpPt.X) / 2, (rotPt.Y + rotOpPt.Y) / 2);

            Matrix m = Matrix.Identity;
            m.RotateAt(-angle, rotNewCenter.X, rotNewCenter.Y);
            var newPt = m.Transform(rotPt);
            var newOppositePt = m.Transform(rotOpPt);

            return new StrechResult
            {
                NewX = newPt.X,
                NewY = newPt.Y,
                NewWidth = Math.Abs(newOppositePt.X - newPt.X),
                NewHeight = Math.Abs(newOppositePt.Y - newPt.Y),
                NewOperationPoint = newOppositePt
            };
        }

        private StrechResult HandleRectSideTransform(Point originPt, Point originOppositePt, Point rotatedOppositePt, Point originCenter, double angle)
        {
            Matrix m1 = Matrix.Identity;
            m1.RotateAt(angle, originCenter.X, originCenter.Y);

            var rotPt = m1.Transform(new Point(originPt.X, originPt.Y));
            var rotOppo = m1.Transform(new Point(originOppositePt.X, originOppositePt.Y));

            var targetVect = ProjectPointToVector(
                new Vector2((float)rotatedOppositePt.X, (float)rotatedOppositePt.Y),
                new Vector2((float)rotPt.X, (float)rotPt.Y),
                new Vector2((float)rotOppo.X, (float)rotOppo.Y));

            var rotOpPt = new Point(targetVect.X, targetVect.Y);
            var rotNewCenter = new Point((rotPt.X + rotOpPt.X) / 2, (rotPt.Y + rotOpPt.Y) / 2);

            Matrix m = Matrix.Identity;
            m.RotateAt(-angle, rotNewCenter.X, rotNewCenter.Y);
            var newPt = m.Transform(rotPt);
            var newOppositePt = m.Transform(rotOpPt);

            return new StrechResult
            {
                NewX = newPt.X,
                NewY = newPt.Y,
                NewWidth = Math.Abs(newOppositePt.X - newPt.X),
                NewHeight = Math.Abs(newOppositePt.Y - newPt.Y),
                NewOperationPoint = newOppositePt
            };
        }


        private void OnMarkDragCompleted(object sender, DragCompletedEventArgs e)
        {
            _isDragging = false;
        }

        private void UpdateMarkPositions()
        {
            foreach (var mark in Marks.Cast<IMark>())
            {
                Point localPosition = GetInitialPosition(mark);
                Point transformedPosition = Matrix.Transform(localPosition);

                mark.Angle = Angle;
                mark.Position = transformedPosition;

                if (mark is RotateHandle rh)
                {
                    rh.Update(GetInitialPosition(rh), Angle, Matrix);
                }
            }
        }

        private Point GetInitialPosition(IMark mark)
        {
            if (mark is ResizeMarkViewModel rm)
            {
                ResizeDirection direction = rm.Direction;
                double x = direction.HasFlag(ResizeDirection.Left) ? 0 :
                    direction.HasFlag(ResizeDirection.Right) ? Width :
                    Width / 2;

                double y = direction.HasFlag(ResizeDirection.Top) ? 0 :
                            direction.HasFlag(ResizeDirection.Bottom) ? Height :
                            Height / 2;

                return new Point(x, y);
            }

            if (mark is RotateMarkViewModel rot)
            {
                return new Point(Width / 2, -rot.HandleLength - rot.Width / 2);
            }

            if (mark is RotateHandle)
            {
                return new Point(Width / 2, 0);
            }

            return new Point();
        }

        public DataTemplate? MatchTemplate(CanvasItemsTemplateSelector.InnerSelector selector)
        {
            return selector.MarkRectangleTemplate;
        }
    }
}