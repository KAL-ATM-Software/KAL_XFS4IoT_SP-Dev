/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;

namespace XFS4IoTFramework.Printer
{
    /// <summary>
    /// A Task represents a small bit of data to be printed (or scanned).
    /// Different classes are derived from <see cref="PrintTask"/> for different types or
    /// data.For instance <see cref="TextTask"/> represents a line of text to print.
    /// </summary>
    public abstract class PrintTask
    {
        public abstract FieldTypeEnum Type { get; init; }
        public int x { get; set; } // x position of task in dots
        public int y { get; set; } // y position of task in dots
    }

    /// <summary>
    /// Represents a fragment of text to be printed at a certain position.
    /// The text must be printed on a single line.
    /// </summary>
    public sealed class TextTask : PrintTask
    {
        public TextTask(int x,
                        int y,
                        string Text,
                        FieldStyleEnum Style,
                        string FontName,
                        int PointSize,
                        int CPI,
                        int LPI,
                        FieldSideEnum Side,
                        FieldColorEnum Color,
                        string Format,
                        bool RowColumn)
        {
            this.x = x;
            this.y = y;
            this.Text = Text;
            this.Style = Style;
            this.FontName = FontName;
            this.PointSize = PointSize;
            this.CPI = CPI;
            this.LPI = LPI;
            this.Side = Side;
            this.Color = Color;
            this.Format = Format;
            this.RowColumn = RowColumn;
        }
        public TextTask(TextTask task)
        {
            x = task.x;
            y = task.y;
            Text = task.Text;
            Style = task.Style;
            FontName = task.FontName;
            PointSize = task.PointSize;
            CPI = task.CPI;
            LPI = task.LPI;
            Side = task.Side;
            Color = task.Color;
            Format = task.Format;
            RowColumn = task.RowColumn;
        }

        public override FieldTypeEnum Type { get; init; } = FieldTypeEnum.TEXT;

        /// <summary>
        /// The text to print
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Combination of STYLE constants
        /// </summary>
        public FieldStyleEnum Style { get; init; }
        public string FontName { get; init; }
        public int PointSize { get; init; }
        public int CPI { get; init; }
        public int LPI { get; init; }

        public FieldSideEnum Side { get; init; }
        public FieldColorEnum Color { get; init; }
        public string Format { get; init; }
        /// <summary>
        /// Flag set true if field came from ROWCOLUMN based form
        /// </summary>
        public bool RowColumn { get; init; }
    }

    /// <summary>
    /// GraphicTask Class
    /// </summary>
    public sealed class GraphicTask : PrintTask
    {
        public enum ImageFormatEnum
        {
            BMP,
            JPG,
        }

        public override FieldTypeEnum Type { get; init; } = FieldTypeEnum.GRAPHIC;

        public GraphicTask(int x,
                           int y,
                           int Width,
                           int Height,
                           ImageFormatEnum Format,
                           FieldScalingEnum Scaling,
                           List<byte> Image)
        {
            this.x = x;
            this.y = y;
            this.Width = Width;
            this.Height = Height;
            this.Format = Format;
            this.Scaling = Scaling;
            this.Image = Image;
        }
        public GraphicTask(GraphicTask task)
        {
            x = task.x;
            y = task.y;
            Width = task.Width;
            Height = task.Height;
            Format = task.Format;
            Scaling = task.Scaling;
            Image = task.Image;
        }

        /// <summary>
        /// Size of box in which bitmap must fit 
        /// </summary>
        public int Width { get; init; }
        public int Height { get; init; }

        /// <summary>
        /// Format of the image stored in the Image property
        /// </summary>
        public ImageFormatEnum Format { get; init; }
        /// <summary>
        /// Image data
        /// </summary>
        public List<byte> Image { get; init; }
        /// <summary>
        /// Information on how to size the GRAPHIC within GRAPHIC field types
        /// BESTFIT: Scale to size indicated.
        /// ASIS: Render at native size.
        /// MAINTAINASPECT: scale as close as possible to size indicated while maintaining the aspect ratio 
        /// and not losing graphic information.
        /// </summary>
        public FieldScalingEnum Scaling { get; init; }
    }

    /// <summary>
    /// BarcodeTask Class
    /// </summary>
    public sealed class BarcodeTask : PrintTask
    {
        public BarcodeTask(int x,
                           int y,
                           string Value,
                           FieldBarcodeEnum Position,
                           string BarcodeFontName,
                           int Width,
                           int Height)
        {
            this.x = x;
            this.y = y;
            this.Value = Value;
            this.Position = Position;
            this.BarcodeFontName = BarcodeFontName;
            this.Width = Width;
            this.Height = Height;
        }
        public BarcodeTask(BarcodeTask task)
        {
            x = task.x;
            y = task.y;
            Value = task.Value;
            Position = task.Position;
            BarcodeFontName = task.BarcodeFontName;
            Width = task.Width;
            Height = task.Height;
        }

        public override FieldTypeEnum Type { get; init; } = FieldTypeEnum.BARCODE;

        public string Value { get; init; }
        /// <summary>
        /// Human Readable Interpretation
        /// </summary>
        public FieldBarcodeEnum Position { get; init; }
        public string BarcodeFontName { get; init; }
        public int Width { get; init; }
        public int Height { get; init; }
    }

    /// <summary>
    /// A <see cref="PrintJobClass"/> is basically a collection of <see cref="PrintTask"/>s that together
    /// define a complete document to be printed or scanned.
    /// The class also provides a method for converting the job to a bitmap.
    /// </summary>
    public sealed class PrintJobClass
    {
        public PrintJobClass()
        {
            Tasks = new();
        }

        /// <summary>
        /// Orientation of document
        /// </summary>
        public FormOrientationEnum Orientation { get; set; }
        /// <summary>
        /// Total length along paper of job, including empty space above and below tasks
        /// </summary>
        public int PrintLength;

        /// <summary>
        /// Print tasks to be processed
        /// </summary>
        public List<PrintTask> Tasks;

        /// <summary>
        /// Sorts tasks into left-to-right, top-to-bottom order.
        /// </summary>
        public void SortTasks()
        {
            bool Swapped;
            int Lim = Tasks.Count - 1;
            do
            {
                Swapped = false;
                for (int i = 0; i < Lim; i++)
                {
                    if (Tasks[i].y > Tasks[i + 1].y ||
                        (Tasks[i].y == Tasks[i + 1].y &&
                         Tasks[i].x > Tasks[i + 1].x))
                    {
                        PrintTask T = Tasks[i];
                        Tasks[i] = Tasks[i + 1];
                        Tasks[i + 1] = T;
                        Swapped = true;
                    }

                }
                Lim--;
            }
            while (Swapped);
        }
    }
}
