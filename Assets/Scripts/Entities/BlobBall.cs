// Created by Binh Bui on 07, 03, 2021

using UnityEngine;
using UnityEngine.U2D;

namespace Entities
{
    public class BlobBall : Ball
    {
        public SpriteShapeController spriteShape;
        public Transform[] points;

        public CircleCollider2D[] colliders;

        private const float SplineOffset = 0.5f;

        protected override void Awake()
        {
            base.Awake();
            UpdateVertices();
        }

        protected override void Update()
        {
            base.Update();
            UpdateVertices();
        }

        private void UpdateVertices()
        {
            for (var i = 0; i < points.Length-1; i++)
            {
                Vector2 vertex = points[i].localPosition;
                Vector2 towardsCenter = (Vector2.zero - vertex).normalized;

                var colliderRadius = colliders[i].radius;
                try
                {
                    spriteShape.spline.SetPosition(i, (vertex - towardsCenter * colliderRadius));
                }
                catch
                {
                    // Spline points are too close to each other;
                    spriteShape.spline.SetPosition(i, (vertex - towardsCenter * (colliderRadius + SplineOffset)));
                }

                Vector2 lt = spriteShape.spline.GetLeftTangent(i);
                Vector2 newRt = Vector2.Perpendicular(towardsCenter) * lt.magnitude;
                Vector2 newLt = Vector2.zero - newRt;

                spriteShape.spline.SetRightTangent(i, newRt);
                spriteShape.spline.SetLeftTangent(i, newLt);
            }
        }
    }
}