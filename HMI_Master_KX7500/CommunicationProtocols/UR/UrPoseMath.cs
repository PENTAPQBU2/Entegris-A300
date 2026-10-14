using System;
using System.Globalization;

namespace HMI_Master_KX7500.CommunicationProtocols.UR
{
    //UR pose = [x, y, z, rx, ry, rz] in metres and radians, rotation as a rotation vector (axis * angle)
    public static class UrPoseMath
    {
        public const double MmToM = 0.001;
        public const double DegToRad = Math.PI / 180.0;

        public static double[,] RotVecToMatrix(double rx, double ry, double rz)
        {
            double angle = Math.Sqrt(rx * rx + ry * ry + rz * rz);
            if(angle < 1e-12)
            {
                return new double[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
            }

            double kx = rx / angle, ky = ry / angle, kz = rz / angle;
            double c = Math.Cos(angle), s = Math.Sin(angle), v = 1 - c;

            return new double[,]
            {
                { kx * kx * v + c,      kx * ky * v - kz * s, kx * kz * v + ky * s },
                { ky * kx * v + kz * s, ky * ky * v + c,      ky * kz * v - kx * s },
                { kz * kx * v - ky * s, kz * ky * v + kx * s, kz * kz * v + c }
            };
        }

        public static double[] MatrixToRotVec(double[,] r)
        {
            double cosAngle = (r[0, 0] + r[1, 1] + r[2, 2] - 1) / 2;
            cosAngle = Math.Max(-1.0, Math.Min(1.0, cosAngle));
            double angle = Math.Acos(cosAngle);

            if(angle < 1e-9)
            {
                return new double[] { 0, 0, 0 };
            }

            if(Math.PI - angle < 1e-6)
            {
                //180 degrees: axis from the diagonal, sign taken from the largest component
                double x = Math.Sqrt(Math.Max(0, (r[0, 0] + 1) / 2));
                double y = Math.Sqrt(Math.Max(0, (r[1, 1] + 1) / 2));
                double z = Math.Sqrt(Math.Max(0, (r[2, 2] + 1) / 2));
                if(x >= y && x >= z)
                {
                    y = Math.Sign(r[0, 1]) * y;
                    z = Math.Sign(r[0, 2]) * z;
                }
                else if(y >= z)
                {
                    x = Math.Sign(r[0, 1]) * x;
                    z = Math.Sign(r[1, 2]) * z;
                }
                else
                {
                    x = Math.Sign(r[0, 2]) * x;
                    y = Math.Sign(r[1, 2]) * y;
                }
                return new double[] { x * angle, y * angle, z * angle };
            }

            double k = angle / (2 * Math.Sin(angle));
            return new double[]
            {
                (r[2, 1] - r[1, 2]) * k,
                (r[0, 2] - r[2, 0]) * k,
                (r[1, 0] - r[0, 1]) * k
            };
        }

        public static double[,] Multiply(double[,] a, double[,] b)
        {
            double[,] m = new double[3, 3];
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    m[i, j] = a[i, 0] * b[0, j] + a[i, 1] * b[1, j] + a[i, 2] * b[2, j];
                }
            }
            return m;
        }

        //Target pose for one jog step.
        //axis 0..2 = X, Y, Z (step in metres), axis 3..5 = Rx, Ry, Rz (step in radians, rotation about the TCP).
        //toolFrame false = directions of the robot base, true = directions of the tool.
        public static double[] JogTarget(double[] pose, int axis, double step, bool toolFrame)
        {
            if(pose == null || pose.Length != 6) { throw new ArgumentException("pose must have 6 values"); }
            if(axis < 0 || axis > 5) { throw new ArgumentOutOfRangeException(nameof(axis)); }

            double[] target = (double[])pose.Clone();
            double[,] r = RotVecToMatrix(pose[3], pose[4], pose[5]);

            if(axis < 3)
            {
                if(toolFrame)
                {
                    target[0] += r[0, axis] * step;
                    target[1] += r[1, axis] * step;
                    target[2] += r[2, axis] * step;
                }
                else
                {
                    target[axis] += step;
                }
                return target;
            }

            double[] unit = new double[3];
            unit[axis - 3] = step;
            double[,] delta = RotVecToMatrix(unit[0], unit[1], unit[2]);
            double[,] rNew = toolFrame ? Multiply(r, delta) : Multiply(delta, r);
            double[] rv = MatrixToRotVec(rNew);
            target[3] = rv[0];
            target[4] = rv[1];
            target[5] = rv[2];
            return target;
        }

        public static string ToUrScript(double[] pose)
        {
            return string.Format(CultureInfo.InvariantCulture, "p[{0:R}, {1:R}, {2:R}, {3:R}, {4:R}, {5:R}]",
                pose[0], pose[1], pose[2], pose[3], pose[4], pose[5]);
        }
    }
}
