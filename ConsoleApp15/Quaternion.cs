using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp15
{
    internal class Quaternion
    {
        public double W { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Quaternion(double w, double x, double y, double z)
        {
            W = w;
            X = x;
            Y = y;
            Z = z;
        }

       
        public static Quaternion operator +(Quaternion a, Quaternion b)
        {
            return new Quaternion(a.W + b.W, a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Quaternion operator -(Quaternion a, Quaternion b)
        {
            return new Quaternion(a.W - b.W, a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Quaternion operator *(Quaternion a, Quaternion b)
        {
            double w = a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z;
            double x = a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y;
            double y = a.W * b.Y - a.X * b.Z + a.Y * b.W + a.Z * b.X;
            double z = a.W * b.Z + a.X * b.Y - a.Y * b.X + a.Z * b.W;

            return new Quaternion(w, x, y, z);
        }

        
        public double Norm()
        {
            return Math.Sqrt(W * W + X * X + Y * Y + Z * Z);
        }

        public Quaternion Conjugate()
        {
            return new Quaternion(W, -X, -Y, -Z);
        }

        public Quaternion Inverse()
        {
            double normSquared = W * W + X * X + Y * Y + Z * Z;
            if (normSquared == 0)
            {
                throw new InvalidOperationException("Quaternion has zero norm, cannot compute inverse.");
            }

            double inverseNorm = 1.0 / normSquared;
            return new Quaternion(W * inverseNorm, -X * inverseNorm, -Y * inverseNorm, -Z * inverseNorm);
        }

      
        public static bool operator ==(Quaternion a, Quaternion b)
        {
            return a.W == b.W && a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Quaternion a, Quaternion b)
        {
            return !(a == b);
        }

        
        public Matrix3x3 ToRotationMatrix()
        {
            double xx = X * X;
            double xy = X * Y;
            double xz = X * Z;
            double xw = X * W;

            double yy = Y * Y;
            double yz = Y * Z;
            double yw = Y * W;

            double zz = Z * Z;
            double zw = Z * W;

            return new Matrix3x3(
                1 - 2 * (yy + zz), 2 * (xy - zw), 2 * (xz + yw),
                2 * (xy + zw), 1 - 2 * (xx + zz), 2 * (yz - xw),
                2 * (xz - yw), 2 * (yz + xw), 1 - 2 * (xx + yy)
            );
        }

        public static Quaternion FromRotationMatrix(Matrix3x3 rotationMatrix)
        {
            double trace = rotationMatrix.M11 + rotationMatrix.M22 + rotationMatrix.M33;

            if (trace > 0)
            {
                double s = 0.5 / Math.Sqrt(trace + 1.0);
                double w = 0.25 / s;
                double x = (rotationMatrix.M32 - rotationMatrix.M23) * s;
                double y = (rotationMatrix.M13 - rotationMatrix.M31) * s;
                double z = (rotationMatrix.M21 - rotationMatrix.M12) * s;

                return new Quaternion(w, x, y, z);
            }
            else if (rotationMatrix.M11 > rotationMatrix.M22 && rotationMatrix.M11 > rotationMatrix.M33)
            {
                double s = 2.0 * Math.Sqrt(1.0 + rotationMatrix.M11 - rotationMatrix.M22 - rotationMatrix.M33);
                double w = (rotationMatrix.M32 - rotationMatrix.M23) / s;
                double x = 0.25 * s;
                double y = (rotationMatrix.M12 + rotationMatrix.M21) / s;
                double z = (rotationMatrix.M13 + rotationMatrix.M31) / s;

                return new Quaternion(w, x, y, z);
            }
            else if (rotationMatrix.M22 > rotationMatrix.M33)
            {
                double s = 2.0 * Math.Sqrt(1.0 + rotationMatrix.M22 - rotationMatrix.M11 - rotationMatrix.M33);
                double w = (rotationMatrix.M13 - rotationMatrix.M31) / s;
                double x = (rotationMatrix.M12 + rotationMatrix.M21) / s;
                double y = 0.25 * s;
                double z = (rotationMatrix.M23 + rotationMatrix.M32) / s;

                return new Quaternion(w, x, y, z);
            }
            else
            {
                double s = 2.0 * Math.Sqrt(1.0 + rotationMatrix.M33 - rotationMatrix.M11 - rotationMatrix.M22);
                double w = (rotationMatrix.M21 - rotationMatrix.M12) / s;
                double x = (rotationMatrix.M13 + rotationMatrix.M31) / s;
                double y = (rotationMatrix.M23 + rotationMatrix.M32) / s;
                double z = 0.25 * s;

                return new Quaternion(w, x, y, z);
            }
        }
    }
}  
        
        
    

