using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace HorrorShorts_Game.Algorithms.Tweener
{
    public static class TweenFunctions
    {
        //Linear
        public static float Linear(float value) => value;


        //Exponencial
        private const int BASE_POW = 1000;
        private static float ExpIn(float value, float basePow)
        {
            float mod = 0.01f * (100f / basePow);
            return Math.Clamp(MathF.Pow(basePow, -(1f - value)) - mod, 0f, 1f);
        }
        private static float ExpOut(float value, float basePow)
        {
            float mod = 0.01f * (100f / basePow);
            return Math.Clamp(1f - MathF.Pow(basePow, -value) + mod, 0f, 1f);
        }
        private static float ExpInOut(float value, float basePow)
        {
            if (value < 0.5f) return ExpIn(value * 2f, basePow) / 2f;
            else return 0.5f + ExpOut((value - 0.5f) * 2f, basePow) / 2f;
        }

        public static float ExpIn(float value) => ExpIn(value, BASE_POW);
        public static float ExpOut(float value) => ExpOut(value, BASE_POW);
        public static float ExpInOut(float value) => ExpInOut(value, BASE_POW);

        public static Func<float, float> ExpIn_CreateCustom(float basePow) => new((a) => ExpIn(a, basePow));
        public static Func<float, float> ExpOut_CreateCustom(float basePow) => new((a) => ExpOut(a, basePow));
        public static Func<float, float> ExpInOut_CreateCustom(float basePow) => new((a) => ExpInOut(a, basePow));


        //Logarithmic
        private const int LOG_BASE = 100;
        private static float LogIn(float value, float logBase)
        {
            float mod = 0.01f * (100f / logBase);
            return Math.Clamp(-MathF.Log(1f - value, logBase) - mod, 0f, 1f);
        }
        private static float LogOut(float value, float logBase)
        {
            float mod = 0.01f * (100f / logBase);
            return Math.Clamp(1f + MathF.Log(value, logBase) + mod, 0f, 1f);
        }
        private static float LogInOut(float value, float logBase)
        {
            if (value < 0.5f) return LogIn(value * 2f, logBase) / 2f;
            else return 0.5f + LogOut((value - 0.5f) * 2f, logBase) / 2f;
        }

        public static float LogIn(float value) => LogIn(value, LOG_BASE);
        public static float LogOut(float value) => LogOut(value, LOG_BASE);
        public static float LogInOut(float value) => LogInOut(value, LOG_BASE);

        public static Func<float, float> LogIn_CreateCustom(float logBase) => new((a) => LogIn(a, logBase));
        public static Func<float, float> LogOut_CreateCustom(float logBase) => new((a) => LogOut(a, logBase));
        public static Func<float, float> LogInOut_CreateCustom(float logBase) => new((a) => LogInOut(a, logBase));


        //Polynomial
        private const int POLYNOMIAL_GRADE = 2;
        private static float PolynomialIn(float value, float grade)
        {
            return MathF.Pow(value, grade);
        }
        private static float PolynomialOut(float value, float grade)
        {
            return 1f - MathF.Pow(1f - value, grade);
        }
        private static float PolynomialInOut(float value, float grade)
        {
            if (value < 0.5f) return PolynomialIn(value * 2f, grade) / 2f;
            else return 0.5f + PolynomialOut((value - 0.5f) * 2f, grade) / 2f;
        }

        public static float PolynomialIn(float value) => PolynomialIn(value, POLYNOMIAL_GRADE);
        public static float PolynomialOut(float value) => PolynomialOut(value, POLYNOMIAL_GRADE);
        public static float PolynomialInOut(float value) => PolynomialInOut(value, POLYNOMIAL_GRADE);

        public static Func<float, float> PolynomialIn_CreateCustom(float grade) => new((a) => PolynomialIn(a, grade));
        public static Func<float, float> PolynomialOut_CreateCustom(float grade) => new((a) => PolynomialOut(a, grade));
        public static Func<float, float> PolynomialInOut_CreateCustom(float grade) => new((a) => PolynomialInOut(a, grade));

        //Quadratic
        public static float QuadraticIn(float value) => PolynomialIn(value, 2);
        public static float QuadraticOut(float value) => PolynomialOut(value, 2);
        public static float QuadraticInOut(float value) => PolynomialInOut(value, 2);

        //Cubic
        public static float CubicIn(float value) => PolynomialIn(value, 3);
        public static float CubicOut(float value) => PolynomialOut(value, 3);
        public static float CubicInOut(float value) => PolynomialInOut(value, 3);


        //Sin
        public static float SinIn(float value)
        {
            return 1f - MathF.Sin((1f - value) * MathHelper.PiOver2);
        }
        public static float SinOut(float value)
        {
            return MathF.Sin(value * MathHelper.PiOver2);
        }
        public static float SinInOut(float value)
        {
            if (value < 0.5f) return SinIn(value * 2) / 2f;
            else return 0.5f + SinOut((value - 0.5f) * 2f) / 2f;
        }
    }
}
