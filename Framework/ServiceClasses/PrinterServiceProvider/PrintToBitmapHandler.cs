using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.IO;
using XFS4IoT;
using XFS4IoTFramework.Printer;

using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;


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
        [SupportedOSPlatform("windows")]
        public bool Convert(PrintJobClass job, int bitCount, bool UpsideDown, out ImageInfo imageInfo)
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

            // Initialise our bitmap to the size of the rectangle
            using (Bitmap image = new(currentWidth, currentHeight, PixelFormat.Format32bppRgb))
            {
                using Graphics graphics = Graphics.FromImage(image);
                graphics.Clear(Color.White);

                // Print all tasks to the bitmap
                foreach (PrintTask task in job.Tasks)
                {
                    switch (task.Type)
                    {
                        case FieldTypeEnum.TEXT:
                            success = PrintTextTask(graphics, task.IsA<TextTask>($"Unexpected task. {task.Type}"), offsetX, offsetY);
                            break;
                        case FieldTypeEnum.GRAPHIC:
                            success = PrintGraphicTask(graphics, task.IsA<GraphicTask>($"Unexpected task. {task.Type}"), offsetX, offsetY);
                            break;
                        case FieldTypeEnum.BARCODE:
                            success = PrintBarcodeTask(graphics, task.IsA<BarcodeTask>($"Unexpected task. {task.Type}"), offsetX, offsetY);
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

                if (job.Orientation == FormOrientationEnum.LANDSCAPE)
                {
                    // Change orientation
                    image.RotateFlip(RotateFlipType.Rotate270FlipXY);
                }

                GetImage(image, bitCount, out ImageData imageData);

                imageInfo = new(offsetX, offsetY, imageData);
            }

            return success;
        }

        /// <summary>
        /// This method can be called in the device class to obtain the dimensions of a KXTask when printed using PrintToImage
        /// </summary>
        [SupportedOSPlatform("windows")]
        public bool GetTaskDimensions(PrintTask task, out int width, out int height)
        {
            task.IsNotNull($"An empty task passed in the {nameof(GetTaskDimensions)}");
            width = -1;
            height = -1;
            bool success = true;

            using (Bitmap image = new(1, 1, PixelFormat.Format32bppRgb))
            {
                switch (task.Type)
                {
                    case FieldTypeEnum.TEXT:
                        {
                            TextTask textTask = task.IsA<TextTask>($"Unexpected interface detected in {nameof(GetTaskDimensions)} and expected text task. Type:{task.Type.GetType()}");

                            using Graphics graphics = Graphics.FromImage(image);
                            SizeF requiredSize = graphics.MeasureString(textTask.Text, SelectFont(textTask.PointSize, textTask.CPI, textTask.FontName, textTask.Style));
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

                            width = (int)sizeX;
                            height = (int)sizeY;
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
            }

            return success;
        }

        /// <summary>
        /// Select font and size matches defined in the XFS form
        /// </summary>
        [SupportedOSPlatform("windows")]
        private Font SelectFont(int pointSize, int cpi, string fontName, FieldStyleEnum style)
        {
            float requiredSize = 0;
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

            FontStyle fontStyle = FontStyle.Regular;
            if (style.HasFlag(FieldStyleEnum.BOLD))
            {
                fontStyle |= FontStyle.Bold;
            }
            if (style.HasFlag(FieldStyleEnum.ITALIC))
            {
                fontStyle |= FontStyle.Italic;
            }
            if (style.HasFlag(FieldStyleEnum.UNDER))
            {
                fontStyle |= FontStyle.Underline;
            }

            // Check all installed font and select best match
            InstalledFontCollection fontInstalled = new();
            bool fontFound = false;
            foreach (var font in fontInstalled.Families)
            {
                if (font.Name.Equals(requiredFontName, StringComparison.CurrentCultureIgnoreCase))
                {
                    fontFound = true;
                    break;
                }
            }

            if (!fontFound)
            {
                // Use one of monospace font available
                FontFamily defaultMono = FontFamily.GenericMonospace;
                defaultMono.IsNotNull($"Failed to find default monospace font.");
                requiredFontName = defaultMono.Name;
                defaultMono.Dispose();

                Logger.Warning(Constants.Framework, $"Specified font '{fontName}' is not found. Use {requiredFontName} instead.");
            }

            return new Font(requiredFontName, requiredSize, fontStyle, GraphicsUnit.Pixel);
        }

        /// <summary>
        /// The method find out the size of image to be used
        /// </summary>
        [SupportedOSPlatform("windows")]
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
                using Bitmap imageToCopy = new(memStream);

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
                    bitmap_width = imageToCopy.Width;
                    bitmap_height = imageToCopy.Height;
                }
                else
                {
                    // Stretch maintaining aspect until either width or height limit of field is reached.
                    // Check if scaling to fit the width would overrun the field height
                    if ((imageToCopy.Height * task.Width) / task.Width > task.Height)
                    {
                        // Should scale to fit the height instead
                        bitmap_height = task.Height;
                        bitmap_width = (imageToCopy.Width * task.Height) / imageToCopy.Height;
                    }
                    else
                    {
                        // Scaling to fit width should be fine
                        bitmap_height = (imageToCopy.Height * task.Width) / imageToCopy.Width;
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
        [SupportedOSPlatform("windows")]
        private void GetImage(Bitmap image, int bitCount, out ImageData imageInfo)
        {
            imageInfo = null;

            PixelFormat pixelFormat = bitCount switch
            {
                 1 => PixelFormat.Format1bppIndexed,
                24 => PixelFormat.Format24bppRgb,
                32 => PixelFormat.Format32bppRgb,
                _  => PixelFormat.DontCare
            };

            Contracts.Assert(pixelFormat != PixelFormat.DontCare, $"The framework doesn't support specified bit count. {bitCount}");

            int stride = ((image.Width * bitCount + 31) & ~31) / 8;
            List<uint> palette = new();
            //Create image buffer to convert
            byte[] pixels = new byte[stride * image.Height];
            if (pixelFormat == PixelFormat.Format1bppIndexed)
            {
                palette.Add(0xff000000);
                palette.Add(0xffffffff);

                for (int i = 0; i < image.Height; i++)
                {
                    for (int j = 0; j < image.Width; j++)
                    {
                        if (image.GetPixel(j, i).GetBrightness() >= 0.5f)
                        {
                            int index = (j >> 3) + stride * i;
                            pixels[index] |= (byte)(0x80 >> (j & 0x7));
                        }
                    }
                }
            }
            else
            {
                BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, pixelFormat);
                Marshal.Copy(data.Scan0, pixels, 0, pixels.Length);
                image.UnlockBits(data);
            }

            // Copy buffer to the result. 
            imageInfo = new(image.Width, image.Height, bitCount, stride, palette, new(pixels.ToList()));
        }

        /// <summary>
        /// PrintTextTask
        /// Print the text task to draw string using a Graphics object
        /// </summary>
        [SupportedOSPlatform("windows")]
        bool PrintTextTask(Graphics graphics, TextTask task, int offsetX, int offsetY)
        {
            Contracts.Assert(task.Type == FieldTypeEnum.TEXT, $"Unexpected type of field to be printerd. expected text. {task.Type}");
            graphics.IsNotNull($"The method PrintToImage has not been called, but the {nameof(PrintTextTask)} is called unexpectedly.");

            FontStyle fontStyle = FontStyle.Regular;
            if (task.Style.HasFlag(FieldStyleEnum.BOLD))
            {
                fontStyle |= FontStyle.Bold;
            }
            if (task.Style.HasFlag(FieldStyleEnum.ITALIC))
            {
                fontStyle |= FontStyle.Italic;
            }

            bool success = true;
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
                    graphics.DrawString(text[i].ToString(), SelectFont(task.PointSize, task.CPI, task.FontName, task.Style), Brushes.Black, task.x - offsetX + (i * top) / bottom, task.y - offsetY);
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
                    graphics.DrawString(text[i].ToString(), SelectFont(task.PointSize, task.CPI, task.FontName, task.Style), Brushes.Black, task.x - offsetX + i * width, task.y - offsetY);
                }
            }
            else
            {
                graphics.DrawString(task.Text, SelectFont(task.PointSize, task.CPI, task.FontName, task.Style), Brushes.Black, task.x - offsetX, task.y - offsetY);
            }

            return success;
        }

        /// <summary>
        /// PrintGraphicTask
        /// Print the graphic task to draw image using a Graphics object
        /// </summary>
        [SupportedOSPlatform("windows")]
        private bool PrintGraphicTask(Graphics graphics, GraphicTask task, int offsetX, int offsetY)
        {
            Contracts.Assert(task.Type == FieldTypeEnum.GRAPHIC, $"Unexpected type of field to be printerd. expected graphic. {task.Type}");
            graphics.IsNotNull($"The method PrintToImage has not been called, but the {nameof(PrintGraphicTask)} is called unexpectedly.");

            bool success = false;

            using (MemoryStream memStream = new(task.Image.ToArray())
            {
                Position = 0
            })
            {
                using Bitmap imageToCopy = new(memStream);

                success = GetImageSize(task, out int width, out int height);

                if (success)
                {
                    try
                    {
                        graphics.DrawImage(imageToCopy, task.x - offsetX, task.y - offsetY, width, height);
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
        [SupportedOSPlatform("windows")]
        private bool PrintBarcodeTask(Graphics graphics, BarcodeTask task, int offsetX, int offsetY)
        {
            Contracts.Assert(task.Type == FieldTypeEnum.BARCODE, $"Unexpected type of field to be printerd. expected barcode. {task.Type}");
            graphics.IsNotNull($"The method PrintToImage has not been called, but the {nameof(PrintBarcodeTask)} is called unexpectedly.");

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
