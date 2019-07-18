using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace mittspela
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 


    public class Skott : Microsoft.Xna.Framework.Game
    {


        public Texture2D Texture_Skott;
        public Vector2 postion_Skott, movmentSpeed_Skott;
        public Rectangle skottRec;


        


        //enemy rörlse


        public Skott(Texture2D new_Texture_Skott, Vector2 new_postion_Skott, Vector2 Momvemnt_skott, Rectangle krock_skott )
        {
            Texture_Skott = new_Texture_Skott;


            postion_Skott = new Vector2(new_postion_Skott.X, new_postion_Skott.Y);

            movmentSpeed_Skott = new Vector2(Momvemnt_skott.X, Momvemnt_skott.Y);

        }



        public void Update(GraphicsDevice graphics, GameTime gameTime)
        {


            skottRec = new Rectangle((int)postion_Skott.X, (int)postion_Skott.Y, 10, 10);
            //kontroll av tid för slummäsig rörlse



            // nya värde för vart enemy ska ta vägen

            postion_Skott += new Vector2(movmentSpeed_Skott.X, movmentSpeed_Skott.Y);




        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture_Skott, postion_Skott, Color.White);

        }

    }


    public class Enemies : Microsoft.Xna.Framework.Game
    {
        

        public Texture2D Texture_enemy;
        public Vector2 postion_enemy, velocity_enemy, movmentSpeed_enemy;
        public Rectangle EnemyRec;


        Random myRandom = new Random();
        double timeSinceLastUpdate = 0;
        double millisecondsPerFrame = 500;

        //enemy rörlse




        void randomload()
        {
            int random = myRandom.Next(0, 4);

            if (random == 0)

                velocity_enemy.Y = -movmentSpeed_enemy.Y;



            if (random == 1)

                velocity_enemy.Y = movmentSpeed_enemy.Y;


            if (random == 2)

                velocity_enemy.X = movmentSpeed_enemy.X;


            if (random == 3)
            {
                velocity_enemy.X = -movmentSpeed_enemy.X;

            }
            
        }


        public Enemies(Texture2D new_Texture_enemy, Vector2 new_postion_enemy,Rectangle krock)
        {
            Texture_enemy = new_Texture_enemy;



            //postion_enemy = new Vector2(randX, randY);
            velocity_enemy = Vector2.Zero;
            movmentSpeed_enemy = new Vector2(1, 1);

            int random = myRandom.Next(0, 4);

            int randx = myRandom.Next(50, 740);
            int randy = myRandom.Next(25, 350);

            if (random == 0)
            {
                postion_enemy = new Vector2(50, randy);

            }

            else if (random == 1)

            {
                postion_enemy = new Vector2(randx, 25);

            }
            else if (random == 2)
            {
                postion_enemy = new Vector2(700, randy);

            }

            else if (random == 3)
            {
                postion_enemy = new Vector2(randx, 375);

            }


        }

        void väggkollision()
        {
            //vägg kollsion enemy
            if (postion_enemy.Y <= 0)
                velocity_enemy.Y = movmentSpeed_enemy.Y;

            if (postion_enemy.Y >= 425 )
                velocity_enemy.Y = -movmentSpeed_enemy.Y;

            if (postion_enemy.X <= 0)
                velocity_enemy.X = movmentSpeed_enemy.X;

            if (postion_enemy.X >= 740)
                velocity_enemy.X = -movmentSpeed_enemy.X;




        }




        public void Update (GraphicsDevice graphics, GameTime gameTime)
        {

            EnemyRec = new Rectangle((int)postion_enemy.X, (int)postion_enemy.Y, 50, 50);

            //kontroll av tid för slummäsig rörlse

            timeSinceLastUpdate += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timeSinceLastUpdate >= millisecondsPerFrame)
            {
                timeSinceLastUpdate = 0;


               randomload();



            }

            väggkollision();

            // nya värde för vart enemy ska ta vägen
            postion_enemy += velocity_enemy;








        }

        public void Draw (SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture_enemy,postion_enemy,Color.White);

        }

    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //player
        Texture2D player;
        Vector2 postion, velocity, movmentSpeed;




        //Collison 
        public Rectangle playerRec;
        int antal_fiende;
        SpriteFont HUD;
        Vector2 HUDpos;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;
        Rectangle mainFrame;
        // ändring av tid för enemy
        Random myRandom = new Random();

       
        // ändring av tid för flera enemys
        double timeSinceLastUpdate2 = 0;
        double millisecondsPerFrame2 = 3000 ;

        // ändring av tid för flera skott
        double timeSinceLastUpdate_skott = 0;
        double millisecondsPerFrame_skott = 250 ;
        

        // lista över enemy
        List<Enemies> enemies = new List<Enemies>();
        
        // Lista över skott 
        List<Skott> skott = new List<Skott>();


        // ändring





        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            antal_fiende = 0;
            HUDpos.X = 10;
            HUDpos.Y = 10;
            // rörlse player
            postion = new Vector2(Window.ClientBounds.Width/2 , Window.ClientBounds.Height/2 );
            velocity = Vector2.Zero;
            movmentSpeed = new Vector2(2,2);



            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.


            
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // ladda bilden
            background = Content.Load<Texture2D>("golv");

            // Värde för skärmens gränser
            mainFrame = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);



            // player bild
            player = Content.Load<Texture2D>("bild_spelare");
            


            HUD = Content.Load<SpriteFont>("score");



            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            playerRec = new Rectangle((int)postion.X, (int)postion.Y, 50, 50);




            KeyboardState KS = Keyboard.GetState();



            if (KS.IsKeyDown(Keys.Escape))
                this.Exit();







            //  tid kontroll av Enemy rörlse
           

            switch (antal_fiende)
            {
                case 5:
                    millisecondsPerFrame2 = 2500;
                    break;

                case 10:
                    millisecondsPerFrame2 = 2000;
                    break;

                case 15:
                    millisecondsPerFrame2 = 1800;
                    break;

                case 20:
                    millisecondsPerFrame2 = 1500;
                    break;

                case 25:
                    millisecondsPerFrame2 = 1300;
                    break;

                case 30:
                    millisecondsPerFrame2 = 1100;
                    break;

                case 35:
                    millisecondsPerFrame2 = 900;
                    break;

                case 40:
                    millisecondsPerFrame2 = 700;
                    break;

                case 45:
                    millisecondsPerFrame2 = 500;
                    break;
            }
            // tid kontroll av enemy spaning
            timeSinceLastUpdate2 += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timeSinceLastUpdate2 >= millisecondsPerFrame2)
            {
                timeSinceLastUpdate2 = 0;

                enemies.Add(new Enemies(Content.Load<Texture2D>("danger"), new Vector2(50, 50), new Rectangle((int)postion.X, (int)postion.Y, 60, 60)));
            }
           

            // tid kontroll av skott spaning
            timeSinceLastUpdate_skott += gameTime.ElapsedGameTime.TotalMilliseconds ;

            if (timeSinceLastUpdate_skott >= millisecondsPerFrame_skott)
            {
                timeSinceLastUpdate_skott = 0;

                loadskott();
            }


            //velocity = Vector2.Zero;

            if (KS.IsKeyDown(Keys.W))
                velocity.Y = -movmentSpeed.Y;
            if (KS.IsKeyDown(Keys.A))
                velocity.X = -movmentSpeed.X;
            if (KS.IsKeyDown(Keys.S))
                velocity.Y = movmentSpeed.Y;
            if (KS.IsKeyDown(Keys.D))
                velocity.X = movmentSpeed.X;
            postion += velocity;

            //väg kollsion spelare
            if (postion.Y <= 0)
               velocity.Y = movmentSpeed.Y;

            if (postion.Y >= Window.ClientBounds.Height -20)
                velocity.Y = -movmentSpeed.Y;

            if (postion.X <= 0)
                velocity.X = movmentSpeed.X;

            if (postion.X >= Window.ClientBounds.Width - 30)
                velocity.X = -movmentSpeed.X;





            // tar bort enemies när skott rör dem

                for (int s = 0; s < skott.Count; s++)
                {
                    for (int k = 0; k < enemies.Count; k++)
                    {

                    if (enemies[k].EnemyRec.Intersects(playerRec))
                    {
                        Exit();

                    }

                    if (enemies[k].EnemyRec.Intersects(skott[s].skottRec))
                        {


                            enemies.RemoveAt(k);
                            skott.RemoveAt(s);
                            antal_fiende++;

                            s--;
                            k--;

                        }

                    }




            }







            // spwana enemies

            foreach (Enemies enemy in enemies)
                enemy.Update(graphics.GraphicsDevice , gameTime);
                
            // spwana skott



           
            foreach (Skott skott in skott )
                skott.Update(graphics.GraphicsDevice, gameTime);
            



            base.Update(gameTime);
        }



        // laddar enemy function

        public void loadskott()
        {
            KeyboardState KS = Keyboard.GetState();


            // rörlse och skapning av skott
            if (KS.IsKeyDown(Keys.Right))
                skott.Add(new Skott(Content.Load<Texture2D>("bullet"), new Vector2(postion.X, postion.Y) , new Vector2(12, 0), new Rectangle((int)postion.X, (int)postion.Y, 30, 30)));


            if (KS.IsKeyDown(Keys.Up))
                skott.Add(new Skott(Content.Load<Texture2D>("bullet"), new Vector2(postion.X, postion.Y), new Vector2(0, -12), new Rectangle((int)postion.X, (int)postion.Y, 30, 30)));

            if (KS.IsKeyDown(Keys.Down))
                skott.Add(new Skott(Content.Load<Texture2D>("bullet"), new Vector2(postion.X, postion.Y), new Vector2(0, 12), new Rectangle((int)postion.X, (int)postion.Y, 30, 30)));


            if (KS.IsKeyDown(Keys.Left))
                skott.Add(new Skott(Content.Load<Texture2D>("bullet"), new Vector2(postion.X, postion.Y), new Vector2(-12, 0), new Rectangle((int)postion.X, (int)postion.Y, 30, 30)));

        



        }






        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>




        
        protected override void Draw(GameTime gameTime)
        {


            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(background, mainFrame, Color.White);
            spriteBatch.End();


            //Rita skott
            spriteBatch.Begin();
            foreach (Skott skott in skott)
                skott.Draw(spriteBatch);
            spriteBatch.End();



            //Rita spealre
            spriteBatch.Begin();
            spriteBatch.Draw(player, postion, Color.White);
            spriteBatch.End();


            //Rita massa enemys
            spriteBatch.Begin();
            foreach (Enemies enemy in enemies)
                enemy.Draw(spriteBatch);
            spriteBatch.End();

            // Rita Score
            spriteBatch.Begin();
            spriteBatch.DrawString(HUD, "score :" + antal_fiende.ToString(), HUDpos, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
