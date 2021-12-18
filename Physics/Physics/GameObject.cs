using System;
using SFML.System;
using SFML.Graphics;

namespace Physics
{
    internal class Box
    {
        Stick[] sticks = new Stick[6];
        public Point[] points = new Point[4];
        Color fillColor;
        float height;
        float width;
        float friction;
        float bounce;
        float gravity;
        public bool drag;

        public Box(Vector2f position, float height, float width, Color fillColor)
        {
            this.fillColor = fillColor;
            this.height = height;
            this.width = width;
            friction = 0.999f;
            bounce = 0.9f;
            gravity = 0.0003f;
            drag = false;
            SetBox(position);
        }

        private void SetBox(Vector2f position)
        {
            //Up left
            points[0] = new Point(position.X, position.Y);
            //Up right
            points[1] = new Point(position.X + width, position.Y);
            //Down right
            points[2] = new Point(position.X + width, position.Y + height);
            //Down left
            points[3] = new Point(position.X, position.Y + height);

            sticks[0] = new Stick(points[0], points[1], fillColor);
            sticks[1] = new Stick(points[1], points[2], fillColor);
            sticks[2] = new Stick(points[2], points[3], fillColor);
            sticks[3] = new Stick(points[3], points[0], fillColor);
            sticks[4] = new Stick(points[3], points[1], fillColor);
            sticks[5] = new Stick(points[0], points[2], fillColor);
        }

        public void Update(Vector2f windowSize, Vector2i mousePos)
        {
            for(int i = 0; i < points.Length; i++)
            {
                if (drag && i != 0)
                {
                    points[i].Update(gravity, friction, bounce, windowSize);
                }
                else if(!drag)
                {
                    points[i].Update(gravity, friction, bounce, windowSize);
                }
                else
                {
                    points[i].x = mousePos.X;
                    points[i].y = mousePos.Y;
                    points[i].KillVelocity();
                }
            }
            foreach (Stick s in sticks)
            {
                s.Update();
            }
        }

        public void Draw(RenderWindow window)
        {
            foreach (Stick s in sticks)
            {
                s.Draw(window);
            }
        }

        public void DrawFill(RenderWindow window)
        {
            Vertex[] vertices = new Vertex[4];
            for(int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Position = new Vector2f(points[i].x, points[i].y);
                vertices[i].Color = fillColor;
            }
            window.Draw(vertices, PrimitiveType.Quads);
        }

        public Vector2f[] GetHitBox()
        {
            //Initialize variable
            float minX = float.PositiveInfinity;
            float minY = float.PositiveInfinity;
            float maxX = float.NegativeInfinity;
            float maxY = float.NegativeInfinity;

            //Get hitbox position
            foreach (Point p in points)
            {
                if (p.x < minX) { minX = p.x; }
                if (p.x > maxX) { maxX = p.x; }
                if (p.y < minY) { minY = p.y; }
                if (p.y > maxY) { maxY = p.y; }
            }

            //Return the hitbox position
            Vector2f[] result = new Vector2f[2];
            result[0] = new Vector2f(minX, minY);
            result[1] = new Vector2f(maxX, maxY);
            return result;
        }
    }

    internal class Stick
    {
        Point[] p = new Point[2];
        float length;
        Color color;

        public Stick(Point p1, Point p2, Color color)
        {
            this.color = color;
            p[0] = p1;
            p[1] = p2;
            float dx = p[1].x - p[0].x;
            float dy = p[1].y - p[0].y;
            this.length = (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public void Update()
        {
            float dx = p[1].x - p[0].x;
            float dy = p[1].y - p[0].y;
            float dlength = (float)Math.Sqrt(dx * dx + dy * dy);
            float difference = length - dlength;
            float percent = difference / dlength / 2;
            float offsetX = dx * percent;
            float offsetY = dy * percent;

            p[1].x += offsetX;
            p[1].y += offsetY;
            p[0].x -= offsetX;
            p[0].y -= offsetY;
        }

        public void Draw(RenderWindow window)
        {
            Vertex[] vertices = new Vertex[2];
            vertices[0].Position = new Vector2f(p[0].x, p[0].y);
            vertices[1].Position = new Vector2f(p[1].x, p[1].y);
            vertices[0].Color = color;
            vertices[1].Color = color;
            window.Draw(vertices, PrimitiveType.Lines);
        }
    }

    internal class Point
    {
        public float x;
        public float y;
        float oldX;
        float oldY;

        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
            oldX = x;
            oldY = y;
        }

        public void SetPosition(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public void SetVelocity(float speed, float heading)
        {
            oldX = x - speed * (float)Math.Cos(heading);
            oldY = y - speed * (float)Math.Sin(heading);
        }

        public void KillVelocity()
        {
            oldX = x;
            oldY = y;
        }

        public void Update(float gravity, float friction, float bounce, Vector2f size)
        {
            //VERLET INTEGRATION
            y += gravity;
            float dx = (x - oldX) * friction;
            float dy = (y - oldY) * friction;
            oldX = x;
            oldY = y;
            x += dx;
            y += dy;

            //CONSTRAINTS
            if (y> size.Y)
            {
                y = size.Y;
                oldY = y + dy * bounce;
            }
            else if (y < 0)
            {
                y = 0;
                oldY = y + dy * bounce;
            }
            if (x > size.X)
            {
                x = size.X;
                oldX = x + dx * bounce;
            }
            else if (x < 0)
            {
                x = 0;
                oldX = x + dx * bounce;
            }
        }

        public void Draw(RenderWindow window)
        {
            Vertex[] vertice = new Vertex[1];
            vertice[0].Position = new Vector2f(x, y);
            vertice[0].Color = Color.White;
            window.Draw(vertice, PrimitiveType.Points);
        }
    }
}
