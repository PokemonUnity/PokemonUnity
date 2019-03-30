using System;
using UnityEngine;

namespace PokemonUnity.Overworld
{
public class SkyDome : MonoBehaviour
{
    private GameObject SkydomeModel;
    public Texture2D TextureUp;
    public Texture2D TextureDown;
    private Texture2D TextureSun;
    private Texture2D TextureMoon;

    //public float Yaw = 0;

    const bool FASTTIMECYCLE = false;

	//ToDo: Replace with Awake, and use GetComponents below
    public SkyDome()
    {
        //SkydomeModel = Core.Content.Load<Model>(@"SkyDomeResource\SkyDome");

        TextureUp = TextureManager.GetTexture(@"SkyDomeResource\Clouds");
        TextureDown = TextureManager.GetTexture(@"SkyDomeResource\Clouds");
        TextureSun = TextureManager.GetTexture(@"SkyDomeResource\sun");
        TextureMoon = TextureManager.GetTexture(@"SkyDomeResource\moon");

        SetLastColor();
    }

    public void Update()
    {
        //Yaw += 0.0002f;
        //while (Yaw > MathHelper.TwoPi)
        //    Yaw -= MathHelper.TwoPi;
        SetLastColor();

        if (FASTTIMECYCLE)
        {
            Second += 60;
            if (Second == 60)
            {
                Second = 0;
                Minute += 1;
                if (Minute == 60)
                {
                    Minute = 0;
                    Hour += 1;
                    if (Hour == 24)
                        Hour = 0;
                }
            }
        }
    }

    private int Hour = 0;
    private int Minute = 0;
    private int Second = 0;

    private float GetUniversePitch()
    {
        if (FASTTIMECYCLE)
        {
            int progress = Hour * 3600 + Minute * 60 + Second;
            return System.Convert.ToSingle((MathHelper.TwoPi / (double)100) * (progress / (double)86400 * 100));
        }
        else
        {
            int progress = World.SecondsOfDay;
            return System.Convert.ToSingle((MathHelper.TwoPi / (double)100) * (progress / (double)86400 * 100));
        }
    }

    public void Draw(float FOV)
    {
        //if (Core.GameOptions.GraphicStyle == 1)
        //{
        //    if (GameVariables.Level.World.EnvironmentType == World.EnvironmentTypes.Outside)
        //    {
        //        if (World.GetWeatherFromWeatherType(GameVariables.Level.WeatherType) != World.Weathers.Fog)
        //        {
        //            RenderHalf(FOV, MathHelper.PiOver2, System.Convert.ToSingle(GetUniversePitch() + Math.PI), true, TextureSun, 100, this.GetSunAlpha()); // Draw the Sun.
        //            RenderHalf(FOV, MathHelper.PiOver2, System.Convert.ToSingle(GetUniversePitch()), true, TextureMoon, 100, GetStarsAlpha()); // Draw the Moon.
        //            RenderHalf(FOV, MathHelper.PiOver2, System.Convert.ToSingle(GetUniversePitch()), true, TextureDown, 50, GetStarsAlpha()); // Draw the first half of the stars.
        //            RenderHalf(FOV, MathHelper.PiOver2, System.Convert.ToSingle(GetUniversePitch()), false, TextureDown, 50, GetStarsAlpha()); // Draw the second half of the stars.
        //            RenderHalf(FOV, MathHelper.TwoPi - Yaw, 0.0f, true, GetCloudsTexture(), 15, GetCloudAlpha()); // Draw the back layer of the clouds.
        //            RenderHalf(FOV, Yaw, 0.0f, true, TextureUp, 10, GetCloudAlpha()); // Draw the front layer of the clouds.
        //        }
        //    }
        //    else
        //    {
        //        RenderHalf(FOV, Yaw, 0.0f, true, TextureUp, 8.0f, 1.0f);
		//		
        //        if (TextureDown != null)
        //            RenderHalf(FOV, Yaw, 0.0f, false, TextureDown, 8.0f, 1.0f);
        //    }
        //}
    }

    private void RenderHalf(float FOV, float useYaw, float usePitch, bool up, Texture2D texture, float scale, float alpha)
    {
        //float Roll = 0.0f;
        //if (!up)
        //    Roll = (float)Math.PI;
		//
        //var previousBlendState = Core.GraphicsDevice.BlendState;
        //Core.GraphicsDevice.BlendState = BlendState.NonPremultiplied;
		//
        //foreach (ModelMesh ModelMesh in SkydomeModel.Meshes)
        //{
        //    foreach (BasicEffect BasicEffect in ModelMesh.Effects)
        //    {
        //        BasicEffect.World = Matrix.CreateScale(scale) * Matrix.CreateTranslation(new Vector3(GameVariables.Camera.Position.x, -5, GameVariables.Camera.Position.z)) * Matrix.CreateFromYawPitchRoll(useYaw, usePitch, Roll);
		//
        //        BasicEffect.View = GameVariables.Camera.View;
        //        BasicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(FOV), Core.GraphicsDevice.Viewport.AspectRatio, 0.01f, 10000);
		//
        //        BasicEffect.TextureEnabled = true;
        //        BasicEffect.Texture = texture;
        //        BasicEffect.Alpha = alpha;
		//
        //        switch (GameVariables.Level.World.CurrentMapWeather)
        //        {
        //            case World.Weathers.Clear:
        //            case World.Weathers.Sunny:
        //                {
        //                    BasicEffect.DiffuseColor = new Vector3(1f, 1f, 1f);
        //                    break;
        //                }
        //            case World.Weathers.Rain:
        //                {
        //                    BasicEffect.DiffuseColor = new Vector3(0.4f, 0.4f, 0.7f);
        //                    break;
        //                }
        //            case World.Weathers.Snow:
        //                {
        //                    BasicEffect.DiffuseColor = new Vector3(0.8f, .8f, .8f);
        //                    break;
        //                }
        //            case World.Weathers.Underwater:
        //                {
        //                    BasicEffect.DiffuseColor = new Vector3(0.1f, 0.3f, 0.9f);
        //                    break;
        //                }
        //            case World.Weathers.Fog:
        //                {
        //                    BasicEffect.DiffuseColor = new Vector3(0.7f, 0.7f, 0.8f);
        //                    break;
        //                }
        //            case World.Weathers.Sandstorm:
        //                {
        //                    BasicEffect.DiffuseColor = new Vector3(0.8f, 0.5f, 0.2f);
        //                    break;
        //                }
        //            case World.Weathers.Ash:
        //                {
        //                    BasicEffect.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f);
        //                    break;
        //                }
        //            case World.Weathers.Blizzard:
        //                {
        //                    BasicEffect.DiffuseColor = new Vector3(0.6f, 0.6f, 0.6f);
        //                    break;
        //                }
        //        }
		//
        //        if (BasicEffect.DiffuseColor != new Vector3(1f, 1f, 1f))
        //            BasicEffect.DiffuseColor = GetWeatherColorMultiplier(BasicEffect.DiffuseColor);
        //    }
		//
        //    ModelMesh.Draw();
        //}
		//
        //Core.GraphicsDevice.BlendState = previousBlendState;
    }

    private static UnityEngine.Color[] DaycycleTextureData = null;
    private static Texture2D DaycycleTexture = null/* TODO Change to default(_) if this is not a reference type */;
    private static UnityEngine.Color LastSkyColor = new UnityEngine.Color(0, 0, 0, 0);
    private static UnityEngine.Color LastEntityColor = new UnityEngine.Color(0, 0, 0, 0);

    public static UnityEngine.Color GetDaytimeColor(bool shader)
    {
        if (shader)
            return LastEntityColor;
        else
            return LastSkyColor;
    }

    private void SetLastColor()
    {
        if (DaycycleTextureData == null)
        {
            Texture2D DaycycleTexture = TextureManager.GetTexture(@"SkyDomeResource\daycycle");
            DaycycleTextureData = new UnityEngine.Color[DaycycleTexture.width * DaycycleTexture.height - 1 + 1];
            //DaycycleTexture.GetData(DaycycleTextureData);
            SkyDome.DaycycleTexture = DaycycleTexture;
        }

        int pixel = GetTimeValue();

        UnityEngine.Color pixelColor = DaycycleTextureData[pixel];
        if (pixelColor != LastSkyColor)
        {
            LastSkyColor = pixelColor;
            LastEntityColor = DaycycleTextureData[(pixel + DaycycleTexture.width).Clamp(0, DaycycleTexture.width * DaycycleTexture.height - 1)];
        }
    }

    private float GetCloudAlpha()
    {
        switch (GameVariables.Level.World.CurrentMapWeather)
        {
            case World.Weathers.Rain:
            case World.Weathers.Blizzard:
            case World.Weathers.Thunderstorm:
                {
                    return 0.6f;
                }
            case World.Weathers.Snow:
            case World.Weathers.Ash:
                {
                    return 0.4f;
                }
            case World.Weathers.Clear:
                {
                    return 0.1f;
                }
        }
        return 0.0f;
    }

    private float GetStarsAlpha()
    {
        int progress = GetTimeValue();

        if (progress < 360 | progress > 1080)
        {
            int dP = progress;
            if (dP < 360)
                dP = 720 - dP * 2;
            else if (dP > 1080)
                dP = 720 - (1440 - dP) * 2;

            float alpha = System.Convert.ToSingle(dP / (double)720) * 0.7f;
            return alpha;
        }
        else
            return 0.0f;
    }

    private float GetSunAlpha()
    {
        int progress = GetTimeValue();

        if (progress >= 1080 & progress < 1140)
        {
            // Between 6:00:00 PM and 7:00:00 PM, the Sun will fade away with 60 stages:
            float i = progress - 1080;
            float percent = i / (float)60 * 100f;

            return 1.0f - percent / (float)100.0f;
        }
        else if (progress >= 300 & progress < 360)
        {
            // Between 5:00:00 AM and 6:00:00 Am, the Sun will fade in with 60 stages:
            float i = progress - 300;
            float percent = i / (float)60 * 100;

            return percent / (float)100.0f;
        }
        else if (progress >= 1140 | progress < 300)
            // Between 7:00:00 PM and 5:00:00 AM, the Sun will be invisible:
            return 0.0f;
        else
            // Between 6:00:00 AM and 6:00:00 PM, the Sun will be fully visible:
            return 1.0f;
    }

    private Texture2D GetCloudsTexture()
    {
        switch (GameVariables.Level.World.CurrentMapWeather)
        {
            case World.Weathers.Rain:
            case World.Weathers.Blizzard:
            case World.Weathers.Thunderstorm:
            case World.Weathers.Snow:
                {
                    return TextureManager.GetTexture(@"SkyDomeResource\CloudsWeather");
                }
        }
        return TextureUp;
    }

    public Vector3 GetWeatherColorMultiplier(Vector3 v)
    {
        int progress = GetTimeValue();

        float p = 0.0f;

        if (progress < 720)
            p = System.Convert.ToSingle((720 - progress) / (double)720);
        else
            p = System.Convert.ToSingle((progress - 720) / (double)720);

        return new Vector3(v.x + ((1 - v.x) * p), v.y + ((1 - v.y) * p), v.z + ((1 - v.z) * p));
    }

    private int GetTimeValue()
    {
        if (FASTTIMECYCLE)
            return Hour * 60 + Minute;
        else
        {
            if (World.IsMainMenu)
                return 720;
            return World.MinutesOfDay;
        }
    }
}
}