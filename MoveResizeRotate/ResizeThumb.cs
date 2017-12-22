using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.IO;
using System.Text;

namespace DiagramDesigner
{
    public class ResizeThumb : Thumb
    {
        private double angle;
        private Point transformOrigin;
        public ContentControl designerItem;
        CSVFile NewCsvFile = new CSVFile();
        string CsvPath = "D:\\ResizeDatas.csv";
        int CallCount = 0;

        public ResizeThumb()
        {
            DragStarted += new DragStartedEventHandler(this.ResizeThumb_DragStarted);
            DragDelta += new DragDeltaEventHandler(this.ResizeThumb_DragDelta);

            
        }

        private void ResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.designerItem = DataContext as ContentControl;

            if (this.designerItem != null)
            {
                this.transformOrigin = this.designerItem.RenderTransformOrigin;
                RotateTransform rotateTransform = this.designerItem.RenderTransform as RotateTransform;

                if (rotateTransform != null)
                {
                    this.angle = rotateTransform.Angle * Math.PI / 180.0;
                }
                else
                {
                    this.angle = 0;
                }
            }
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            CallCount++;

            double PreviousTop = Canvas.GetTop(this.designerItem);

            if (this.designerItem != null)
            {
                double deltaVertical = 0.0;
                double deltaHorizontal = 0.0;

                switch (VerticalAlignment)
                {
                    case System.Windows.VerticalAlignment.Bottom:
                        deltaVertical = Math.Min(-e.VerticalChange, this.designerItem.ActualHeight - this.designerItem.MinHeight);
                        Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) + (this.transformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angle))));
                        Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) - deltaVertical * this.transformOrigin.Y * Math.Sin(-this.angle));
                        this.designerItem.Height -= deltaVertical;
                        break;
                    case System.Windows.VerticalAlignment.Top:
                        deltaVertical = Math.Min(e.VerticalChange, this.designerItem.ActualHeight - this.designerItem.MinHeight);
                        Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) + deltaVertical * Math.Cos(-this.angle) + (this.transformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angle))));
                        Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) + deltaVertical * Math.Sin(-this.angle) - (this.transformOrigin.Y * deltaVertical * Math.Sin(-this.angle)));
                        this.designerItem.Height -= deltaVertical;
                        break;
                    default:
                        break;
                }

                if (CallCount == 1)
                {
                    ArrayList ResizeDataField = new ArrayList();
                    ResizeDataField.Add("deltaVertical");
                    ResizeDataField.Add("VerticalChange");
                    ResizeDataField.Add("Canvas's Previous Top");
                    ResizeDataField.Add("transformOrigin.Y");
                    ResizeDataField.Add("rotate angle");
                    ResizeDataField.Add("Canvas's Current Top");

                    NewCsvFile.CreateCSVFile(ResizeDataField, CsvPath);
                }

                ArrayList ResizeDatas = new ArrayList();
                ResizeDatas.Add(deltaVertical);
                ResizeDatas.Add(e.VerticalChange);
                ResizeDatas.Add(PreviousTop);
                ResizeDatas.Add(this.transformOrigin.Y);
                ResizeDatas.Add(this.angle);
                ResizeDatas.Add(Canvas.GetTop(this.designerItem));

                NewCsvFile.CreateCSVFile(ResizeDatas, CsvPath);

                switch (HorizontalAlignment)
                {
                    case System.Windows.HorizontalAlignment.Left:
                        deltaHorizontal = Math.Min(e.HorizontalChange, this.designerItem.ActualWidth - this.designerItem.MinWidth);
                        Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) + deltaHorizontal * Math.Sin(this.angle) - this.transformOrigin.X * deltaHorizontal * Math.Sin(this.angle));
                        Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) + deltaHorizontal * Math.Cos(this.angle) + (this.transformOrigin.X * deltaHorizontal * (1 - Math.Cos(this.angle))));
                        this.designerItem.Width -= deltaHorizontal;
                        break;
                    case System.Windows.HorizontalAlignment.Right:
                        deltaHorizontal = Math.Min(-e.HorizontalChange, this.designerItem.ActualWidth - this.designerItem.MinWidth);
                        Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) - this.transformOrigin.X * deltaHorizontal * Math.Sin(this.angle));
                        Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) + (deltaHorizontal * this.transformOrigin.X * (1 - Math.Cos(this.angle))));
                        this.designerItem.Width -= deltaHorizontal;
                        break;
                    default:
                        break;
                }

                System.Diagnostics.Debug.WriteLine("deltaHorizontal: " + deltaHorizontal + " " + "Canvas.Top: " + Canvas.GetTop(this.designerItem) + " "
                                                    + "Canvas.Left: " + Canvas.GetLeft(this.designerItem) + " " + "angle: " + this.angle + " " + 
                                                    "transformOrigin.X:" + this.transformOrigin.X);
                System.Diagnostics.Debug.WriteLine("deltaVertical: " + deltaVertical + " " + "Canvas.Top: " + Canvas.GetTop(this.designerItem) + " "
                                                    + "Canvas.Left: " + Canvas.GetLeft(this.designerItem) + " " + "angle: " + this.angle + " " +
                                                    "transformOrigin.Y:" + this.transformOrigin.Y);
            }

            e.Handled = true;
        }


       
    }
}
