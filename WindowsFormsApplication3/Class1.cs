using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication3
{
    public class Deskew
    {
        // Representation of a line in the image.  
        private class HougLine
        {
            // Count of points in the line.
            public int Count;
            // Index in Matrix.
            public int Index;
            // The line is represented as all x,y that solve y*cos(alpha)-x*sin(alpha)=d
            public double Alpha;
        }


        // The Bitmap
        public Bitmap _internalBmp;

        // The range of angles to search for lines
        const double ALPHA_START = -20;
        const double ALPHA_STEP = 0.2;
        const int STEPS = 40 * 5;
        const double STEP = 1;

        // Precalculation of sin and cos.
        double[] _sinA;
        double[] _cosA;

        // Range of d
        double _min;


        int _count;
        // Count of points that fit in a line.
        int[] _hMatrix;

        // Calculate the skew angle of the image cBmp.
        public double GetSkewAngle()
        {
            // Hough Transformation
            Calc();

            // Top 20 of the detected lines in the image.
            HougLine[] hl = GetTop(20);

            // Average angle of the lines
            double sum = 0;
            int count = 0;
            for (int i = 0; i <= 19; i++)
            {
                sum += hl[i].Alpha;
                count += 1;
            }
            return sum / count;
        }

        // Calculate the Count lines in the image with most points.
        private HougLine[] GetTop(int count)
        {
            HougLine[] hl = new HougLine[count];

            for (int i = 0; i <= count - 1; i++)
            {
                hl[i] = new HougLine();
            }
            for (int i = 0; i <= _hMatrix.Length - 1; i++)
            {
                if (_hMatrix[i] > hl[count - 1].Count)
                {
                    hl[count - 1].Count = _hMatrix[i];
                    hl[count - 1].Index = i;
                    int j = count - 1;
                    while (j > 0 && hl[j].Count > hl[j - 1].Count)
                    {
                        HougLine tmp = hl[j];
                        hl[j] = hl[j - 1];
                        hl[j - 1] = tmp;
                        j -= 1;
                    }
                }
            }

            for (int i = 0; i <= count - 1; i++)
            {
                int dIndex = hl[i].Index / STEPS;
                int alphaIndex = hl[i].Index - dIndex * STEPS;
                hl[i].Alpha = GetAlpha(alphaIndex);
                //hl[i].D = dIndex + _min;
            }

            return hl;
        }


        // Hough Transforamtion:
        private void Calc()
        {
            int hMin = _internalBmp.Height / 4;
            int hMax = _internalBmp.Height * 3 / 4;

            Init();
            for (int y = hMin; y <= hMax; y++)
            {
                for (int x = 1; x <= _internalBmp.Width - 2; x++)
                {
                    // Only lower edges are considered.
                    if (IsBlack(x, y))
                    {
                        if (!IsBlack(x, y + 1))
                        {
                            Calc(x, y);
                        }
                    }
                }
            }
        }

        // Calculate all lines through the point (x,y).
        private void Calc(int x, int y)
        {
            int alpha;

            for (alpha = 0; alpha <= STEPS - 1; alpha++)
            {
                double d = y * _cosA[alpha] - x * _sinA[alpha];
                int calculatedIndex = (int)CalcDIndex(d);
                int index = calculatedIndex * STEPS + alpha;
                try
                {
                    _hMatrix[index] += 1;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
        }
        private double CalcDIndex(double d)
        {
            return Convert.ToInt32(d - _min);
        }
        private bool IsBlack(int x, int y)
        {
            Color c = _internalBmp.GetPixel(x, y);
            double luminance = (c.R * 0.299) + (c.G * 0.587) + (c.B * 0.114);
            return luminance < 140;
        }

        private void Init()
        {
            // Precalculation of sin and cos.
            _cosA = new double[STEPS];
            _sinA = new double[STEPS];

            for (int i = 0; i < STEPS; i++)
            {
                double angle = GetAlpha(i) * Math.PI / 180.0;
                _sinA[i] = Math.Sin(angle);
                _cosA[i] = Math.Cos(angle);
            }

            // Range of d:            
            _min = -_internalBmp.Width;
            _count = (int)(2 * (_internalBmp.Width + _internalBmp.Height) / STEP);
            _hMatrix = new int[_count * STEPS];

        }

        private static double GetAlpha(int index)
        {
            return ALPHA_START + index * ALPHA_STEP;
        }
    
    public static void DeskewImage(string fileName, byte binarizeThreshold)
        {
            ////打开图像
            //Bitmap bmp = OpenImage(fileName);
 
            //Deskew deskew = new Deskew();
            //Bitmap tempBmp = CropImage(bmp, bmp.Width / 4, bmp.Height / 4, bmp.Width / 2, bmp.Height / 2);
            //deskew._internalBmp = BinarizeImage(tempBmp, binarizeThreshold);
            //double angle = deskew.GetSkewAngle();
            //bmp = RotateImage(bmp, (float)(-angle));
 
            ////保存图像(需要再还原图片原本的位深度)
            //SaveImage(bmp, fileName);
        }
 
        /// <summary>
        /// 图像剪切
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="StartX"></param>
        /// <param name="StartY"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        private static Bitmap CropImage(Bitmap bmp, int StartX, int StartY, int w, int h)
        {
            try
            {
                Bitmap bmpOut = new Bitmap(w, h, PixelFormat.Format32bppArgb);
 
                Graphics g = Graphics.FromImage(bmpOut);
                g.DrawImage(bmp, new Rectangle(0, 0, w, h), new Rectangle(StartX, StartY, w, h), GraphicsUnit.Pixel);
                g.Dispose();
 
                return bmpOut;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 图像二值化
        /// </summary>
        /// <param name="b"></param>
        /// <param name="threshold">阈值</param>
        private static Bitmap BinarizeImage(Bitmap b, byte threshold)
        {
            int width = b.Width;
            int height = b.Height;
            BitmapData data = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
            unsafe
            {
                byte * p = (byte*)data.Scan0;
                int offset = data.Stride - width * 4;
                byte R, G, B, gray;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        R = p[2];
                        G = p[1];
                        B = p[0];
                        gray = (byte)((R * 19595 + G * 38469 + B * 7472) >> 16);
                        if (gray >= threshold)
                        {
                            p[0] = p[1] = p[2] = 255;
                        }
                        else
                        {
                            p[0] = p[1] = p[2] = 0;
                        }
                        p += 4;
                    }
                    p += offset;
                }
                b.UnlockBits(data);
                return b;
            }
        }
 
        /// <summary>
        /// 图像旋转
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="angle">角度</param>
        /// <returns></returns>
        private static Bitmap RotateImage(Bitmap bmp, float angle)
        {
            PixelFormat pixelFormat = bmp.PixelFormat;
            PixelFormat pixelFormatOld = pixelFormat;
            if (bmp.Palette.Entries.Count() > 0)
            {
                pixelFormat = PixelFormat.Format24bppRgb;
            }
 
            Bitmap tmpBitmap = new Bitmap(bmp.Width, bmp.Height, pixelFormat);
            tmpBitmap.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);
            Graphics g = Graphics.FromImage(tmpBitmap);
            try
            {
                g.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);
                g.RotateTransform(angle);
                g.DrawImage(bmp, 0, 0);
            }
            catch
            {
            }
            finally
            {
                g.Dispose();
            }
 
            if (pixelFormatOld == PixelFormat.Format8bppIndexed) tmpBitmap = CopyTo8bpp(tmpBitmap);
            else if (pixelFormatOld == PixelFormat.Format1bppIndexed) tmpBitmap = CopyTo1bpp(tmpBitmap);
 
            return tmpBitmap;
        }
        private static Bitmap CopyTo1bpp(Bitmap b)
        {
            int w = b.Width, h = b.Height; Rectangle r = new Rectangle(0, 0, w, h);
            if (b.PixelFormat != PixelFormat.Format32bppPArgb)
            {
                Bitmap temp = new Bitmap(w, h, PixelFormat.Format32bppPArgb);
                temp.SetResolution(b.HorizontalResolution, b.VerticalResolution);
                Graphics g = Graphics.FromImage(temp);
                g.DrawImage(b, r, 0, 0, w, h, GraphicsUnit.Pixel);
                g.Dispose();
                b = temp;
            }
            BitmapData bdat = b.LockBits(r, ImageLockMode.ReadOnly, b.PixelFormat);
            Bitmap b0 = new Bitmap(w, h, PixelFormat.Format1bppIndexed);
            b0.SetResolution(b.HorizontalResolution, b.VerticalResolution);
            BitmapData b0dat = b0.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int index = y * bdat.Stride + (x * 4);
                    if (Color.FromArgb(Marshal.ReadByte(bdat.Scan0, index + 2), Marshal.ReadByte(bdat.Scan0, index + 1), Marshal.ReadByte(bdat.Scan0, index)).GetBrightness() > 0.5f)
                    {
                        int index0 = y * b0dat.Stride + (x >> 3);
                        byte p = Marshal.ReadByte(b0dat.Scan0, index0);
                        byte mask = (byte)(0x80 >> (x & 0x7));
                        Marshal.WriteByte(b0dat.Scan0, index0, (byte)(p | mask));
                    }
                }
            }
            b0.UnlockBits(b0dat);
            b.UnlockBits(bdat);
            return b0;
        }

        private static Bitmap CopyTo8bpp(Bitmap bmp)
        {
            if (bmp == null) return null;

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);

            int width = bmpData.Width;
            int height = bmpData.Height;
            int stride = bmpData.Stride;
            int offset = stride - width * 3;
            IntPtr ptr = bmpData.Scan0;
            int scanBytes = stride * height;

            int posScan = 0, posDst = 0;
            byte[] rgbValues = new byte[scanBytes];
            Marshal.Copy(ptr, rgbValues, 0, scanBytes);
            byte[] grayValues = new byte[width * height];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    double temp = rgbValues[posScan++] * 0.11 +
                        rgbValues[posScan++] * 0.59 +
                        rgbValues[posScan++] * 0.3;
                    grayValues[posDst++] = (byte)temp;
                }
                posScan += offset;
            }

            Marshal.Copy(rgbValues, 0, ptr, scanBytes);
            bmp.UnlockBits(bmpData);

            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            bitmap.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

            int offset0 = bitmapData.Stride - bitmapData.Width;
            int scanBytes0 = bitmapData.Stride * bitmapData.Height;
            byte[] rawValues = new byte[scanBytes0];

            int posSrc = 0;
            posScan = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    rawValues[posScan++] = grayValues[posSrc++];
                }
                posScan += offset0;
            }

            Marshal.Copy(rawValues, 0, bitmapData.Scan0, scanBytes0);
            bitmap.UnlockBits(bitmapData);

            ColorPalette palette;
            using (Bitmap bmp0 = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
            {
                palette = bmp0.Palette;
            }
            for (int i = 0; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(i, i, i);
            }
            bitmap.Palette = palette;

            return bitmap;
        }


    }
}
