using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace PlantProtectionTechnologist.Scipts
{
    public class CircleAnimator
    {
        private readonly Ellipse _circle;
        private double _degrees = 0;
        private readonly double _orbitRadius;
        private readonly double _centerX;
        private readonly double _centerY;

        public CircleAnimator(Ellipse circle, double orbitRadius = 40, double centerX = 50, double centerY = 50)
        {
            _circle = circle;
            _orbitRadius = orbitRadius;
            _centerX = centerX;
            _centerY = centerY;
        }

        public void Move()
        {
            double phi = _degrees * Math.PI / 180.0;

            double offsetX = Math.Cos(phi);
            double offsetY = Math.Sin(phi);

            double newLeft = _centerX + offsetX * _orbitRadius - _circle.Width / 2;
            double newTop = _centerY + offsetY * _orbitRadius - _circle.Height / 2;

            Canvas.SetLeft(_circle, newLeft);
            Canvas.SetTop(_circle, newTop);

            _degrees += 2;
            if (_degrees >= 360) _degrees = 0;
        }

        public void Reset()
        {
            _degrees = 0;
        }
    }
}
