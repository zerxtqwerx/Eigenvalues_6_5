using System;
using System.Collections.Generic;
using System.Linq;

namespace EigenValueCalculation
{
    class Program
    {
        static double[][] InputMatrix(int n)
        {
            double[][] A = new double[n][];
            Console.WriteLine($"Введите элементы матрицы размером {n}x{n}:");
            for (int i = 0; i < n; i++)
            {
                while (true)
                {
                    Console.Write($"Строка {i + 1} (через пробел): ");
                    string input = Console.ReadLine();
                    double[] row = input.Split(' ').Select(double.Parse).ToArray();
                    if (row.Length == n)
                    {
                        A[i] = row;
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка: ожидается {n} элементов. Попробуйте еще раз.");
                    }
                }
            }
            return A;
        }

        static double DotProduct(double[] v1, double[] v2)
        {
            return v1.Zip(v2, (a, b) => a * b).Sum();
        }

        static double Norm(double[] vector)
        {
            return Math.Sqrt(vector.Sum(x => x * x));
        }

        static double[] MatrixVectorProduct(double[][] A, double[] v)
        {
            return A.Select(row => DotProduct(row, v)).ToArray();
        }

        static (double eigenvalue, double[] eigenvector, int iterations) PowerIteration(double[][] A, double tol = 1e-10, int maxIterations = 100)
        {
            int n = A.Length;

            double[] b_k = new double[n];
            for (int i = 0; i < n; i++)
                b_k[i] = 1.0;

            int iterations = 0;

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                double[] b_k_prev = (double[])b_k.Clone();

                b_k = MatrixVectorProduct(A, b_k);


                double norm_b_k = Norm(b_k);
                for (int i = 0; i < n; i++)
                    b_k[i] /= norm_b_k;

                double eigenvalue = DotProduct(b_k, MatrixVectorProduct(A, b_k));

                iterations++;

                if (Norm(b_k.Zip(b_k_prev, (x, y) => x - y).ToArray()) < tol)
                    return (eigenvalue, b_k, iterations);
            }

            return (0, null, iterations); 
        }

        static void Main(string[] args)
        {
            Console.Write("Введите размерность матрицы: ");
            int n = int.Parse(Console.ReadLine());

            double[][] A = InputMatrix(n); 

            double tolerance = 1e-10;
            var (eigenvalue, eigenvector, iterCount) = PowerIteration(A, tol: tolerance);

            Console.WriteLine($"Собственное значение: {eigenvalue}");
            Console.WriteLine($"Собственный вектор: {string.Join(", ", eigenvector)}");
            Console.WriteLine($"Количество итераций, при которых достигнута заданная точность: {iterCount}");
            Console.Read();
        }
    }
}