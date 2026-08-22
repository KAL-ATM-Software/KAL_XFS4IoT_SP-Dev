/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2026
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XFS4IoT;
using XFS4IoTFramework.Printer;

namespace XFS4IoTServer
{
    internal sealed class PrintToBitmapHandler
    {
        public PrintToBitmapHandler(IPrinterDevice Device, ILogger Logger)
        {
            this.Device = Device;
            this.Logger = Logger;

            // Set default width and height to row column width and height
            DefaultCharWidth = this.Device.DotsPerColumnTop / this.Device.DotsPerColumnBottom;
            DefaultCharHeight = this.Device.DotsPerRowTop / this.Device.DotsPerRowBottom;
        }

        /// <summary>
        /// Convert XFS form data to the image
        /// </summary>
        /// <param name="FullImage">
        /// When false (default), the returned image is a tight crop around just the tasks present in this
        /// job, with ImageInfo.OffsetX/OffsetY giving the crop's position relative to the media's top-left -
        /// callers that need a full-media-sized image must composite the crop onto their own canvas at that
        /// offset. When true, the image covers the entire media instead: width comes from
        /// Device.MediaSpecs[0].Width and height from job.PrintLength (falling back to the tight crop's own
        /// dimensions if either is unavailable), tasks are drawn at their absolute positions, and
        /// ImageInfo.OffsetX/OffsetY are always 0 since no further compositing is needed.
        /// </param>
        public bool Convert(PrintJobClass job, int bitCount, bool UpsideDown, out ImageInfo imageInfo, bool FullImage = false)
        {
            imageInfo = null;

            int right = -10000;
            int bottom = -10000;
            int offsetX = 10000;
            int offsetY = 10000;

            bool success = false;
            // only support monochrome color for now
            if (bitCount != 1 &&
                bitCount != 24)
            {
                Logger.Warning(Constants.Framework, $"The framework supports only monochrome color or full color at the moment. {bitCount}");
                return success;
            }

            // Look through all list of tasks to find out bounding of dimensions that requires for printing.
            foreach (var task in job.Tasks)
            {
                success = GetTaskDimensions(task, out int width, out int height);
                if (!success)
                {
                    return success;
                }
                if (task.x < offsetX)
                {
                    offsetX = task.x;
                }
                if (task.y < offsetY)
                {
                    offsetY = task.y;
                }
                if (task.x + width > right)
                {
                    right = task.x + width;
                }
                if (task.y + height > bottom)
                {
                    bottom = task.y + height;
                }
            }

            int currentWidth = right - offsetX;
            if (currentWidth == 0)
            {
                currentWidth = 1;
            }
            int currentHeight = bottom - offsetY;
            if (currentHeight == 0)
            {
                currentHeight = 1;
            }

            if (FullImage)
            {
                if (Device.MediaSpecs is { Count: > 0 } && Device.MediaSpecs[0].Width > 0)
                {
                    currentWidth = Device.MediaSpecs[0].Width;
                }
                if (job.PrintLength > 0)
                {
                    currentHeight = job.PrintLength;
                }
                // Use real absolute position on the full canvas instead of relative to the crop.
                offsetX = 0;
                offsetY = 0;
            }

            // Initialise our bitmap to the size of the rectangle
            SkiaBitmapExportContext image = new(currentWidth, currentHeight, 1.0f);
            ICanvas canvas = image.Canvas;

            // Turn off antialias for monochrome bitmap to make the output sharper at all. instead of blurry output with anti-alias on
            if (bitCount == 1)
            {
                canvas.Antialias = false;
            }

            // Set up default colors
            canvas.FillColor = Colors.White;
            canvas.FontColor = Colors.Black;
            canvas.FillRectangle(0, 0, currentWidth, currentHeight);

            // Draw GRAPHIC tasks first, then everything else, so text/barcodes always render on top of any
            // background image regardless of where each task's position happens to fall in the
            // position-based sort order PrintJobClass.SortTasks already applied to job.Tasks.
            foreach (PrintTask task in job.Tasks.Where(t => t.Type == FieldTypeEnum.GRAPHIC))
            {
                success = PrintGraphicTask(canvas, task.IsA<GraphicTask>($"Unexpected task. {task.Type}"), offsetX, offsetY);

                if (!success)
                {
                    Logger.Warning(Constants.Framework, $"Failed to print data on the image. {task.Type}");
                    break;
                }
            }

            if (success)
            {
                foreach (PrintTask task in job.Tasks.Where(t => t.Type != FieldTypeEnum.GRAPHIC))
                {
                    switch (task.Type)
                    {
                        case FieldTypeEnum.TEXT:
                            success = PrintTextTask(canvas, task.IsA<TextTask>($"Unexpected task. {task.Type}"), offsetX, offsetY);
                            break;
                        case FieldTypeEnum.BARCODE:
                            success = PrintBarcodeTask(canvas, task.IsA<BarcodeTask>($"Unexpected task. {task.Type}"), offsetX, offsetY);
                            break;
                        default:
                            Contracts.Fail($"Unsupported type of task received. {task.Type}");
                            break;
                    }

                    if (!success)
                    {
                        Logger.Warning(Constants.Framework, $"Failed to print data on the image. {task.Type}");
                        break;
                    }
                }
            }

            if (job.Orientation == FormOrientationEnum.LANDSCAPE)
            {
                // Change orientation
                canvas.Rotate(90);
            }

            GetImage(image.Bitmap, bitCount, out ImageData imageData);

            imageInfo = new(offsetX, offsetY, imageData);

            return success;
        }

        /// <summary>
        /// This method can be called in the device class to obtain the dimensions of a KXTask when printed using PrintToImage
        /// </summary>
        public bool GetTaskDimensions(PrintTask task, out int width, out int height)
        {
            task.IsNotNull($"An empty task passed in the {nameof(GetTaskDimensions)}");
            width = -1;
            height = -1;
            bool success = true;

            SkiaBitmapExportContext image = new(1, 1, 1.0f);
            ICanvas canvas = image.Canvas;

            switch (task.Type)
            {
                case FieldTypeEnum.TEXT:
                    {
                        TextTask textTask = task.IsA<TextTask>($"Unexpected interface detected in {nameof(GetTaskDimensions)} and expected text task. Type:{task.Type.GetType()}");

                        SelectFont(textTask.PointSize, textTask.CPI, textTask.FontName, textTask.Style, out Font font, out float fontSize);
                        SizeF requiredSize = canvas.GetStringSize(textTask.Text, font, fontSize);
                        float sizeX = requiredSize.Width;
                        float sizeY = requiredSize.Height;

                        // For certain types of task, chars are output one by one
                        if (textTask.CPI > 0)
                        {
                            // If CPI is set, set width using CPI because text is per letter to ensure the CPI is valid
                            sizeX = (Device.DotsPerInchTopX * textTask.Text.Length) / (Device.DotsPerInchBottomX * textTask.CPI);

                            // If double width, take twice as many dots per char etc
                            if (textTask.Style.HasFlag(FieldStyleEnum.QUADRUPLE))
                            {
                                sizeX *= 4;
                            }
                            else if (textTask.Style.HasFlag(FieldStyleEnum.TRIPLE))
                            {
                                sizeX *= 3;
                            }
                            else if (textTask.Style.HasFlag(FieldStyleEnum.DOUBLE))
                            {
                                sizeX *= 2;
                            }
                        }
                        else if (textTask.RowColumn &&
                                    textTask.PointSize <= 0 &&
                                    DefaultCharWidth != 0)
                        {
                            float charWidth = DefaultCharWidth;
                            // If row column based task, and no point size specified,
                            // chars will be aligned with DefaultCharWidth boundaries
                            // So set width accordingly and adjust char width for style
                            if (textTask.Style.HasFlag(FieldStyleEnum.QUADRUPLE))
                            {
                                charWidth = DefaultCharWidth * 4;
                            }
                            else if (textTask.Style.HasFlag(FieldStyleEnum.TRIPLE))
                            {
                                charWidth = DefaultCharWidth * 3;
                            }
                            else if (textTask.Style.HasFlag(FieldStyleEnum.DOUBLE))
                            {
                                charWidth = DefaultCharWidth * 2;
                            }

                            sizeX = charWidth * textTask.Text.Length;
                            sizeY = DefaultCharHeight;
                        }

                        width = (int)Math.Ceiling(sizeX);
                        height = (int)Math.Ceiling(sizeY);
                    }
                    break;
                case FieldTypeEnum.GRAPHIC:
                    {
                        GraphicTask graphicTask = task.IsA<GraphicTask>($"Unexpected interface detected in {nameof(GetTaskDimensions)} and expected graphic task. Type:{task.Type.GetType()}");
                        success = GetImageSize(graphicTask, out width, out height);
                    }
                    break;
                case FieldTypeEnum.BARCODE:
                    {
                        BarcodeTask barcodeTask = task.IsA<BarcodeTask>($"Unexpected interface detected in {nameof(GetTaskDimensions)} and expected barcode task. Type:{task.Type.GetType()}");
                        Logger.Warning(Constants.Framework, $"Barcode tasks not supported");
                    }
                    break;
                default:
                    Contracts.Fail($"Unsupported task type received. {task.Type.GetType()}");
                    break;
            }

            return success;
        }

        /// <summary>
        /// Select font and size matches defined in the XFS form
        /// </summary>
        private void SelectFont(int pointSize, int cpi, string fontName, FieldStyleEnum style, out Font font, out float requiredSize)
        {
            requiredSize = 0;
            // First find out requirements for logical font
            if (pointSize > 0)
            {
                // Calc required width in pixels. Pointsize is number of 72ths of an inch
                requiredSize = (pointSize * Device.DotsPerInchTopX) / (72 * Device.DotsPerInchBottomX);
            }

            if (cpi > 0)
            {
                // Required width is Pixels per inch / CPI round to the nearest number of dots per char
                requiredSize = Device.DotsPerInchTopX / (cpi * Device.DotsPerInchBottomX);
            }

            // If no size specified, then use default
            if (requiredSize == 0)
            {
                requiredSize = DefaultCharWidth;
            }

            // Maybe we can make this default font configurable
            string requiredFontName = "Lucida Console";

            if (!string.IsNullOrEmpty(fontName))
            {
                requiredFontName = fontName;
            }

            FontStyleType fontStyle = FontStyleType.Normal;
            int fontWeights = FontWeights.Normal;

            if (style.HasFlag(FieldStyleEnum.BOLD))
            {
                fontWeights = FontWeights.Bold;
            }
            if (style.HasFlag(FieldStyleEnum.ITALIC))
            {
                fontStyle = FontStyleType.Italic;
            }

            font = new Font(requiredFontName, fontWeights, fontStyle);
        }

        /// <summary>
        /// Maps a field's color enum to the Maui color used to draw it. FieldColorEnum is [Flags], but text is
        /// drawn in a single color, so this picks the first (lowest-bit) color set rather than combining them.
        /// </summary>
        private static Color SelectColor(FieldColorEnum color) => color switch
        {
            FieldColorEnum.WHITE => Colors.White,
            FieldColorEnum.GRAY => Colors.Gray,
            FieldColorEnum.RED => Colors.Red,
            FieldColorEnum.BLUE => Colors.Blue,
            FieldColorEnum.GREEN => Colors.Green,
            FieldColorEnum.YELLOW => Colors.Yellow,
            _ => Colors.Black,
        };

        /// <summary>
        /// The method find out the size of image to be used
        /// </summary>
        private bool GetImageSize(GraphicTask task, out int width, out int height)
        {
            bool success = true;
            width = -1;
            height = -1;

            try
            {
                using MemoryStream memStream = new(task.Image.ToArray())
                {
                    Position = 0
                };

                using SKBitmap decodedImage = SKBitmap.Decode(memStream);
                IImage imageToCopy = new SkiaImage(decodedImage);

                int bitmap_width;
                int bitmap_height;
                if (task.Scaling == FieldScalingEnum.BESTFIT)
                {
                    // Stretch size to fit field exactly
                    bitmap_width = task.Width;
                    bitmap_height = task.Height;
                }
                else if (task.Scaling == FieldScalingEnum.ASIS)
                {
                    // Display is same as bitmap width/height
                    bitmap_width = (int)imageToCopy.Width;
                    bitmap_height = (int)imageToCopy.Height;
                }
                else
                {
                    // Stretch maintaining aspect until either width or height limit of field is reached.
                    // Check if scaling to fit the width would overrun the field height.
                    if ((imageToCopy.Height * task.Width) / imageToCopy.Width > task.Height)
                    {
                        // Should scale to fit the height instead
                        bitmap_height = task.Height;
                        bitmap_width = (int)Math.Ceiling((imageToCopy.Width * task.Height) / imageToCopy.Height);
                    }
                    else
                    {
                        // Scaling to fit width should be fine
                        bitmap_height = (int)Math.Ceiling((imageToCopy.Height * task.Width) / imageToCopy.Width);
                        bitmap_width = task.Width;
                    }
                }

                width = bitmap_width <= task.Width ? bitmap_width : task.Width;
                height = bitmap_height <= task.Height ? bitmap_height : task.Height;
            }
            catch (Exception ex)
            {
                success = false;
                Logger.Warning(Constants.Framework, $"Unsupported image. {ex.Message}");
            }

            return success;
        }

        /// <summary>
        /// GetImage
        /// The method extracts pixel data and return it to the caller
        /// </summary>
        private void GetImage(SKBitmap image, int bitCount, out ImageData imageInfo)
        {
            if (bitCount != 1 &&
                bitCount != 24)
            {
                Contracts.Fail($"The framework doesn't support specified bit count for PrintToBitmap. {bitCount}");
            }

            // Multiple of 4 bytes per row
            int stride = ((image.Width * bitCount + 31) & ~31) / 8;
            List<uint> palette = [];
            //Create image buffer to convert
            byte[] pixels = new byte[stride * image.Height];

            if (bitCount == 1)
            {
                palette.Add(0xff000000);
                palette.Add(0xffffffff);

                for (int i = 0; i < image.Height; i++)
                {
                    for (int j = 0; j < image.Width; j++)
                    {
                        SKColor color = image.GetPixel(j, i);
                        // Make a vivid grayscale with perceptual luminance.
                        float y = (0.2126f * color.Red +
                                   0.7152f * color.Green +
                                   0.0722f * color.Blue) / 255f;

                        // Convert luminance to HSL grayscale
                        SKColor.FromHsl(0f, 0f, y).ToHsl(out float _, out float _, out float lightness);
                        if (lightness >= 0.5f)
                        {
                            int index = (j >> 3) + stride * i;
                            pixels[index] |= (byte)(0x80 >> (j & 0x7));
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < image.Height; i++)
                {
                    int bytePerRow = 0;
                    for (int j = 0; j < image.Width; j++)
                    {
                        SKColor color = image.GetPixel(j, i);
                        // Write the pixel in BGR format, with GDI Format24bppRgb in order to keep backwards compatibility with existing code that expects it.
                        pixels[i * stride + bytePerRow++] = color.Blue;
                        pixels[i * stride + bytePerRow++] = color.Green;
                        pixels[i * stride + bytePerRow++] = color.Red;
                    }
                }
            }

            // Copy buffer to the result.
            imageInfo = new(image.Width, image.Height, bitCount, stride, palette, [.. pixels.ToList()]);
        }

        /// <summary>
        /// PrintTextTask
        /// Print the text task to the canvas
        /// </summary>
        bool PrintTextTask(ICanvas canvas, TextTask task, int offsetX, int offsetY)
        {
            Contracts.Assert(task.Type == FieldTypeEnum.TEXT, $"Unexpected type of field to be printerd. expected text. {task.Type}");
            canvas.IsNotNull($"The method PrintToImage has not been called, but the {nameof(PrintTextTask)} is called unexpectedly.");

            bool success = GetTaskDimensions(task, out int fieldWidth, out int fieldHeight);
            if (!success)
            {
                return success;
            }

            // canvas.GetStringSize's measured height is too tight for DrawString's own ClipBounds layout --
            // passing it back as-is clips away 100% of the glyph (confirmed empirically: anything under
            // roughly 1.2x the measured height renders nothing at all). Double it for a safe margin.
            int drawHeight = fieldHeight * 2;

            Color color = SelectColor(task.Color);

            char[] text = task.Text.ToArray();
            // If CPI is set jiggle chars to get as near as possible to
            // requested CPI
            if (task.CPI > 0)
            {
                // Dots per character is Dots per inch over characters per inch
                int top = Device.DotsPerInchTopX;
                int bottom = Device.DotsPerInchBottomX * task.CPI;

                // If double width, take twice as many dots per char etc
                if (task.Style.HasFlag(FieldStyleEnum.QUADRUPLE))
                {
                    top *= 4;
                }
                else if (task.Style.HasFlag(FieldStyleEnum.TRIPLE))
                {
                    top *= 3;
                }
                else if (task.Style.HasFlag(FieldStyleEnum.DOUBLE))
                {
                    top *= 2;
                }

                for (long i=0; i< text.Length; i++)
                {
                    SelectFont(task.PointSize, task.CPI, task.FontName, task.Style, out Font font, out float fontSize);
                    canvas.Font = font;
                    canvas.FontSize = fontSize;
                    canvas.FontColor = color;
                    canvas.DrawString(
                        value: text[i].ToString(),
                        x: task.x - offsetX + (i * top) / bottom,
                        y: task.y - offsetY,
                        width: fieldWidth,
                        height: drawHeight,
                        horizontalAlignment: HorizontalAlignment.Left,
                        verticalAlignment: VerticalAlignment.Top,
                        textFlow: TextFlow.ClipBounds,
                        lineSpacingAdjustment: 0);
                }
            }
            else if (task.RowColumn &&
                     task.PointSize <= 0 &&
                     DefaultCharWidth != 0)
            {
                // If row column based task, and no point size specified,
                // ensure characters align with DefaultCharWidth boundaries if set
                // Do this by outputting character by character
                float width = DefaultCharWidth;

                // Adjust char width for style
                if (task.Style.HasFlag(FieldStyleEnum.QUADRUPLE))
                {
                    width = DefaultCharWidth * 4;
                }
                else if (task.Style.HasFlag(FieldStyleEnum.TRIPLE))
                {
                    width = DefaultCharWidth * 3;
                }
                else if (task.Style.HasFlag(FieldStyleEnum.DOUBLE))
                {
                    width = DefaultCharWidth * 2;
                }

                for (long i = 0; i < text.Length; i++)
                {
                    SelectFont(task.PointSize, task.CPI, task.FontName, task.Style, out Font font, out float fontSize);
                    canvas.Font = font;
                    canvas.FontSize = fontSize;
                    canvas.FontColor = color;
                    canvas.DrawString(
                        value: text[i].ToString(),
                        x: task.x - offsetX + i * width,
                        y: task.y - offsetY,
                        width: fieldWidth,
                        height: drawHeight,
                        horizontalAlignment: HorizontalAlignment.Left,
                        verticalAlignment: VerticalAlignment.Top,
                        textFlow: TextFlow.ClipBounds,
                        lineSpacingAdjustment: 0);
                }
            }
            else
            {
                SelectFont(task.PointSize, task.CPI, task.FontName, task.Style, out Font font, out float fontSize);
                canvas.Font = font;
                canvas.FontSize = fontSize;
                canvas.FontColor = color;
                canvas.DrawString(
                        value: task.Text,
                        x: task.x - offsetX,
                        y: task.y - offsetY,
                        width: fieldWidth,
                        height: drawHeight,
                        horizontalAlignment: HorizontalAlignment.Left,
                        verticalAlignment: VerticalAlignment.Top,
                        textFlow: TextFlow.ClipBounds,
                        lineSpacingAdjustment: 0);
            }

            return success;
        }

        /// <summary>
        /// PrintGraphicTask
        /// Print the graphic task to the canvas
        /// </summary>
        private bool PrintGraphicTask(ICanvas canvas, GraphicTask task, int offsetX, int offsetY)
        {
            Contracts.Assert(task.Type == FieldTypeEnum.GRAPHIC, $"Unexpected type of field to be printerd. expected graphic. {task.Type}");
            canvas.IsNotNull($"The method PrintToImage has not been called, but the {nameof(PrintGraphicTask)} is called unexpectedly.");

            bool success = false;

            using (MemoryStream memStream = new(task.Image.ToArray())
            {
                Position = 0
            })
            using (SKBitmap decodedImage = SKBitmap.Decode(memStream))
            {
                IImage imageToCopy = new SkiaImage(decodedImage);

                success = GetImageSize(task, out int width, out int height);
                if (success)
                {
                    try
                    {
                        canvas.DrawImage(imageToCopy, task.x - offsetX, task.y - offsetY, width, height);
                    }
                    catch (Exception ex)
                    {
                        Logger.Warning(Constants.Framework, $"Failed on writing image on the form. {ex.Message}");
                        success = false;
                    }
                }
            }

            return success;
        }

        /// <summary>
        /// PrintBarcodeTask
        /// Print the barcode task
        /// </summary>
        private bool PrintBarcodeTask(ICanvas canvas, BarcodeTask task, int offsetX, int offsetY)
        {
            Contracts.Assert(task.Type == FieldTypeEnum.BARCODE, $"Unexpected type of field to be printerd. expected barcode. {task.Type}");
            canvas.IsNotNull($"The method PrintToImage has not been called, but the {nameof(PrintBarcodeTask)} is called unexpectedly.");

            // Not supported
            Contracts.Fail($"The barcode task is not supported.");
            return false;
        }

        /// <summary>
        /// Default size of the character
        /// </summary>
        private float DefaultCharWidth { get; set; } = 0;
        private float DefaultCharHeight { get; set; } = 0;

        /// <summary>
        /// Device specific class
        /// </summary>
        private readonly IPrinterDevice Device;

        /// <summary>
        /// Logging information
        /// </summary>
        private readonly ILogger Logger;
    }
}
