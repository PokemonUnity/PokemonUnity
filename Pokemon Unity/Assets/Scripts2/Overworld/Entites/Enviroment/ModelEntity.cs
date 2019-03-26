namespace PokemonUnity.Overworld.Entity.Environment
{
public class ModelEntity : Entity
{
    // A private copy of a model, because who needs Inherhitance or OO in general.
    // Just... Microsoft... Why make the Model class NotInheritable
    private Model _model;

    public override void Initialize()
    {
        base.Initialize();

        if (ModelManager.ModelExist(this.AdditionalValue) == true)
            this._model = ModelManager.GetModel(this.AdditionalValue);
        this.NeedsUpdate = true;

        ApplyEffect();
    }

    public void LoadModel(string m)
    {
        this._model = ModelManager.GetModel(m);
        this.AdditionalValue = m;

        ApplyEffect();
    }

    private void ApplyEffect()
    {
        if (_model != null)
        {
            foreach (ModelMesh mesh in this._model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    if (part.Effect.GetType().Name.ToLower() == Screen.Effect.GetType().Name.ToLower())
                    {
                        {
                            var withBlock = (BasicEffect)part.Effect;
                            Lighting.UpdateLighting((BasicEffect)part.Effect, true);

                            withBlock.DiffuseColor = Screen.Effect.DiffuseColor;

                            if (Screen.Level.World != null)
                            {
                                if (Screen.Level.World.EnvironmentType == P3D.World.EnvironmentTypes.Outside)
                                    withBlock.DiffuseColor *= SkyDome.GetDaytimeColor(true).ToVector3();
                            }

                            withBlock.FogEnabled = true;
                            withBlock.FogColor = Screen.Effect.FogColor;
                            withBlock.FogEnd = Screen.Effect.FogEnd;
                            withBlock.FogStart = Screen.Effect.FogStart;
                        }
                    }
                }
            }
        }
    }

    public override void Update()
    {
        ViewBox = new BoundingBox(Vector3.Transform(new Vector3(-1, -1, -1), Matrix.CreateScale(viewBoxScale) * Matrix.CreateTranslation(Position)), Vector3.Transform(new Vector3(1, 1, 1), Matrix.CreateScale(viewBoxScale) * Matrix.CreateTranslation(Position)));

        ApplyEffect();
    }

    public override void Render()
    {
        if (Visible == true)
        {
            if (_model != null)
                _model.Draw(this.World, Screen.Camera.View, Screen.Camera.Projection);

            if (drawViewBox == true)
                BoundingBoxRenderer.Render(ViewBox, Core.GraphicsDevice, Screen.Camera.View, Screen.Camera.Projection, UnityEngine.Color.red);
        }
    }
}
}