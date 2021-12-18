using System;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace Physics
{
    internal class GameLoop
    {
        //Game window
        GameTime gameTime = new GameTime();
        RenderWindow window;
        uint wWidth, wHeight;
        string wTitle;
        Color clearColor;

        //Input state
        bool MouseLeftPrevState = false;
        Vector2i mousePos;

        //GameObject
        Box box = new Box(new Vector2f(10, 10), 100, 100, new Color(150,20,20));

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="wWidth"> Window width </param>
        /// <param name="wHeight"> Window height </param>
        /// <param name="wTitle"> Window title </param>
        public GameLoop(uint wWidth, uint wHeight, string wTitle)
        {
            window = new RenderWindow(new VideoMode(wWidth, wHeight), wTitle, Styles.None);
            this.wTitle = wTitle;
            this.wHeight = wHeight;
            this.wWidth = wWidth;
            clearColor = Color.Black;
        }

        /// <summary>
        /// Start game loop
        /// </summary>
        public void Run()
        {
            Load();

            while (window.IsOpen)
            {
                //Handles window events 
                window.DispatchEvents();

                //Get elapsed time since last frame
                gameTime.SetElapsedSeconds();
                gameTime.SetInitTime();

                //Update object to render
                Update();

                //Clear frame
                window.Clear(clearColor);
                //Draw frame
                Render();
                //Display fram
                window.Display();
            }
        }

        /// <summary>
        /// Load games elements
        /// </summary>
        void Load()
        {

        }

        /// <summary>
        /// Update games elements
        /// </summary>
        void Update()
        {
            HandleInput();
            box.Update(new Vector2f(window.Size.X, window.Size.Y), mousePos);
        }

        /// <summary>
        /// Render games elements
        /// </summary>
        void Render()
        {
            box.DrawFill(window);
        }

        /// <summary>
        /// Handles input from keyboard/mouse
        /// </summary>
        void HandleInput()
        {
            mousePos.X = Mouse.GetPosition().X - (int)window.Position.X;
            mousePos.Y = Mouse.GetPosition().Y - (int)window.Position.Y;
            Vector2f[] hitbox;

            if (Mouse.IsButtonPressed(Mouse.Button.Left) && !MouseLeftPrevState)
            {
                hitbox = box.GetHitBox();
                if (mousePos.X >= hitbox[0].X && mousePos.X <= hitbox[1].X && mousePos.Y >= hitbox[0].Y && mousePos.Y <= hitbox[1].Y)
                {
                    box.drag = true;
                }
                MouseLeftPrevState = true;
            }
            else if(!Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                box.drag = false;
                MouseLeftPrevState = false;
            }
        }
    }
}
