using System;
using System.ComponentModel;
using System.Linq;
using RocketLandingPlatform.Interfaces;

namespace RocketLandingPlatform
{
    public class LandingChecker : ILandingChecker
    {
        private const int LandingAreaWidth = 100;
        private const int LandingAreaHeight = 100;
        private static int _landingAreaStartPositionX = -1;
        private static int _landingAreaStartPositionY = -1;
        private static LastCheckedPoint _lastCheckedPoint;
        private static int[,] _landingArea;
        static readonly object _lockObject = new();
        /// <summary>
        /// Warning: Inject this service as singleton,otherwise it will not work correctly
        /// <para>The default landing area size is 100 X 100 </para>
        /// <para>Landing platform sizes (platformWidth, platformHeight) can be configure from settings or can vary manually</para>
        /// </summary>
        /// <param name="platformWidth">Landing platform width in unit</param>
        /// <param name="platformHeight">Landing platform height in unit</param>
        /// <param name="landingPlatformStartPositionX"></param>
        /// <param name="landingPlatformStartPositionY"></param>
        public LandingChecker(int platformWidth, int platformHeight, int landingPlatformStartPositionX, int landingPlatformStartPositionY)
        {
            var errors = ValidateParameters(platformWidth, platformHeight, landingPlatformStartPositionX,
                landingPlatformStartPositionY);
            if (!string.IsNullOrEmpty(errors)) throw new InvalidOperationException(errors);

            Init(platformWidth, platformHeight, landingPlatformStartPositionX, landingPlatformStartPositionY);
        }

        /// <summary>
        /// Creates new matrix and checkpoint bject
        /// </summary>
        /// <param name="platformWidth"></param>
        /// <param name="platformHeight"></param>
        /// <param name="landingPlatformStartPositionX"></param>
        /// <param name="landingPlatformStartPositionY"></param>
        private static void Init(int platformWidth, int platformHeight, int landingPlatformStartPositionX, int landingPlatformStartPositionY)
        {

            //Create landing area as matrix and asign landing platform coordinates on
            _landingArea = CreateMatrix(LandingAreaHeight, LandingAreaWidth, platformHeight, platformWidth, landingPlatformStartPositionX, landingPlatformStartPositionY);
            //Create default last check point.Initial values are -1,-1 , this coordinates does not match any area
            _lastCheckedPoint = new LastCheckedPoint(-1, -1); // initial values for last checked point 
        }

        /// <summary>
        /// Creates a new landing are which contains landing platform as matrix by given parameters
        /// </summary>
        /// <param name="areaWith"></param>
        /// <param name="areaHeight"></param>
        /// <param name="platformWith"></param>
        /// <param name="platformHeight"></param>
        /// <param name="landingPlatformStartPositionX"></param>
        /// <param name="landingPlatformStartPositionY"></param>
        /// <returns></returns>
        private static int[,] CreateMatrix(int areaWith, int areaHeight, int platformWith, int platformHeight, int landingPlatformStartPositionX, int landingPlatformStartPositionY)
        {
            var area = new int[areaWith, areaHeight];
            for (var i = 0; i < areaWith; i++)
            {
                for (var j = 0; j < areaHeight; j++)
                {
                    if (i >= landingPlatformStartPositionX && i < platformWith + landingPlatformStartPositionX && j >= landingPlatformStartPositionY && j < platformHeight + landingPlatformStartPositionY)
                    {
                        area[i, j] = 1;
                    }
                    else
                    {
                        area[i, j] = 0;
                    }
                }
            }

            _landingAreaStartPositionX = landingPlatformStartPositionX;
            _landingAreaStartPositionY = landingPlatformStartPositionY;
            return area;
        }

        /// <summary>
        /// Search given point on Landing area and controls if it is in Landing platform
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public string CheckLandingPosition(int x, int y)
        {
            lock (_lockObject)
            {
                var message = string.Empty;

                var rowLength = _landingArea.GetLength(0);
                var colLength = _landingArea.GetLength(1);

                if (x < 0 || y < 0)
                {
                    throw new InvalidOperationException("Invalid Coordinate(s)");
                }

                if (rowLength <= x || colLength <= y)
                {
                    throw new InvalidOperationException("Coordinates are out of range");
                }

                var finishSearching = false;
                var inLandingPlatform = false;

                for (var i = _landingAreaStartPositionX; i < rowLength; i++)
                {
                    //If searching operation which checking points is finished break
                    if (finishSearching) break;

                    for (var j = _landingAreaStartPositionY; j < colLength; j++)
                    {
                        //The point to be checked
                        if (i == x && j == y)
                        {

                            //Control last checked point, is there any clash. If not then return "Ok for landing" message else "clash"
                            message = !_lastCheckedPoint.NeighborCoordinates.Any(c => c.X == x && c.Y == y) ? "ok for landing" : "clash";

                            lock (_lastCheckedPoint)
                            {
                                //Set last checked point
                                _lastCheckedPoint = new LastCheckedPoint(x, y);
                            }

                            //set flag of finishing operation
                            finishSearching = true;

                            //Set out of platform flag for control
                            inLandingPlatform = true;
                            break;
                        }

                    }
                }

                if (!inLandingPlatform)
                {
                    message = "out of platform";
                }
                return message;
            }


        }

        private static string ValidateParameters(int platformWidth, int platformHeight, int landingPlatformStartPositionX, int landingPlatformStartPositionY)
        {
            var errors = string.Empty;

            if (platformWidth > LandingAreaWidth || platformHeight > LandingAreaHeight)
                errors += "Landing platform height or width can not be grater than  landing area dimensions";

            if (LandingAreaHeight - landingPlatformStartPositionY < platformHeight)
            {
                errors += "Wrong start position for landingPlatformStartPositionX ";
            }

            if (LandingAreaWidth - landingPlatformStartPositionX < platformWidth)
            {
                errors += "Wrong start position for landingPlatformStartPositionY ";
            }
            if (platformWidth <= 0 || platformHeight <= 0 || landingPlatformStartPositionX < 0 || landingPlatformStartPositionY < 0)
            {
                errors += "invalid coordinate(s)!";
            }

            return errors;
        }
    }
}
