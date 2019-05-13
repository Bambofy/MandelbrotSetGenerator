using System;
using System.Numerics;
using System.Drawing;

namespace MandelbrotSetGenerator
{
    class Program
    {
        /// <summary>
        /// This defines how many iterations, of oribt 0 for a given x^2 + c to complete.
        /// Points within the Mandelbrot Set do not tend towards infinity but have a cyclic pattern.
        /// Points outside of the Mandelbrot Set tends towards infinity the greater the iteration number.
        /// </summary>
        private static int MAX_ITERATION = 5; // how many iterations in the orbit of 0 to complete.

        /// <summary>
        /// What we are going to assume to be the threshold value to say that a value is trending towards infinity.
        /// This could be improved by performing statistical measures on values.
        /// </summary>
        private static double MAX_INF_VALUE = 4.0;

        static void Main(string[] args)
        {
            Complex c = new Complex(1.0, 1.0);

            /// Image proprerties.
            int ImageXMin = 0;
            int ImageXMax = 800;
            int ImageYMin = 0;
            int ImageYMax = 800;

            /// Create bitmap and clear the image to Black (which represents points IN the Mandelbrot set).
            /// We clear to clear to black because it's easier to tell which points are probably tending towards infinity.
            Bitmap mandelbrotBmp = new Bitmap(ImageXMax, ImageYMax);
            for (int Xpixel = ImageXMin; Xpixel < ImageXMax; Xpixel++)
            {
                for (int Ypixel = ImageYMin; Ypixel < ImageYMax; Ypixel++)
                {
                    mandelbrotBmp.SetPixel(Xpixel, Ypixel, Color.Black);
                }
            }

            /// Limits of the REAL axis. (x axis).
            double Xmin = -2.0;
            double Xmax = 1;
            double Xstep = 0.001;

            int numberOfXSteps = (int)Math.Floor((Xmax - Xmin) / Xstep);

            /// Limits of the IMAGINARY axis. (y axis).
            double Ymin = -1.0;
            double Ymax = 1.0;
            double Ystep = 0.001;

            /// For each point in the complex plane defined above...
            for (double Xreal = Xmin; Xreal < Xmax; Xreal += Xstep)
            {
                for (double Yimag = Ymin; Yimag < Ymax; Yimag += Ystep)
                {
                    Complex point = new Complex(Xreal, Yimag);

                    /// Does this point, when iterated using the orbit 0, tend towards infinity?
                    Complex previousIteration = 0;
                    for (int iter = 0; iter < MAX_ITERATION; iter++)
                    {
                        /// current iteration value = previousIteration value^2 + original point.
                        Complex currentIteration = Complex.Pow(previousIteration, 2) + point;

                        /// Size of the complex number.
                        double mag = currentIteration.Magnitude;

                        /// Value of 4 is assumed to be an infinite value.
                        if (mag > MAX_INF_VALUE)
                        {
                            /// map the current complex plane coordinate to catesian.
                            int pixelCoordinateX = (int)Xreal.Remap(Xmin, Xmax, ImageXMin, ImageXMax);
                            int pixelCoordinateY = (int)Yimag.Remap(Ymin, Ymax, ImageYMin, ImageYMax);

                            /// this point is assumed to be trending towards 0 therefore we colour it WHITE.
                            mandelbrotBmp.SetPixel(pixelCoordinateX, pixelCoordinateY, Color.White);

                            break;
                        }

                        previousIteration = currentIteration;
                    }
                }
            }

            mandelbrotBmp.Save("mandelbrotSet.bmp");
        }
    }

    public static class ExtensionMethods
    {
        public static double Remap(this double from, double fromMin, double fromMax, double toMin, double toMax)
        {
            var fromAbs = from - fromMin;
            var fromMaxAbs = fromMax - fromMin;

            var normal = fromAbs / fromMaxAbs;

            var toMaxAbs = toMax - toMin;
            var toAbs = toMaxAbs * normal;

            var to = toAbs + toMin;

            return to;
        }
    }

}