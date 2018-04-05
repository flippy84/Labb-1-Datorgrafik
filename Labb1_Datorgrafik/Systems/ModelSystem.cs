﻿using System;
using Microsoft.Xna.Framework;
using Labb1_Datorgrafik.Components;
using Labb1_Datorgrafik.Managers;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Labb1_Datorgrafik.Systems
{
    class ModelSystem : ISystem, IRender
    {
        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gametime)
        {
            throw new NotImplementedException();
        }

        public void Load(ContentManager content)
        {
            ComponentManager cm = ComponentManager.GetInstance();
            foreach (var entity in cm.GetComponentsOfType<ModelComponent>())
            {
                ModelComponent modelComp = (ModelComponent)entity.Value;
                modelComp.Model = content.Load<Model>(modelComp.ModelPath);
            }
        }

        public void Render(GraphicsDevice gd, BasicEffect be)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            foreach (var model in cm.GetComponentsOfType<ModelComponent>())
            {
                ModelComponent modelComp = (ModelComponent)model.Value;
                if (modelComp.IsActive)
                {
                    TransformComponent transComp = cm.GetComponentForEntity<TransformComponent>(model.Key);

                    float aspectRatio = gd.Viewport.AspectRatio;
                    Matrix[] transforms = new Matrix[modelComp.Model.Bones.Count];
                    modelComp.Model.CopyAbsoluteBoneTransformsTo(transforms);

                    foreach (ModelMesh mesh in modelComp.Model.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {

                            effect.EnableDefaultLighting();
                            effect.View = be.View;
                            effect.Projection = be.Projection;
                            effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(transComp.Position);

                            effect.AmbientLightColor = new Vector3(1f, 0, 0);
                            effect.CurrentTechnique.Passes[0].Apply();
                            mesh.Draw();
                        }
                    }
                }
            }
        }
    }
}
