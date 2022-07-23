using NUnit.Framework;
using System;

namespace Lection_2_BL.Tests
{
    public class ArrayHelper
    {
        public static int Max(int[] array)
        {
            if(array == null || array.Length == 0)
            {
                throw new ArgumentException();
            }

            int max = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                if(array[i] > max)
                {
                    max = array[i];
                }
            }

            return max;
        }
    }

    public class ArrayHelperTests
    {
        [TestCase(new int[] { 10 }, 10)]
        [TestCase(new int[] { 10, 20 }, 20)]
        [TestCase(new int[] { 10, 11, 7 }, 11)]
        [TestCase(new int[] { 10, 7, 11, -40, 66 }, 66)]
        public void Max_WhenArrayContainsElements_ShouldReturnMaxValue
            (int[] array, int expected)
        {
            //Act
            int actual = ArrayHelper.Max(array);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase(null)]
        [TestCase(new int[0])]
        public void Max_WhenArrayDoesntContainsElements_ShouldThrowArgumentException
            (int[] array)
        {
            Assert.Throws<ArgumentException>(() => ArrayHelper.Max(array));
        }
    }
}