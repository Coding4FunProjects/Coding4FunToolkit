﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Coding4Fun.Phone.Controls.Toolkit
{
    public struct MotionParameters
    {
        public const double MaximumSpeed = 4000.0;
		public const double ParkingSpeed = 80.0;
		public const double Friction = 0.2;
    }

    public static class PhysicsConstants
    {
		public static double GetStopTime(Point initialVelocity)
		{
			return GetStopTime(initialVelocity, MotionParameters.Friction, MotionParameters.MaximumSpeed, MotionParameters.ParkingSpeed);
		}

    	public static double GetStopTime(Point initialVelocity, double friction, double maximumSpeed, double parkingSpeed)
        {
            // We need to cap the velocity's magnitude at the maximum speed in order not to have an unbounded scrolling velocity.
            double initialVelocityMagnitude =
                Math.Min(
                    Math.Sqrt(initialVelocity.X * initialVelocity.X + initialVelocity.Y * initialVelocity.Y),
					maximumSpeed);

            // The formula is
            //
            //     t_max = ln(gamma / |v_0|) / ln mu
            //
            // where t_max is the stop time, gamma is the parking speed
            // (that is to say, the speed at which we stop the animation),
            // v_0 is the initial velocity, and mu is the friction coefficient.
            //
            // This is derived by solving the below equation for velocity
            //
            //     v = v_0 mu^t
            //
            // for t when |v| = gamma.
            //
            // This parking speed is necessary because the equation for velocity
            // will only asymptotically trend towards 0; it will never reach it.
            //
            // If the parking speed is greater than the initial velocity, we just stop immediately.
            //
            if (parkingSpeed >= initialVelocityMagnitude)
            {
                return 0;
            }
            else
            {
                return Math.Log(parkingSpeed / initialVelocityMagnitude) / Math.Log(friction);
            }
        }

		public static Point GetStopPoint(Point initialVelocity)
		{
			return GetStopPoint(initialVelocity, MotionParameters.Friction, MotionParameters.MaximumSpeed, MotionParameters.ParkingSpeed);
		}

		public static Point GetStopPoint(Point initialVelocity, double friction, double maximumSpeed, double parkingSpeed)
        {
            // We need to cap the velocity's magnitude at the maximum speed in order not to have an unbounded scrolling velocity.
            double initialVelocityMagnitude = Math.Sqrt(initialVelocity.X * initialVelocity.X + initialVelocity.Y * initialVelocity.Y);
            Point cappedInitialVelocity = initialVelocity;

			if (initialVelocityMagnitude > maximumSpeed && initialVelocityMagnitude > 0)
            {
                // We want to cap the magnitude, so multiplying each directional value by
                // the ratio between the maximum speed and the current magnitude accomplishes this.
				cappedInitialVelocity.X *= maximumSpeed / initialVelocityMagnitude;
				cappedInitialVelocity.Y *= maximumSpeed / initialVelocityMagnitude;
            }

            // The formula is
            //
            //     v = v_0 mu^t
            //
            // where v is the velocity at time t, v_0 is the initial velocity,
            // and mu is the friction coefficient.
            //
            // To find the distance reached after a certain amount of time,
            // we integrate
            //
            //     v dt between 0 and t (t being the current time)
            //
            // to get
            //
            //      r = v_0 (mu^t - 1) / ln mu
            //
            // (mu^t - 1) / ln mu is a scalar value, so we only need
            // to calculate it once.
            //
			double initialVelocityCoefficient = (Math.Pow(friction, GetStopTime(cappedInitialVelocity, friction, maximumSpeed, parkingSpeed)) - 1) / Math.Log(friction);

            return new Point(
                cappedInitialVelocity.X * initialVelocityCoefficient,
                cappedInitialVelocity.Y * initialVelocityCoefficient);
        }

		public static IEasingFunction GetEasingFunction(double stopTime)
		{
			return GetEasingFunction(stopTime, MotionParameters.Friction);
		}

    	public static IEasingFunction GetEasingFunction(double stopTime, double friction)
        {
            // From above, we have the equation of position
            //
            //    r = v_0 (mu^t - 1) / ln mu
            //
            // IEasingFunction.Ease() is a method that accepts
            // a normalized time as a parameter
            // (that is, a number between 0.0 and 1.0 such that
            // 0.0 represents the beginning of the animation duration and
            // 1.0 represents the end of the animation duration),
            // and which returns a normalized progress
            // (that is, a number between 0.0 and 1.0 such that
            // 0.0 represents no progress along the animation and
            // 1.0 represents full progress along the animation).
            //
            // In order to get the above equation of position
            // to work as an easing function, we need two things:
            // to normalize the LHS such that it varies between 0.0 and 1.0
            // (corresponding to its initial position and its final position, respectively),
            // and to have it accept as a parameter on the RHS a normalized time,
            // rather than an actual time.
            //
            // First, to get a normalized LHS, we divide |r| by |r_max| (the stop distance as above)
            // to get the normalized position r_n:
            //
            //     |r| / |r_max| = r_n = (mu^t - 1) / (mu^t_max- 1)
            //
            // Now we note that
            //
            //     t = t_n t_max
            //
            // where t_max is the stop time as above and where t_n is the normalized time
            // to get
            //
            //     r_n = (mu^(t_n t_max) - 1) / (mu^t_max- 1)
            //
            // Finally, we can take advantage of the fact that
            //
            //     x^y = e^(y ln x)
            //
            // where e is Euler's number to put this in the form of
            //
            //     r_n = (e^(t_n t_max ln mu) - 1) / (e^(t_max ln mu) - 1)
            //
            // and if we define a = t_max ln mu, then we have
            //
            //     r_n = (e^(a t_n) - 1) / (e^a - 1)
            //
            // which is precisely the form of the exponential easing function.
            // So, we can use an exponential easing function here with its
            // Exponent property set to t_max ln mu, and it will get us what we want.
            //
    		ExponentialEase ee = new ExponentialEase
    		                     	{
										Exponent = stopTime*Math.Log(friction), 
										EasingMode = EasingMode.EaseIn
									};
    		return ee;
        }
    }
}

