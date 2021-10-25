using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RocketLandingPlatform.Test
{
    public class RocketLandingTest
    {


        /// <summary>
        /// This test method for testing "clash" status. On singelton instance there is a racing situation. This method check racing and successful checkpoint
        /// </summary>
        [Theory]
        [InlineData(10, 10, 10, 10, 11, 10)]
        [InlineData(10, 10, 5, 5, 6, 6)]
        [InlineData(5, 5, 5, 5, 7, 7)]
        public void Load_Matrix_With_Given_LandingArea_Coordinates_And_Check_Given_Points(int platformWidth, int platformHeight, int startPositionX, int startPositionY, int checkPointX, int checkPointY)
        {
            //Arrange

            LandingChecker checker = new(platformWidth, platformHeight, startPositionX, startPositionY);

            Parallel.Invoke(
                () =>
                                {
                                    //Assert
                                    var result = checker.CheckLandingPosition(checkPointX, checkPointY);
                                    Assert.True(result is "clash" or "ok for landing");
                                },
                           () =>
                               {
                                   var newY = checkPointY + 1;
                                   var result = checker.CheckLandingPosition(checkPointX, newY);
                                   //Assert
                                   Assert.True(result is "clash" or "ok for landing");
                               }
                           );



        }


        [Theory]
        [InlineData(10, 10, 10, 10, true)]
        [InlineData(10, 10, 0, 0, true)]
        [InlineData(10, 10, 5, 5, true)]
        public void Load_Matrix_With_Given_LandingArea_Coordinates_Successfully(int platformWidth, int platformHeight, int startPositionX, int startPositionY, bool objectWillBeCreated)
        {
            //Arrange

            LandingChecker checker = new(platformWidth, platformHeight, startPositionX, startPositionY);

            //Act

            Assert.Equal(objectWillBeCreated, checker != null);



        }
        [Theory]
        [InlineData(10, 10, -1, 10, false)]
        [InlineData(-1, 10, 0, 0, false)]
        [InlineData(10, 10, 125, 5, false)]
        [InlineData(105, 10, 125, 5, false)]
        public void Load_Matrix_With_Given_LandingArea_Coordinates_Throws_Exception(int platformWidth, int platformHeight, int startPositionX, int startPositionY, bool objectWillBeCreated)
        {
            //Arrange

            void act()
            {
                _ = new LandingChecker(platformWidth, platformHeight, startPositionX, startPositionY);
            }

            //Act

            //Assert
            var exception = Assert.Throws<InvalidOperationException>(act);

            Assert.Equal(objectWillBeCreated, string.IsNullOrEmpty(exception.Message));



        }
       

        public static IEnumerable<object[]> GetTestDataForMayInlineData()
        {
            yield return new object[] { 10, 10, -1, 10, 5, 5 };
            yield return new object[] { -1, 10, 0, 0, 6, 6, };
            yield return new object[] { 10, 10, 125, 5, 0, 0 };
            yield return new object[] { 105, 10, 125, 5, 2, 2 };
        }
    }
}
