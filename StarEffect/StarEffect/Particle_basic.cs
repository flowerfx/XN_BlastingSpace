using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Particle_Demo
{
    public class Particle_basic
    {
	// The position of the particle
	public Vector2 position { get; set; }
	//Where to move the particle
	public Vector2 direction {get; set; }
    // The lifetime of the particle
    public int lifeTime { get; set; }
	// The texture that will be drawn to represent the particle
	public Texture2D texture { get; set; }

	public Particle_basic(Texture2D texture, Vector2 position, Vector2 direction, int lifeTime)
	{
		this.texture = texture;
		this.position = position;
		this.direction = direction;
		this.lifeTime = lifeTime;
	}

	public void Update()
	{
		//We reduce the lifeTime of the particle
		lifeTime--;
		// Updating the position in the desired direction
		position += direction;
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		//Basic drawing
		spriteBatch.Draw(texture, position, Color.YellowGreen);
	}
}
}
